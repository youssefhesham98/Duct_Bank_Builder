using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Duck_Bank_Builder.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Schema = Autodesk.Revit.DB.ExtensibleStorage.Schema;

namespace Duck_Bank_Builder
{
    public class EextensibleStorage
    {
        public static Guid SchemaGuid = new Guid("D1B2A3C4-E5F6-4789-ABCD-1234567890AB");

        public static Autodesk.Revit.DB.ExtensibleStorage.Schema CreateSchema()
        {
            Autodesk.Revit.DB.ExtensibleStorage.Schema schema = Autodesk.Revit.DB.ExtensibleStorage.Schema.Lookup(SchemaGuid);
            if (schema != null) return schema;

            SchemaBuilder sb = new SchemaBuilder(SchemaGuid);
            sb.SetReadAccessLevel(AccessLevel.Public);
            sb.SetWriteAccessLevel(AccessLevel.Public);
            //sb.SetVendorId("YOUR_VENDOR_ID");
            sb.SetDocumentation("Schema for storing installation data on elements");
            //sb.AddArrayField("CoreValues", typeof(double));
            //sb.AddMapField("CoreMap", typeof(string), typeof(bool));
            sb.SetSchemaName("DuctBuilderSchema");
            sb.AddSimpleField("Author", typeof(string));
            sb.AddSimpleField("Version", typeof(int));
            sb.AddSimpleField("CreatedOn", typeof(string));
            //sb.AddArrayField("Cores", typeof(CoresData));
            sb.AddSimpleField("ElemedID", typeof(ElementId));
            //var location = sb.AddSimpleField("ElementOrigin", typeof(XYZ));
            //location.SetSpec(SpecTypeId.Length);
            sb.AddSimpleField("Core_01", typeof(string));
            sb.AddSimpleField("Core_02", typeof(string));
            sb.AddSimpleField("Core_03", typeof(string));
            sb.AddSimpleField("Core_04", typeof(string));
            sb.AddSimpleField("Core_05", typeof(string));
            sb.AddSimpleField("Core_06", typeof(string));
            sb.AddSimpleField("Core_07", typeof(string));
            sb.AddSimpleField("Core_08", typeof(string));
            sb.AddSimpleField("Core_09", typeof(string));
            sb.AddSimpleField("Core_10", typeof(string));
            sb.AddSimpleField("Core_11", typeof(string));
            sb.AddSimpleField("Core_12", typeof(string));
            sb.AddSimpleField("Core_13", typeof(string));
            sb.AddSimpleField("Core_14", typeof(string));
            sb.AddSimpleField("Core_15", typeof(string));
            sb.AddSimpleField("Core_16", typeof(string));
            sb.AddSimpleField("Core_17", typeof(string));
            sb.AddSimpleField("Core_18", typeof(string));
            sb.AddSimpleField("Core_19", typeof(string));
            sb.AddSimpleField("Core_20", typeof(string));

            // Add fields
            //sb.AddSimpleField("Status", typeof(string));
            //sb.AddSimpleField("Installer", typeof(string));
            //sb.AddSimpleField("LastUpdated", typeof(string));
            Autodesk.Revit.DB.ExtensibleStorage.Schema Sc = null;
            try
            {
                Sc = sb.Finish();

            }
            catch (Exception ex)
            {

                TaskDialog.Show("Error", ex.Message);
            }
            return Sc;
        }

