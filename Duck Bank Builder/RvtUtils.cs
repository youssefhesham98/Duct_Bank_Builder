using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Shapes;
using static Autodesk.Revit.DB.SpecTypeId;

namespace Duck_Bank_Builder
{
    public class RvtUtils
    {
        public static void CreatePipes(Document doc, UIDocument uidoc, PipingSystemType systemType, PipeType pipeType, List<int> userselections)
        {
            //foreach (var sel in userselections)
            //{
            //    Data.userselections.Add(sel);
            //}

            var pickedRefs = uidoc.Selection.PickObjects(ObjectType.Element, "Select a structural framing element");
            // convert references to elements
            List<Element> elements = pickedRefs
                .Select(r => doc.GetElement(r))
                .ToList();
            //Element element = doc.GetElement(pickedRef);
            foreach (var element in elements)
            {
                Data.Beams.Add(element);
            }

            double tolerance = 1e-6;
            Data.startpts_ = new Dictionary<int, XYZ>();
            Data.endpts_ = new Dictionary<int, XYZ>();

            Dictionary<int, List<XYZ>> beamStartPoints = new Dictionary<int, List<XYZ>>();
            Dictionary<int, List<XYZ>> beamEndPoints = new Dictionary<int, List<XYZ>>();

            using (Transaction tx = new Transaction(doc, "Create Pipe from Void Axis"))
            {
                tx.Start();
                foreach (var ele in elements)
                {
                    foreach (var geometryinstance in ele.get_Geometry(new Options()).OfType<GeometryInstance>())
                    {
                        Solid solid = geometryinstance.GetInstanceGeometry().OfType<Solid>().FirstOrDefault(s => s.Volume > 0);
                        var origins = solid.Faces.OfType<CylindricalFace>();
                        List<CylindricalFace> uniqueCylFaces = new List<CylindricalFace>();
                        var origins_count = origins.Count() / 2;

                        var startorigins = new List<XYZ>();
                        var endorigins = new List<XYZ>();

                        // keep only unique cylinder axes (avoid duplicate faces)
                        foreach (var face in origins)
                        {
                            XYZ origin = face.Origin;
                            XYZ axis = face.Axis.Normalize();

                            bool exists = uniqueCylFaces.Any(f =>
                                f.Origin.IsAlmostEqualTo(origin, tolerance) &&
                                f.Axis.Normalize().IsAlmostEqualTo(axis, tolerance));

                            if (!exists)
                            {
                                uniqueCylFaces.Add(face);
                            }
                        }

                        foreach (CylindricalFace cylFace in uniqueCylFaces)
                        {
                            XYZ axisDir = cylFace.Axis.Normalize();
                            XYZ origin = cylFace.Origin;

                            // get the param bounds of the cylinder
                            BoundingBoxUV bb = cylFace.GetBoundingBox();
                            UV min = bb.Min;
                            UV max = bb.Max;

                            // axis points at both ends
                            XYZ axisStart = origin + axisDir * min.V;
                            XYZ axisEnd = origin + axisDir * max.V;

                            startorigins.Add(axisStart);
                            endorigins.Add(axisEnd);

                            // Sort startorigins by X, then by Y
                            startorigins = startorigins
                                .OrderBy(p => p.Z)
                                .ThenBy(p => p.Y)
                                .ToList();

                            // Sort endorigins by X, then by Y
                            endorigins = endorigins
                                .OrderBy(p => p.Z)
                                .ThenBy(p => p.Y)
                                .ToList();

                            // Unique Z values in startorigins
                            int uniqueStartZCount = startorigins
                                .Select(p => Math.Round(p.Z, 6)) // round to avoid floating-point noise
                                .Distinct()
                                .Count();

                            // Unique Z values in endorigins
                            int uniqueEndZCount = endorigins
                                .Select(p => Math.Round(p.Z, 6))
                                .Distinct()
                                .Count();

                            Data.points_count = startorigins.Count;
                            Data.row_points = uniqueStartZCount;
                            Data.col_points = Data.points_count / uniqueStartZCount;

                            Data.startpts = new XYZ[Data.row_points, Data.col_points];
                            Data.endpts = new XYZ[Data.row_points, Data.col_points];

                            for (int i = 0; i < Data.row_points; i++)
                            {
                                for (int j = 0; j < Data.col_points; j++)
                                {
                                    int index = i * Data.col_points + j; // calculate 1D index
                                    Data.startpts[i, j] = startorigins[index];
                                    Data.startptsExSt_[$"{i}_{j}"] = startorigins[index];
                                }
                            }

                            for (int i = 0; i < Data.row_points; i++)
                            {
                                for (int j = 0; j < Data.col_points; j++)
                                {
                                    int index = i * Data.col_points + j; // calculate 1D index
                                    Data.endpts[i, j] = endorigins[index];
                                    Data.endptsExSt_[$"{i}_{j}"] = endorigins[index];
                                }
                            }
                            for (int i = 0; i < startorigins.Count; i++)
                            {
                                Data.startpts_[i + 1] = startorigins[i];
                            }
                            for (int i = 0; i < endorigins.Count; i++)
                            {
                                Data.endpts_[i + 1] = endorigins[i];
                            }
                        }

                        Level level = new FilteredElementCollector(doc)
                            .OfClass(typeof(Level))
                            .Cast<Level>()
                            .OrderBy(l => l.Elevation)
                            .FirstOrDefault();

                        if (pipeType == null || systemType == null || level == null)
                        {
                            TaskDialog.Show("Error", "Could not find required PipeType, SystemType, or Level.");
                            return;
                        }
                        foreach (var userselection in userselections)
                        {
                            // Step 6: Create pipe between start and end points
                            if (Data.startpts_.Keys.Count >= userselection && Data.endpts_.Keys.Count >= userselection)
                            {
                                Pipe pipe = Pipe.Create(doc, systemType.Id, pipeType.Id, level.Id, Data.startpts_[userselection], Data.endpts_[userselection]);
                                Data.Pipes.Add(pipe);
                                Data.Cores[userselection] = true;
                                Data.Cores_index[userselection] = pipe;
                                Data.Cores_Pipes[pipe] = ele;
                                Data.corelocations[userselection] = $"{Data.startpts_[userselection]}_{Data.endpts_[userselection]}";
                            }
                        }
                    }
                }
                tx.Commit();
            }
        }

