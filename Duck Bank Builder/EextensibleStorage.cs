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

namespace Duck_Bank_Builder
{
    public class EextensibleStorage
    {
        public static Guid SchemaGuid = new Guid("D1B2A3C4-E5F6-4789-ABCD-1234567890AB");

        public static Schema CreateSchema()
        {
            Schema schema = Schema.Lookup(SchemaGuid);
            if (schema != null) return schema;

            SchemaBuilder sb = new SchemaBuilder(SchemaGuid);
            sb.SetReadAccessLevel(AccessLevel.Public);
            sb.SetWriteAccessLevel(AccessLevel.Public);
            //sb.SetVendorId("YOUR_VENDOR_ID");
            sb.SetDocumentation("Schema for storing installation data on elements");
            sb.AddArrayField("CoreValues", typeof(bool));
            //sb.AddMapField("CoreMap", typeof(string), typeof(bool));
            sb.SetSchemaName("DuckBuilderSchema");
            sb.AddSimpleField("Author", typeof(string));
            sb.AddSimpleField("Version", typeof(int));
            sb.AddSimpleField("CreatedOn", typeof(string));
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
            Schema vv = null;
            try
            {
                vv = sb.Finish();

            }
            catch (Exception ex)
            {

                TaskDialog.Show("Error", ex.Message);
            }
            return vv;
        }

