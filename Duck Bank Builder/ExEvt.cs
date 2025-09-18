using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Duck_Bank_Builder.UI;

namespace Duck_Bank_Builder
{
    public class ExEvt : IExternalEventHandler
    {
        public Request request { get; set; }
        //public static Document doc { get; set; }
        //public static UIDocument uidoc { get; set; }
        public void Execute(UIApplication app)
        {
            //doc = ExCmd.doc;
            //uidoc = ExCmd.uidoc;
            switch (request)
            {
                case Request.Create_Pipes:
                    RvtUtils.CreatePipes(ExCmd.doc, ExCmd.uidoc, Mainform.pipingsys, Mainform.pipetype,Data.userselections);
                    break;
                case Request.Create_DB:
                    //RvtUtils.CreateDB(ExCmd.doc, ExCmd.uidoc/*,Data.Beams,Data.Pipes.Count,Data.userselections*/);
                    EextensibleStorage.CreateDB(ExCmd.doc,ExCmd.uidoc);
                    break;
                case Request.WriteDB:
                    //RvtUtils.WriteDB(ExCmd.doc, ExCmd.uidoc, Mainform.userselect, Data.Beams,Data.xml_path,Data.excel_path);
                    EextensibleStorage.WriteInstallationData(ExCmd.doc,Mainform.userselect, ExCmd.uidoc);
                    break;
            }
        }

        public string GetName()
        {
            return "EDECS Toolkit";
        }
        public enum Request
        {
            Create_Pipes,
            Create_DB,
            WriteDB,
        }
    }
}