        public static void CreateDB(List<Element> beams,int count, List<int> userselections)
        {
            //Reference pickedRef = uidoc.Selection.PickObject(ObjectType.Element, "Select a structural framing element");
            //Element element = doc.GetElement(pickedRef);
            foreach (var duct in beams)
            {
                if (duct != null)
                {
                    foreach (var userselection in userselections)
                    {
                        string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RevitEntityExport.xml");

                        bool status = true;
                        Schema schema = EextensibleStorage.CreateSchema();
                        //EextensibleStorage.WriteInstallationData(duct, count,userselections);
                        Entity Read_entity_ = EextensibleStorage.ReadInstallationData(duct);
                        Data.listST.Add(Read_entity_);
                    }
                }
            }
        }

        public static void WriteDB(Document doc, UIDocument uidoc, int userInput, List<Element>beams, string xml_path, string excel_path)
        {
            //string xml_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RevitEntityExport.xml");
            //string excel_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RevitEntityExport.xlsx");
            //string xml_path = @"C:\Users\y.hesham\Desktop\RevitEntityExport.xml";
            //string excel_path = @"D:\RevitEntityExport.xlsx";
            var pickedRef = uidoc.Selection.PickObjects(ObjectType.Element, "Select a structural framing element");
            foreach (var ele in pickedRef)
            {
                Element element = doc.GetElement(ele);
                Data.Beams.Add(element);
            }

            Schema schema = EextensibleStorage.CreateSchema();
            EextensibleStorage.WriteInstallationData(userInput, beams);
            foreach (var beam in beams)
            {
                Data.listST.Add(EextensibleStorage.ReadInstallationData(beam));
            }

            EextensibleStorage.xmlexporter(Data.listST, xml_path);
            try
            {
                EextensibleStorage.excelexporter(Data.listST, excel_path);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message);
            }
        }

