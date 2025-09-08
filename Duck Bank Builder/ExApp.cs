using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using System.Reflection;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace Duck_Bank_Builder
{
    public class ExApp : IExternalApplication
    {
        public UIControlledApplication uicapp { get; set; }
        public Result OnShutdown(UIControlledApplication uicapp)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication uicapp)
        {
            //this.uicapp = uicapp;
            //CreateBushButton();
            //return Result.Succeeded;

            LicenseCheck check = new LicenseCheck();
            var begin = check.Check();
            if (begin)
            {
                try
                {
                    this.uicapp = uicapp;
                    CreateBushButton();
                }
                catch (Exception ex)
                {
                    var message = ex.Message;
                    TaskDialog.Show("Error", message);
                }
            }
            return Result.Succeeded;
        }
        private void CreateBushButton()
        {
            try
            {
                var tab_name = "EDECS TOOLKIT";
                var pnl_name = "MEP KIT";
                var btn_name = "DUCT BANK";
                try
                {
                    uicapp.CreateRibbonTab(tab_name);
                }
                catch (Exception ex) { /*TaskDialog.Show("Failed", ex.Message.ToString());*/ }
                List<RibbonPanel> panels = uicapp.GetRibbonPanels(tab_name);
                RibbonPanel panel = panels.FirstOrDefault(p => p.Name == pnl_name);
                if (panel == null)
                {
                    panel = uicapp.CreateRibbonPanel(tab_name, pnl_name);
                }
                //RibbonPanel panel = uicapp.CreateRibbonPanel(pnl_name,tab_name);
                Assembly assembly = Assembly.GetExecutingAssembly();
                // @"Directory\XXXX.dll"
                PushButtonData pd_data = new PushButtonData(btn_name, btn_name, assembly.Location, "Duck_Bank_Builder.ExCmd");
                PushButton pb = panel.AddItem(pd_data) as PushButton;
                if (pb != null)
                {
                    pb.ToolTip = "Duct Bank Builder.";
                    pb.LongDescription = "Duct Bank Builder for Revit MEP";
                    //pb.LargeImage = new BitmapImage(new Uri($@"{Path.GetDirectoryName(assembly.Location)}\pb.png"));
                    //Stream stream = assembly.GetManifestResourceStream("Naming_Convention_Tester.bin.Resources.pb.png");
                    //PngBitmapDecoder decoder = new PngBitmapDecoder(stream , BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                    //pb.LargeImage = decoder.Frames[0];

                    //string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    //ContextualHelp contextHelp = new ContextualHelp(ContextualHelpType.ChmFile, path + "\\myHelp.html"); // hard coding for simplicity. 
                    //pb.SetContextualHelp(contextHelp);

                    pb.LargeImage = GetImageSource("Duck_Bank_Builder.bin.Resources.LOGO.png");
                }
            }
            catch (Exception ex) { TaskDialog.Show("Failed", ex.Message.ToString()); }
        }
        private ImageSource GetImageSource(string ImageFullname)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(ImageFullname);
            PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            // use the extension related to the image extension like PngBitmapDecoder for PNG Image
            return decoder.Frames[0];
        }
    }
}