        public static void WriteInstallationData(Element element,int pipeCount, int userselection)
        {
            Schema schema = Schema.Lookup(new Guid("D1B2A3C4-E5F6-4789-ABCD-1234567890AB"));
            if (schema == null)
            {
                schema = CreateSchema();
            }

            Entity entity = new Entity(schema);
            entity.Set("Author", "EDECS BIM UNIT");
            entity.Set("Version", 1);
            entity.Set("CreatedOn", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            entity.Set("ElemedID", element.Id);
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

            //var strb = new StringBuilder();
            //foreach (var pt in Data.startpts)
            //{
            //    var key = Data.startptsExSt_.FirstOrDefault(x => x.Value.Equals(pt)).Key;
            //    strb.AppendLine($"{key}_{status}_{(element.Location as LocationPoint).Point}");
            //}

            //bool status = false;
            //var input = Mainform.usersel;
            // Loop through 20 core fields
            //int i = 1;
            try
            {
                //foreach (var pipe in Data.Pipes)
                //{
                //    bool status = Data.Cores[i];
                //    var pipe_ = Data.Cores_index[i];
                //    var beam = Data.Cores_Pipes[pipe];
                //    if (status == true && pipe_ != null && beam != null) // true if within pipeCount, false otherwise
                //    {
                //        var pt = Data.startpts_[i];
                //        var key = Data.startptsExSt_.FirstOrDefault(x => x.Value.Equals(pt)).Key;
                //        string fieldName = $"Core_{i:00}";
                //        //if (key != null) { }
                //        var value = $"{key}_{status}_{beam.Id}_{pipe.Id}";
                //        //{(element.Location as LocationPoint).Point}
                //        entity.Set(fieldName, value);
                //    }
                //    else
                //    {
                //        var pt = Data.startpts_[i];
                //        var key = Data.startptsExSt_.FirstOrDefault(x => x.Value.Equals(pt)).Key;
                //        string fieldName = $"Core_{i:00}";
                //        status = false;
                //        var value = $"{key}_{status}_{beam.Id}_{pipe.Id}";
                //        entity.Set(fieldName, value);
                //    }
                //    i++;
                //}
                for (int i = 1; i <= 20; i++)
                {
                  foreach (var sel in Data.userselections)
                  {
                        if (sel == i)
                        {
                            bool status = Data.Cores[i];
                            var pipe = Data.Cores_index[i];
                            var beam = Data.Cores_Pipes[pipe];
                            if (status == true && pipe != null && pipe != null) // true if within pipeCount, false otherwise
                            {
                                var pt = Data.startpts_[i];
                                var key = Data.startptsExSt_.FirstOrDefault(x => x.Value.Equals(pt)).Key;
                                string fieldName = $"Core_{i:00}";
                                //if (key != null) { }
                                var value = $"{key}_{status}_{beam.Id}_{pipe.Id}";
                                //{(element.Location as LocationPoint).Point}
                                entity.Set(fieldName, value);
                            }
                            else
                            {
                                var pt = Data.startpts_[i];
                                var key = Data.startptsExSt_.FirstOrDefault(x => x.Value.Equals(pt)).Key;
                                string fieldName = $"Core_{i:00}";
                                status = false;
                                var value = $"{key}_{status}_{beam.Id}_{pipe.Id}";
                                entity.Set(fieldName, value);
                            }
                        }


                    } 
                }
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Key", ex.Message); // Error here
            }

            //entity.Set("Status", "In Progress");
            //entity.Set("Installer", "John Doe");
            //entity.Set("LastUpdated", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));

            using (Transaction t = new Transaction(element.Document, "Write Installation Data"))
            {
                t.Start();
                element.SetEntity(entity);
                t.Commit();
            }
            //return entity;
        }

        public static Entity ReadInstallationData(Element element)
        {
            Schema schema = Schema.Lookup(new Guid("D1B2A3C4-E5F6-4789-ABCD-1234567890AB"));
            if (schema == null) TaskDialog.Show("Error","Schema Failed.");

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

                //string status = entity.Get<string>("Status");
                //string installer = entity.Get<string>("Installer");
                //string lastUpdated = entity.Get<string>("LastUpdated");

                //TaskDialog.Show("Installation Info",
                //$"Status: {status}\nInstaller: {installer}\nLast Updated: {lastUpdated}");

                // Collect the boolean core values

                sb.AppendLine($"Author: {author}");
                sb.AppendLine($"Version: {version}");
                sb.AppendLine($"Created On: {createdOn}");
                sb.AppendLine($"ElementId: {elementid}");
                //sb.AppendLine($"Origin: ({origin?.X:F3}, {origin?.Y:F3}, {origin?.Z:F3})");
                sb.AppendLine();
                sb.AppendLine("Core Values:");
                //sb.AppendLine($"Core_01: {core_01}");
                //sb.AppendLine($"Core_02: {core_02}");
                //sb.AppendLine($"Core_03: {core_03}");
                //sb.AppendLine($"Core_04: {core_04}");
                //sb.AppendLine($"Core_05: {core_05}");
                //sb.AppendLine($"Core_06: {core_06}");
                //sb.AppendLine($"Core_07: {core_07}");
                //sb.AppendLine($"Core_08: {core_08}");
                //sb.AppendLine($"Core_09: {core_09}");
                //sb.AppendLine($"Core_10: {core_10}");
                //sb.AppendLine($"Core_11: {core_11}");
                //sb.AppendLine($"Core_12: {core_12}");
                //sb.AppendLine($"Core_13: {core_13}");
                //sb.AppendLine($"Core_14: {core_14}");
                //sb.AppendLine($"Core_15: {core_15}");
                //sb.AppendLine($"Core_16: {core_16}");
                //sb.AppendLine($"Core_17: {core_17}");
                //sb.AppendLine($"Core_18: {core_18}");
                //sb.AppendLine($"Core_19: {core_19}");
                //sb.AppendLine($"Core_20: {core_20}");

                for (int i = 1; i <= 20; i++)
                {
                    string coreVal = entity.Get<string>($"Core_{i:00}");
                    sb.AppendLine($"  Core_{i:00}: {coreVal}");
                }
            }
            TaskDialog.Show("Extensible Storage Data", sb.ToString());
            return entity;
        }

        public static void ExportEntityToXml(Entity entity, string filePath)
        {
            if (!entity.IsValid())
                throw new InvalidOperationException("Invalid or empty entity.");

            Schema schema = entity.Schema;
            if (schema == null)
                throw new InvalidOperationException("Schema not found.");

            // Root XML element
            XElement root = new XElement("ExtensibleStorage",
                new XAttribute("SchemaName", schema.SchemaName),
                new XAttribute("SchemaGUID", schema.GUID.ToString())
            );

            // Iterate all fields in schema
            foreach (Field field in schema.ListFields())
            {
                string fieldName = field.FieldName;
                Type fieldType = field.ValueType;

                object value = null;
                try
                {
                    // Generic Get<T> using reflection
                    var method = typeof(Entity).GetMethod("Get", new Type[] { typeof(string) });
                    var generic = method.MakeGenericMethod(fieldType);
                    value = generic.Invoke(entity, new object[] { fieldName });
                }
                catch
                {
                    value = "Unsupported type";
                }

                // Write into XML
                root.Add(new XElement("Field",
                    new XAttribute("Name", fieldName),
                    new XAttribute("Type", fieldType.Name),
                    value?.ToString() ?? "null"
                ));
            }

            // Save to file
            XDocument doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);
            doc.Save(filePath);
        }
    }
}
