using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static Autodesk.Revit.DB.SpecTypeId;

namespace Duck_Bank_Builder
{
    public class RvtUtils
    {
        public static void CreatePipes(Document doc, UIDocument uidoc, PipingSystemType systemType, PipeType pipeType, int userselection)
        {
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

            List<XYZ> startorigins = new List<XYZ>();
            List<XYZ> endorigins = new List<XYZ>();
            double tolerance = 1e-6;

            Data.startptsExSt_ = new Dictionary<string, XYZ>();
            Data.endptsExSt_ = new Dictionary<string, XYZ>();

            foreach (var ele in Data.Beams)
            {
                foreach (var geometryinstance in ele.get_Geometry(new Options()).OfType<GeometryInstance>())
                {
                    Solid solid = geometryinstance.GetInstanceGeometry().OfType<Solid>().FirstOrDefault(s => s.Volume > 0);
                    var origins = solid.Faces.OfType<CylindricalFace>();
                    List<CylindricalFace> uniqueCylFaces = new List<CylindricalFace>();
                    var origins_count = origins.Count() / 2;



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

                    // Step 6: Create pipe between start and end points
                    using (Transaction tx = new Transaction(doc, "Create Pipe from Void Axis"))
                    {
                        tx.Start();

                        //for (int i = 0; i < /*origins.Count() / 2*/ userselection; i++)
                        //{
                        Pipe pipe = Pipe.Create(doc, systemType.Id, pipeType.Id, level.Id, Data.startpts_[userselection], Data.endpts_[userselection]);
                        Data.Pipes.Add(pipe);
                        //}

                        tx.Commit();
                    }

                    //var sb = new StringBuilder();

                    //for (int i = 0; i < Data.row_points; i++)
                    //{
                    //    for (int j = 0; j < Data.col_points; j++)
                    //    {
                    //        sb.AppendLine($"Start Point[{i},{j}]: {Data.startpts[i, j]}");
                    //        sb.AppendLine($"End Point[{i},{j}]: {Data.endpts[i, j]}");
                    //        // Just for testing
                    //        //TaskDialog.Show("Points", $"Start Point[{i},{j}]: {Data.startpts[i, j]}\nEnd Point[{i},{j}]: {Data.endpts[i, j]}");

                    //    }
                    //}
                    //sb.AppendLine($"Total Points: \n {Data.points_count}, {Data.row_points} * {Data.col_points}");
                    //TaskDialog.Show("Points", sb.ToString());
                }
            }
        }

        public static void CreateDB(List<Element> beams,int count)
        {
            //Reference pickedRef = uidoc.Selection.PickObject(ObjectType.Element, "Select a structural framing element");
            //Element element = doc.GetElement(pickedRef);
            foreach (var duct in beams)
            {
                if (duct != null)
                {
                    string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "RevitEntityExport.xml");

                    bool status = true;
                    Schema schema = EextensibleStorage.CreateSchema();
                    Entity Read_entity = EextensibleStorage.ReadInstallationData(duct);
                    if (!Read_entity.IsValid())
                    {
                        TaskDialog.Show("Export", "No extensible storage found on this element.");
                    }
                    EextensibleStorage.WriteInstallationData(duct, count);
                    Entity Read_entity_ = EextensibleStorage.ReadInstallationData(duct);
                    Data.listST.Add(Read_entity_);

                    EextensibleStorage.ExportEntityToXml(Read_entity_, path);
                    TaskDialog.Show("Export", $"Data exported to:\n{path}");

                    //Entity entity = duct.GetEntity(schema);
                    //duct.SetEntity(entity);
                    //TaskDialog.Show("Entity Data", sb.ToString());
                }
            }
        }
    }

}
