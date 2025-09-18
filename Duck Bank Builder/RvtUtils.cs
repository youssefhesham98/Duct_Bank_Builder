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
using Reference = Autodesk.Revit.DB.Reference;

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

        public static void CreateDB(Document doc, UIDocument uidoc/*,List<Element> beams,int count, List<int> userselections*/)
        {
            var pickedRef = uidoc.Selection.PickObjects(ObjectType.Element, "Select a structural framing element");

            Schema schema = Schema.Lookup(new Guid("D1B2A3C4-E5F6-4789-ABCD-1234567890AB"));
            if (schema == null)
            {
                schema = EextensibleStorage.CreateSchema();
            }

            foreach (var duct in pickedRef)
            {
                Element element = doc.GetElement(duct);
                Data.Beams.Add(element);
                if (element != null)
                {
                    //foreach (var userselection in userselections)
                    //{
                    //string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RevitEntityExport.xml");

                    //bool status = true;
                    //Schema schema = EextensibleStorage.CreateSchema();
                    //EextensibleStorage.WriteInstallationData(duct, count,userselections);
                    //Entity Read_entity_ = EextensibleStorage.ReadInstallationData(duct);
                    //Data.listST.Add(Read_entity_);
                    //}

                    Entity entity = new Entity(schema);

                    //Entity entity = element.GetEntity(schema);

                    entity.Set("Author", "EDECS BIM UNIT");
                    entity.Set("Version", 1);
                    entity.Set("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    entity.Set("ElemedID", element.Id);

                    if (element != null)
                    {
                        for (int i = 1; i <= 20; i++)
                        {
                            entity.Set($"Core_{i:00}", "False");
                        }
                    }

                    using (Transaction t = new Transaction(element.Document, "Write Installation Data"))
                    {
                        t.Start();
                        element.SetEntity(entity);
                        t.Commit();
                    }

                    Data.listST.Add(entity);
                    Data.beams_entities[element] = entity;
                }
            }
        }

        //public static void WriteDB(Document doc, UIDocument uidoc, int userInput, List<Element> beams, string xml_path, string excel_path)
        //{
        //    //string xml_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RevitEntityExport.xml");
        //    //string excel_path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RevitEntityExport.xlsx");
        //    //string xml_path = @"C:\Users\y.hesham\Desktop\RevitEntityExport.xml";
        //    //string excel_path = @"D:\RevitEntityExport.xlsx";
        //    var pickedRef = uidoc.Selection.PickObjects(ObjectType.Element, "Select a structural framing element");
        //    foreach (var ele in pickedRef)
        //    {
        //        Element element = doc.GetElement(ele);
        //        Data.Beams.Add(element);
        //    }

        //    //Schema schema = EextensibleStorage.CreateSchema();
        //    EextensibleStorage.WriteInstallationData(userInput, beams);
        //    foreach (var beam in beams)
        //    {
        //        Data.listST.Add(EextensibleStorage.ReadInstallationData(beam));
        //    }

        //    EextensibleStorage.xmlexporter(Data.listST, xml_path);
        //    try
        //    {
        //        EextensibleStorage.excelexporter(Data.listST, excel_path);
        //    }
        //    catch (Exception ex)
        //    {
        //        TaskDialog.Show("Error", ex.Message);
        //    }
        //}

        public static List<int> GetBankData(Element ele, out List<string> ptsdata, out int CoresCount)
        {
            List<int> data = new List<int>();
            ptsdata = new List<string>();
            CoresCount = 0;
            int rows = 0;
            int columns = 0;
            int matrix = 0;
            double tolerance = 1e-6;
            //Data.startpts_ = new Dictionary<int, XYZ>();
            //Data.endpts_ = new Dictionary<int, XYZ>();

            //Dictionary<int, List<XYZ>> beamStartPoints = new Dictionary<int, List<XYZ>>();
            //Dictionary<int, List<XYZ>> beamEndPoints = new Dictionary<int, List<XYZ>>();

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

                    // Get cylinder radius directly
                    XYZ radiusFt = cylFace.get_Radius(0); // radius in feet
                    double rd_length = radiusFt.GetLength();
                    double areaFt2 = Math.PI * rd_length * rd_length; // circular base area in ft²

                    // Convert ft² to mm²
                    // 1 foot = 304.8 mm
                    // Convert ft² → m²
                    //1 foot = 0.3048 meters
                    //1 square foot = 0.092903 square meters
                    // 1 ft² = (304.8)² mm² = 92903.04 mm²
                    double aream2 = areaFt2 * 0.092903;

                    ptsdata.Add($"Circular Area (m²): {Math.Round(aream2, 3)}");

                    //int area = cylFace.Area > 0 ? (int)Math.Round(cylFace.Area) : 0;
                    //ptsdata.Add($"Area: {area}");

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

                    //// Sort startorigins by Z, then by Y
                    //startorigins = startorigins
                    //    .OrderBy(p => p.Z)
                    //    .ThenBy(p => p.Y)
                    //    .ToList();

                    //// Sort endorigins by Z, then by Y
                    //endorigins = endorigins
                    //    .OrderBy(p => p.Z)
                    //    .ThenBy(p => p.Y)
                    //    .ToList();

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
                    CoresCount = ptscount;
                    var rowpts = uniqueStartZCount;
                    rows = rowpts;
                    var colpts = ptscount / uniqueStartZCount;
                    columns = colpts;


                    //Data.points_count = ptscount;

                    //Data.startpts = new XYZ[rowpts, colpts];
                    //Data.endpts = new XYZ[rowpts, colpts];

                    //for (int i = 0; i < rowpts; i++)
                    //{aq
                    //    for (int j = 0; j < colpts; j++)
                    //    {
                    //        int index = i * colpts + j; // calculate 1D index
                    //        Data.startpts[i, j] = startorigins[index];
                    //    }
                    //}

                    //for (int i = 0; i < rowpts; i++)
                    //{
                    //    for (int j = 0; j < colpts; j++)
                    //    {
                    //        int index = i * colpts + j; // calculate 1D index
                    //        Data.endpts[i, j] = endorigins[index];
                    //    }
                    //}
                }
                data.Add(CoresCount);
                data.Add(rows);
                data.Add(columns);
                data.Add(rows * columns);
            }
            return data;
        }

        public static void SetCoresDta(Document doc,IList<Reference> pickedRef, int userInput)
        {
            StringBuilder sb = new StringBuilder();
            Schema schema = Schema.Lookup(new Guid("EA3711B6-7914-4A27-BFA7-9D6973EEB238"));

            int coresPerBeam = userInput;

            foreach (var beam in pickedRef)
            {
                Element element = doc.GetElement(beam);
                Entity entity = element.GetEntity(schema);
                //Entity entity = new Entity(schema);

                int assigned = userInput;

                // Walk all cores in order
                string fieldName = $"Cores";
                string currentValue = entity.Get<string>(fieldName);

                List<CoresData> cores = Data.beams_cores[element.Id];

                ElementId elemedid = element.Id;
                string author = "EDECS BIM UNIT";
                sb.AppendLine($"Author: {author}");
                var schemaGUID = schema.GUID;
                sb.AppendLine($"GUID: {schemaGUID}");
                string schemaName = schema.SchemaName;
                sb.AppendLine($"Schema Name: {schemaName}");
                int version = 1;
                sb.AppendLine($"Version: {version}");
                string createdOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                sb.AppendLine($"Created on: {createdOn}");
                List<string> ptsdata;
                int CoresCount;
                var matrix = GetBankData(element, out ptsdata, out CoresCount);
                for (int i = 0; i < CoresCount; i++)
                {
                    if (currentValue.Contains("Status: False") && assigned != 0)
                    {
                        CoresData core = cores[i];
                        //CoresData core = new CoresData(author, schemaGUID.ToString(), schemaName, version, createdOn);
                        core.Space = "0" + "%";
                        core.Origin = ptsdata[0];
                        core.Area = ptsdata[1];
                        core.Startpt = ptsdata[2];
                        core.Endpt = ptsdata[3];
                        core.IsFilled = true;
                        core.PtsCouunt = matrix[0];
                        core.Rows = matrix[1];
                        core.Columns = matrix[2];
                        core.Matrix = matrix[3];
                        sb.AppendLine($"Core_{i:00}");
                        sb.AppendLine("---------------------------");
                        sb.AppendLine($"Space: {core.Space}");
                        sb.AppendLine($"{core.Origin}");
                        sb.AppendLine($"{core.Area}");
                        sb.AppendLine($"{core.Startpt}");
                        sb.AppendLine($"{core.Endpt}");
                        sb.AppendLine($"Status: {core.IsFilled}");
                        sb.AppendLine($"Cores Count: {core.PtsCouunt}");
                        sb.AppendLine($"Rows: {core.Rows}");
                        sb.AppendLine($"Columns: {core.Columns}");
                        sb.AppendLine($"Matrix: {core.Matrix}");
                        sb.AppendLine("---------------------------");

                        entity.Set("Cores", sb.ToString());
                        assigned--;
                    }
                    else
                    {
                        CoresData core = cores[i];
                        core.Space = "0" + "%";
                        core.Origin = ptsdata[0];
                        core.Area = ptsdata[1];
                        core.Startpt = ptsdata[2];
                        core.Endpt = ptsdata[3];
                        core.IsFilled = false;
                        core.PtsCouunt = matrix[0];
                        core.Rows = matrix[1];
                        core.Columns = matrix[2];
                        core.Matrix = matrix[3];
                        sb.AppendLine($"Core_{i:00}");
                        sb.AppendLine("---------------------------");
                        sb.AppendLine($"Space: {core.Space}");
                        sb.AppendLine($"{core.Origin}");
                        sb.AppendLine($"{core.Area}");
                        sb.AppendLine($"{core.Startpt}");
                        sb.AppendLine($"{core.Endpt}");
                        sb.AppendLine($"Status: {core.IsFilled}");
                        sb.AppendLine($"Cores Count: {core.PtsCouunt}");
                        sb.AppendLine($"Rows: {core.Rows}");
                        sb.AppendLine($"Columns: {core.Columns}");
                        sb.AppendLine($"Matrix: {core.Matrix}");
                        sb.AppendLine("---------------------------");
                        entity.Set("Cores", sb.ToString());
                    }
                    
                }

                Data.beams_entities[element] = entity;
                using (Transaction t = new Transaction(element.Document, "Write Installation Data"))
                {
                    t.Start();
                    element.SetEntity(entity);
                    t.Commit();
                }  
            }
        }
    }
}
