using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB.Plumbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Duck_Bank_Builder
{
    internal class Data
    {
        // Categories
        public static List<Pipe> Pipes { get; set; }
        public static List<Element> Beams { get; set; }
        public static int col_points { get; set; }
        public static int row_points { get; set; }
        public static int points_count { get; set; }
        public static XYZ [,] startpts { get; set; }
        public static XYZ[,] endpts { get; set; }
        public static Dictionary<int, XYZ> endpts_ { get; set; }
        public static Dictionary<int, bool> Cores { get; set; }
        public static Dictionary<int, Pipe> Cores_index { get; set; }
        public static Dictionary<Pipe, Element> Cores_Pipes { get; set; }
        public static Dictionary<int, XYZ> startpts_ { get; set; }
        public static Dictionary<string, XYZ> startptsExSt_ { get; set; }
        public static Dictionary<string, XYZ> endptsExSt_ { get; set; }
        public static List<PipingSystemType> pipingsys { get; set; }
        public static List<PipeType> pipetypes { get; set; }
        public static List<Entity> listST { get; set; }
        public static List<int> userselections { get; set; }
        public static Dictionary<int, string> corelocations { get; set; }

        public static void Intialize()
        {
            // Categories
            Pipes = new List<Pipe>();
            Beams = new List<Element>();
            pipingsys = new List<PipingSystemType>();
            pipetypes = new List<PipeType>();
            startpts = new XYZ[row_points,col_points];
            endpts = new XYZ[row_points, col_points];
            startpts_ = new Dictionary<int, XYZ>();
            endpts_ = new Dictionary<int, XYZ>();
            startptsExSt_ = new Dictionary<string, XYZ>();
            endptsExSt_ = new Dictionary<string, XYZ>();
            listST = new List<Entity>();
            Cores = new Dictionary<int, bool>();
            Cores_index = new Dictionary<int, Pipe>();
            Cores_Pipes = new Dictionary<Pipe, Element>();
            userselections = new List<int>();
            corelocations = new Dictionary<int, string>();


        }
    }
}
