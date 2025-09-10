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
        public string CoreIndex { get; set; }
        //public ElementId BanksIDs { get; set; }
        public string Origin { get; set; }
        public bool IsFilled { get; set; }
        public string Core_01 { get; set; }


        public CoresData(string coreIndex,string origin,bool isFilled)
        {
            this.IsFilled = isFilled;   
            this.CoreIndex = coreIndex;
            //this.BanksIDs = elementId;
            this.Origin = origin;
            
        }



    }
}
