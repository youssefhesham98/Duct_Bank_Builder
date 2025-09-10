using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using static Duck_Bank_Builder.ExEvt;

namespace Duck_Bank_Builder.UI
{
    public partial class Mainform : System.Windows.Forms.Form
    {
        public static PipingSystemType pipingsys { get; set; }
        public static PipeType pipetype { get; set; }
        public static int usersel{ get; set; }
        //public static List<int> count_list { get; set; }
        public Mainform()
        {
            InitializeComponent();
            
            // -------------------------------------------------- COLLECT
            var pipeTypes = new FilteredElementCollector(ExCmd.doc)
                 .OfClass(typeof(PipeType))
                 .Cast<PipeType>().OrderBy(pt => pt.Name).ToList();

            foreach (var item in pipeTypes)
            {
                Data.pipetypes.Add(item);
            }

            var systemTypes = new FilteredElementCollector(ExCmd.doc)
              .OfClass(typeof(PipingSystemType))
              .Cast<PipingSystemType>().OrderBy(st => st.Name).ToList();

            foreach (var item in systemTypes)
            {
                Data.pipingsys.Add(item);
            }
            // -------------------------------------------------- COLLECT

            pipe_types.DataSource = Data.pipetypes;
            pipe_types.DisplayMember = "Name";
            pipe_types.ValueMember = "Id";
            system_types.DataSource = Data.pipingsys;
            system_types.DisplayMember = "Name";
            system_types.ValueMember = "Id";


            //for (int i = 0; i < 20; i++)
            //{
            //    points.Items.Add(i+1);
            //}

            //points.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
        }

        private void getcenters_Click(object sender, EventArgs e)
        {
            Data.Beams.Clear();
            Data.Pipes.Clear();
            Data.Cores_index.Clear();
            Data.Cores_Pipes.Clear();
            Data.Cores.Clear();
            Data.listST.Clear();
            //RvtUtils.X = surveyx.Text;
            //RvtUtils.Y = surveyy.Text;
            //RvtUtils.Z = internalz.Text;
            //RvtUtils.angle = angle.Text;
            //RvtUtils.InternalOriginX = internalx.Text;
            //RvtUtils.InternalOriginY = internaly.Text;
            pipetype = pipe_types.SelectedItem as PipeType;
            pipingsys = system_types.SelectedItem as PipingSystemType;
            usersel = int.Parse(userselection.Text);

            // Later, get selected indices
            //List<int> selectedIndices = points.CheckedItems
            //    .Cast<int>()
            //    .Select(humanIndex => humanIndex - 1).OrderBy(id => id) // Convert to zero-based
            //    .ToList();
            //count_list = selectedIndices;

            if (pipetype != null && pipingsys != null)
            {
                ExCmd.exevt.request = Request.Create_Pipes;
                ExCmd.exevthan.Raise();
            }

            //foreach (int index in selectedIndices)
            //{
            //    TaskDialog.Show("Index", $"Selected Index: {index}");
            //}
        }

        private void edecs_Click(object sender, EventArgs e)
        {
            string url = @"https://www.edecs.com/";

            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    // Open the URL in the default browser
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true // Required to use the default browser
                    });

                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", $"Error opening the URL: {ex.Message}");
                }
            }
            else
            {
                TaskDialog.Show("Error", "No URL entered.");
            }

    
        }
        private void lnkd_Click(object sender, EventArgs e)
        {
            string url = @"https://www.linkedin.com/in/youssef-hesham/";

            if (!string.IsNullOrEmpty(url))
            {
                try
                {
                    // Open the URL in the default browser
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true // Required to use the default browser
                    });

                }
                catch (Exception ex)
                {
                    TaskDialog.Show("Error", $"Error opening the URL: {ex.Message}");
                }
            }
            else
            {
                TaskDialog.Show("Error", "No URL entered.");
            }
        }

        private void cls_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void userselection_TextChanged(object sender, EventArgs e)
        {

        }

        private void crt_db_Click(object sender, EventArgs e)
        {
            usersel = int.Parse(userselection.Text);
            ExCmd.exevt.request = Request.Create_DB;
            ExCmd.exevthan.Raise();
        }
    }
}