        public static List<int> GetBankData(Element ele, out List<string> ptsdata)
        {
            List<int> data = new List<int>();
            ptsdata = new List<string>();
            double tolerance = 1e-6;
            Data.startpts_ = new Dictionary<int, XYZ>();
            Data.endpts_ = new Dictionary<int, XYZ>();

            Dictionary<int, List<XYZ>> beamStartPoints = new Dictionary<int, List<XYZ>>();
            Dictionary<int, List<XYZ>> beamEndPoints = new Dictionary<int, List<XYZ>>();

            foreach (var geometryinstance in ele.get_Geometry(new Options()).OfType<GeometryInstance>())
            {
                Solid solid = geometryinstance.GetInstanceGeometry().OfType<Solid>().FirstOrDefault(s => s.Volume > 0);
                var origins = solid.Faces.OfType<CylindricalFace>();
                List<CylindricalFace> uniqueCylFaces = new List<CylindricalFace>();
                var origins_count = origins.Count() / 2;

                var startorigins = new List<XYZ>();
                var endorigins = new List<XYZ>();

                // keep only unique cylinder axes (avoid duplicate faces)
                foreach (var face in origins)
                {
                    XYZ origin = face.Origin;
                    XYZ axis = face.Axis.Normalize();
                  

                    bool exists = uniqueCylFaces.Any(f =>
                        f.Origin.IsAlmostEqualTo(origin, tolerance) &&
                        f.Axis.Normalize().IsAlmostEqualTo(axis, tolerance));

                    if (!exists)
                    {
                        uniqueCylFaces.Add(face);
                    }
                }

                foreach (CylindricalFace cylFace in uniqueCylFaces)
                {
                    XYZ axisDir = cylFace.Axis.Normalize();
                    XYZ origin = cylFace.Origin;
                    ptsdata.Add($"Origin: {origin}, Axis: {axisDir}");
                    int area = cylFace.Area > 0 ? (int)Math.Round(cylFace.Area) : 0;
                    ptsdata.Add($"Area: {area}");

                    // get the param bounds of the cylinder
                    BoundingBoxUV bb = cylFace.GetBoundingBox();
                    UV min = bb.Min;
                    UV max = bb.Max;

                    // axis points at both ends
                    XYZ axisStart = origin + axisDir * min.V;
                    ptsdata.Add($"Start Point: {axisStart}");   
                    XYZ axisEnd = origin + axisDir * max.V;
                    ptsdata.Add($"End Point: {axisEnd}");

                    startorigins.Add(axisStart);
                    endorigins.Add(axisEnd);

                    // Sort startorigins by Z, then by Y
                    startorigins = startorigins
                        .OrderBy(p => p.Z)
                        .ThenBy(p => p.Y)
                        .ToList();

                    // Sort endorigins by Z, then by Y
                    endorigins = endorigins
                        .OrderBy(p => p.Z)
                        .ThenBy(p => p.Y)
                        .ToList();

                    // Unique Z values in startorigins
                    int uniqueStartZCount = startorigins
                        .Select(p => Math.Round(p.Z, 6)) // round to avoid floating-point noise
                        .Distinct()
                        .Count();

                    // Unique Z values in endorigins
                    int uniqueEndZCount = endorigins
                        .Select(p => Math.Round(p.Z, 6))
                        .Distinct()
                        .Count();

                    var ptscount = startorigins.Count;
                    data.Add(ptscount);
                    var rowpts  = uniqueStartZCount;
                    data.Add(rowpts);
                    var colpts = ptscount / uniqueStartZCount;
                    data.Add(colpts);
                    data.Add(rowpts * colpts);

                    Data.points_count = ptscount;

                    Data.startpts = new XYZ[rowpts, colpts];
                    Data.endpts = new XYZ[rowpts, colpts];

                    for (int i = 0; i < rowpts; i++)
                    {
                        for (int j = 0; j < colpts; j++)
                        {
                            int index = i * colpts + j; // calculate 1D index
                            Data.startpts[i, j] = startorigins[index];
                        }
                    }

                    for (int i = 0; i < rowpts; i++)
                    {
                        for (int j = 0; j < colpts; j++)
                        {
                            int index = i * colpts + j; // calculate 1D index
                            Data.endpts[i, j] = endorigins[index];
                        }
                    }
                }
            }
            return data;
        }
    }

}
