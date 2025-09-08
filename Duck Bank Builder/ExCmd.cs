using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Duck_Bank_Builder.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Duck_Bank_Builder
{
    [Transaction(TransactionMode.Manual)]
    public class ExCmd : IExternalCommand
    {
        private static Mainform maininterface = null;
        public static Document doc { get; set; }
        public static UIDocument uidoc { get; set; }
        public static UIApplication uiapp { get; set; }
        public static ExEvt exevt { get; set; }
        public static ExternalEvent exevthan { get; set; }
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            uidoc = commandData.Application.ActiveUIDocument;
            doc = uidoc.Document;

            uiapp = commandData.Application;
            uiapp.PostCommand(RevitCommandId.LookupPostableCommandId(PostableCommand.AlignedToSelectedLevels));

            Data.Intialize();

            // If the form is already open, close it before opening a new one
            if (maininterface != null && !maininterface.IsDisposed)
            {
                maininterface.Close();
                maininterface.Dispose();
            }

            maininterface = new Mainform();
            maininterface.Show();

            #region ex_ev&ev_han&tns
            exevt = new ExEvt();
            exevthan = ExternalEvent.Create(exevt);

            //using (Transaction tns = new Transaction(doc, "Renamer"))
            //{
            //    tns.Start();

            //    tns.Commit();
            //}
            #endregion

            return Result.Succeeded;
        }
    }
}
