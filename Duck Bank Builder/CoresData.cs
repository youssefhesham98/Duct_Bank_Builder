using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duck_Bank_Builder
{
    public class CoresData
    {
        public static string Author { get; set; }
        public static string SchemaGUID { get; set; }
        public static string SchemaName { get; set; }
        public static int Version { get; set; }
        public static string CreatedOn { get; set; }
        public  string Space { get; set; }
        public  string Origin { get; set; }
        public  bool IsFilled { get; set; }
        public  int PtsCouunt { get; set; }
        public  int Rows { get; set; }
        public  int Columns { get; set; }
        public  int Matrix { get; set; }


        public CoresData(string author,string schemaGUID,string schemaName,int version,string createdOn)
        {
            Author = author;   
            SchemaGUID = schemaGUID;
            SchemaName = schemaName;
            Version = version;
            CreatedOn = createdOn;
            
        }



    }
}
