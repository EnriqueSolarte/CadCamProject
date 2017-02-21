using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CadCamProject
{
    public class PostProcessor
    {
       
        public string gCodeFile { get; set; }
        private FunctionChart code { get; set; }
        

        public PostProcessor()
        {
            code = new FunctionChart(gCodeFile);
        }

        public void Header(Gcode gCode,WorkingPlane wPlane)
        {
            code.NewBlock();
            code.Add(gCode.ToString());
            code.Add(GetGCode(wPlane));
            code.Add("G21 G90 G95");            
        }

        private string GetGCode(WorkingPlane wPlane)
        {
            string _gCode="";

            if (wPlane == WorkingPlane.ZX)
            {
                _gCode = "G18";
            }
            if (wPlane == WorkingPlane.XY)
            {
                _gCode = "G17";
            }
            return _gCode;
        }

      
        public void LineMove()
        {

        }

        public void ArcMove()
        {

        }

        public void RapidMove(CoordinatePoint _point)
        {
            code.NewBlock();
            code.Add("G00");

            X
            code.Add(_point.coord1.ToString);
        }

        public void printGCODE()
        {
            File.WriteAllText("gCode.NC", code.chainData);
           
        }

        internal void StartFile()
        {
            code.Add("%");
        }

        internal void ToolChanging(Tool tool)
        {
            RapidMove(new CoordinatePoint(50, 50));
            code.NewBlock();
            string _tool = "T0" + tool.localization.ToString() +
                            "0" + tool.toolSet.ToString();
            code.Add(_tool);

        }


        internal void EndFile()
        {
            code.Add("M30");
        }
    }
}