        public static void WriteInstallationData(int userInput, List<Element> beams/*, int pipeCount, List<int> userselections*/)
        {
            Autodesk.Revit.DB.ExtensibleStorage.Schema schema = Autodesk.Revit.DB.ExtensibleStorage.Schema.Lookup(new Guid("D1B2A3C4-E5F6-4789-ABCD-1234567890AB"));
            if (schema == null)
            {
                schema = CreateSchema();
            }

            #region Assign
            //Entity entity = new Entity(schema);
            //entity.Set("Author", "EDECS BIM UNIT");
            //entity.Set("Version", 1);
            //entity.Set("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            //entity.Set("ElemedID", element.Id);
            //var cores = new CoresData[Data.userselections.Count];
            //entity.Set("Cores", cores);
            //entity.Set("ElementOrigin", (element.Location as LocationPoint).Point);
            //entity.Set("Core_01", status);
            //entity.Set("Core_02", status);
            //entity.Set("Core_03", status);
            //entity.Set("Core_04", status);
            //entity.Set("Core_05", status);
            //entity.Set("Core_06", status);
            //entity.Set("Core_07", status);
            //entity.Set("Core_08", status);
            //entity.Set("Core_09", status);
            //entity.Set("Core_10", status);
            //entity.Set("Core_11", status);
            //entity.Set("Core_12", status);
            //entity.Set("Core_13", status);
            //entity.Set("Core_14", status);
            //entity.Set("Core_15", status);
            //entity.Set("Core_16", status);
            //entity.Set("Core_17", status);
            //entity.Set("Core_18", status);
            //entity.Set("Core_19", status);
            //entity.Set("Core_20", status);
            #endregion

            // Loop through 20 core fields
            try
            {
                #region Assign
                //foreach (var beam in Data.Beams)
                //{
                //    if (beam != null)
                //    {
                //        for (int i = 1; i <= 20; i++)
                //        {
                //            entity.Set($"Core_{i:00}", "False");
                //        }
                //    }
                //}
                #endregion

                #region Assign per beam

                //foreach (var beam in beams)
                //{
                //    Entity entity = beam.GetEntity(schema);
                //    entity.Set("Author", "EDECS BIM UNIT");
                //    entity.Set("Version", 1);
                //    entity.Set("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                //    entity.Set("ElemedID", beam.Id);
                //    if (beam != null)
                //    {
                //        int count = 0; // how many "true" we've set so far

                //        for (int i = 1; i <= 20; i++)
                //        {
                //            string fieldName = $"Core_{i:00}";

                //            // get current value
                //            string currentValue = entity.Get<string>(fieldName);

                //            if (currentValue == "True")
                //            {
                //                // already true → skip
                //                continue;
                //            }
                //            else if (count < userInput)
                //            {
                //                // if still need to set more true, do it
                //                entity.Set(fieldName, "True");
                //                count++;
                //            }
                //            else
                //            {
                //                // past the required number → make sure it stays false
                //                entity.Set(fieldName, "False");
                //            }
                //        }
                //    }
                //    Data.beams_entities[beam] = entity;
                //    using (Transaction t = new Transaction(beam.Document, "Write Installation Data"))
                //    {
                //        t.Start();
                //        beam.SetEntity(entity);
                //        t.Commit();
                //    }
                //}
                #endregion

                int coresPerBeam = userInput;

                foreach (var beam in beams)
                {
                    Entity entity = beam.GetEntity(schema);
                    entity.Set("Author", "EDECS BIM UNIT");
                    entity.Set("Version", 1);
                    entity.Set("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                    entity.Set("ElemedID", beam.Id);

                    int assigned = 0;

                    // Walk all cores in order
                    for (int i = 1; i <= 20 && assigned < coresPerBeam; i++)
                    {
                        string fieldName = $"Core_{i:00}";
                        string currentValue = entity.Get<string>(fieldName);

                        if (currentValue == "True")
                        {
                            // already taken → skip
                            continue;
                        }

                        // set new one to True
                        entity.Set(fieldName, "True");
                        assigned++;
                    }

                    Data.beams_entities[beam] = entity;
                    using (Transaction t = new Transaction(beam.Document, "Write Installation Data"))
                    {
                        t.Start();
                        beam.SetEntity(entity);
                        t.Commit();
                    }
                }

                #region Comparing_Try

                //int coresPerBeam = userInput;
                //List<int> lastAssignedIndices = new List<int>();

                //for (int b = 0; b < beams.Count; b++)
                //{
                //    var beam = beams[b];
                //    Entity entity = beam.GetEntity(schema);
                //    entity.Set("Author", "EDECS BIM UNIT");
                //    entity.Set("Version", 1);
                //    entity.Set("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                //    entity.Set("ElemedID", beam.Id);

                //    List<int> assignedIndices = new List<int>();

                //    if (b == 0)
                //    {
                //        // First beam → assign the first free cores
                //        for (int i = 1; i <= 20 && assignedIndices.Count < coresPerBeam; i++)
                //        {
                //            string fieldName = $"Core_{i:00}";
                //            string currentValue = entity.Get<string>(fieldName);

                //            if (currentValue != "True")
                //            {
                //                entity.Set(fieldName, "True");
                //                assignedIndices.Add(i);
                //            }
                //        }
                //    }
                //    else
                //    {
                //        // Next beams → try to reuse the previous indices first
                //        foreach (int idx in lastAssignedIndices)
                //        {
                //            string fieldName = $"Core_{idx:00}";
                //            string currentValue = entity.Get<string>(fieldName);

                //            if (currentValue != "True" && assignedIndices.Count < coresPerBeam)
                //            {
                //                entity.Set(fieldName, "True");
                //                assignedIndices.Add(idx);
                //            }
                //        }

                //        // If not enough cores assigned yet, slide forward to find more
                //        int startIndex = lastAssignedIndices.Count > 0 ? lastAssignedIndices.Last() + 1 : 1;
                //        for (int i = startIndex; i <= 20 && assignedIndices.Count < coresPerBeam; i++)
                //        {
                //            string fieldName = $"Core_{i:00}";
                //            string currentValue = entity.Get<string>(fieldName);

                //            if (currentValue != "True")
                //            {
                //                entity.Set(fieldName, "True");
                //                assignedIndices.Add(i);
                //            }
                //        }
                //    }

                //    // Mark all unassigned cores explicitly as False
                //    for (int i = 1; i <= 20; i++)
                //    {
                //        string fieldName = $"Core_{i:00}";
                //        if (!assignedIndices.Contains(i))
                //            entity.Set(fieldName, "False");
                //    }

                //    lastAssignedIndices = assignedIndices;

                //    Data.beams_entities[beam] = entity;
                //    using (Transaction t = new Transaction(beam.Document, "Write Installation Data"))
                //    {
                //        t.Start();
                //        beam.SetEntity(entity);
                //        t.Commit();
                //    }
                //}


                #endregion

                #region Assign_By_User_Selection
                //foreach (var sel in userselections)
                //{
                //    if (sel != null)
                //    {
                //        bool status = Data.Cores[sel];
                //        var pipe = Data.Cores_index[sel];
                //        var beam = Data.Cores_Pipes[pipe];
                //        var location = Data.corelocations[sel];
                //        if (status == true && pipe != null && pipe != null) // true if within pipeCount, false otherwise
                //        {
                //            var pt = Data.startpts_[sel];
                //            var key = Data.startptsExSt_.FirstOrDefault(x => x.Value.Equals(pt)).Key;
                //            string fieldName = $"Core_{sel:00}";
                //            //if (key != null) { }
                //            var value = $"{key}_{status}_{beam.Id}_{pipe.Id}_{location}";
                //            //{(element.Location as LocationPoint).Point}
                //            entity.Set(fieldName, value);

                //            //for (int i = 0; i < cores.Length; i++)
                //            //{
                //            //    cores[sel] = new CoresData(sel.ToString(),location,status);
                //            //}
                //        }
                //        else
                //        {
                //            var pt = Data.startpts_[sel];
                //            var key = Data.startptsExSt_.FirstOrDefault(x => x.Value.Equals(pt)).Key;
                //            string fieldName = $"Core_{sel:00}";
                //            status = false;
                //            var value = $"{key}_{status}_{beam.Id}_{pipe.Id}_{location}";
                //            entity.Set(fieldName, value);
                //            //cores[sel] = new CoresData(sel.ToString(), location, status);
                //        }
                //    }
                //}
                #endregion

            }
            catch (Exception ex)
            {
                TaskDialog.Show("Key", ex.Message);
            }
        }

        public static Entity ReadInstallationData(Element element)
        {
            Autodesk.Revit.DB.ExtensibleStorage.Schema schema = Autodesk.Revit.DB.ExtensibleStorage.Schema.Lookup(new Guid("D1B2A3C4-E5F6-4789-ABCD-1234567890AB"));
            if (schema == null) TaskDialog.Show("Error", "Schema Failed.");

            StringBuilder sb = new StringBuilder();

            Entity entity = element.GetEntity(schema);
            if (entity.IsValid())
            {
                string author = entity.Get<string>("Author");
                int version = entity.Get<int>("Version");
                string createdOn = entity.Get<string>("CreatedOn");
                ElementId elementid = entity.Get<ElementId>("ElemedID");
                //XYZ origin = entity.Get<XYZ>("ElementOrigin");
                string core_01 = entity.Get<string>("Core_01");
                string core_02 = entity.Get<string>("Core_02");
                string core_03 = entity.Get<string>("Core_03");
                string core_04 = entity.Get<string>("Core_04");
                string core_05 = entity.Get<string>("Core_05");
                string core_06 = entity.Get<string>("Core_06");
                string core_07 = entity.Get<string>("Core_07");
                string core_08 = entity.Get<string>("Core_08");
                string core_09 = entity.Get<string>("Core_09");
                string core_10 = entity.Get<string>("Core_10");
                string core_11 = entity.Get<string>("Core_11");
                string core_12 = entity.Get<string>("Core_12");
                string core_13 = entity.Get<string>("Core_13");
                string core_14 = entity.Get<string>("Core_14");
                string core_15 = entity.Get<string>("Core_15");
                string core_16 = entity.Get<string>("Core_16");
                string core_17 = entity.Get<string>("Core_17");
                string core_18 = entity.Get<string>("Core_18");
                string core_19 = entity.Get<string>("Core_19");
                string core_20 = entity.Get<string>("Core_20");

                //// Collect the boolean core values
                //sb.AppendLine($"Author: {author}");
                //sb.AppendLine($"Version: {version}");
                //sb.AppendLine($"Created On: {createdOn}");
                //sb.AppendLine($"ElementId: {elementid}");
                ////sb.AppendLine($"Origin: ({origin?.X:F3}, {origin?.Y:F3}, {origin?.Z:F3})");
                //sb.AppendLine();
                //sb.AppendLine("Core Values:");

                //for (int i = 1; i <= 20; i++)
                //{
                //    string coreVal = entity.Get<string>($"Core_{i:00}");
                //    sb.AppendLine($"  Core_{i:00}: {coreVal}");
                //}
            }
            //TaskDialog.Show("Extensible Storage Data", sb.ToString());
            return entity;
        }

        /// <summary>
        /// Export entities to XML file
        /// </summary>
        public static void xmlexporter(List<Entity> entities, string xmlPath)
        {
            XElement root = new XElement("ExtensibleStorageData");

            foreach (var entity in entities)
            {
                if (!entity.IsValid()) continue;
                Autodesk.Revit.DB.ExtensibleStorage.Schema schema = entity.Schema;
                if (schema == null) continue;

                XElement entityNode = new XElement("Entity",
                    new XAttribute("SchemaName", schema.SchemaName),
                    new XAttribute("SchemaGUID", schema.GUID.ToString())
                );

                foreach (Autodesk.Revit.DB.ExtensibleStorage.Field field in schema.ListFields())
                {
                    string fieldName = field.FieldName;
                    Type fieldType = field.ValueType;

                    object value = null;
                    try
                    {
                        var method = typeof(Entity).GetMethod("Get", new Type[] { typeof(string) });
                        var generic = method.MakeGenericMethod(fieldType);
                        value = generic.Invoke(entity, new object[] { fieldName });
                    }
                    catch
                    {
                        value = "Unsupported type";
                    }

                    // Skip unwanted fields
                    if ((fieldName == "CoreValues" || fieldName == "ElementOrigin") && value.ToString() == "Unsupported type")
                        continue;

                    entityNode.Add(new XElement("Field",
                        new XAttribute("Name", fieldName),
                        new XAttribute("Type", fieldType.Name),
                        value?.ToString() ?? "null"
                    ));
                }

                root.Add(entityNode);
            }

            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
            doc.Save(xmlPath);
        }

        /// <summary>
        /// Export entities to Excel file
        /// </summary>
        public static void excelexporter(List<Entity> entities, string excelPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("ExtensibleStorage");

                // Step 1: Collect all possible field names (union of all entities)
                HashSet<string> allFields = new HashSet<string>();

                foreach (var entity in entities)
                {
                    if (!entity.IsValid()) continue;
                    Schema schema = entity.Schema;
                    if (schema == null) continue;

                    foreach (var field in schema.ListFields())
                    {
                        string fieldName = field.FieldName;
                        if (fieldName == "CoreValues" || fieldName == "ElementOrigin")
                            continue; // skip unwanted
                        allFields.Add(fieldName);
                    }
                }

                // Step 2: Write header row
                var headers = new List<string> { "SchemaName", "SchemaGUID" };
                headers.AddRange(allFields);

                for (int c = 0; c < headers.Count; c++)
                {
                    ws.Cells[1, c + 1].Value = headers[c];
                }

                // Step 3: Write each entity in one row
                int row = 2;
                foreach (var entity in entities)
                {
                    if (!entity.IsValid()) continue;
                    Schema schema = entity.Schema;
                    if (schema == null) continue;

                    // Fill a dictionary of fieldName → value
                    Dictionary<string, string> values = new Dictionary<string, string>();

                    foreach (var field in schema.ListFields())
                    {
                        string fieldName = field.FieldName;
                        if (fieldName == "CoreValues" || fieldName == "ElementOrigin")
                            continue;

                        Type fieldType = field.ValueType;
                        object value = null;
                        try
                        {
                            var method = typeof(Entity).GetMethod("Get", new Type[] { typeof(string) });
                            var generic = method.MakeGenericMethod(fieldType);
                            value = generic.Invoke(entity, new object[] { fieldName });
                        }
                        catch
                        {
                            value = "Unsupported type";
                        }

                        values[fieldName] = value?.ToString() ?? "null";
                    }

                    // Write Schema info
                    ws.Cells[row, 1].Value = schema.SchemaName;
                    ws.Cells[row, 2].Value = schema.GUID.ToString();

                    // Write field values in their respective columns
                    for (int c = 2; c < headers.Count; c++)
                    {
                        string header = headers[c];
                        if (values.ContainsKey(header))
                            ws.Cells[row, c + 1].Value = values[header];
                    }

                    row++;
                }

                // Save file
                package.SaveAs(new FileInfo(excelPath));
            }
        }

        // Helper Methods
        public static void WritePoint(Entity ent, XYZ point)
        {
            // Convert XYZ to a list of doubles
            List<double> coords = new List<double> { point.X, point.Y, point.Z };
            ent.Set<IList<double>>("PointCoords", coords);
        }
        public static XYZ ReadPoint(Entity ent)
        {
            IList<double> coords = ent.Get<IList<double>>("PointCoords");
            if (coords.Count >= 3)
            {
                return new XYZ(coords[0], coords[1], coords[2]);
            }
            return XYZ.Zero; // fallback
        }
    }
}
