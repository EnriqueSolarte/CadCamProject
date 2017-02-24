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
        private PathDefinition gCodePathFile;
        private int blockNumber=0;

        private WorkingPlane wPlane;
        private string gCode;
        private string mCode;
       
        private WorkSettings wSettings;
        private Turning turning;
        private SpecialsChart sCh;


        private string gCodeFile = "";
        
        public PostProcessor(WorkSettings _wSettings, Turning _turningOp)
        {
            sCh = new SpecialsChart();
            wSettings = _wSettings;
            turning = _turningOp;
            turningOperation();
        }

        private void turningOperation()
        {

            Add(Comment(wSettings.statusBar.version));
            Add("Turning Programing");
            NewLine();
            NewBlock();
            Add("G291");
            NewBlock();

            #region Header
            Add(turning.profile.workOffset.ToString());
            Add("G90 G40");
            Add(GCODE(turning.profile.workingPlane));
            Add(GCODE(wSettings.units));
            Add(GCODE(turning.feedRateType));
            #endregion

            #region Safety Position
            NewBlock();
            Add(RapidMoveTurning(wSettings.safetyPoint, true));
            #endregion

            #region Changing Tool
            NewBlock();
            Add(ChangeTool(turning.tool));
            NewBlock();
            Add(GetSpeedSpindle(turning));
            #endregion

            #region
            NewBlock();
            Add(RapidMoveTurning(turning.apprachingPoint, false));
            NewBlock();
            Add(TurningCycle(turning));
            #endregion

        }

        private string TurningCycle(Turning turning)
        {
            string _data="";

            
            return _data;
        }

        private string RapidMoveTurning(CoordinatePoint safetyPoint, bool safatyMode)
        {
            string _data;
            gCode = Gcode.G00.ToString() + " ";
            string[] _coordinate = GetCoordinateLabel(wPlane);

            if (safatyMode)
            {
                _data = gCode + _coordinate[0] + GetPosition(safetyPoint.coord1);
                blockNumber = blockNumber + 10;
                _data = _data + Environment.NewLine + "N" + blockNumber + sCh.blank;
                _data = _data + _coordinate[1] + GetPosition(safetyPoint.coord2);
            }
            else
            {
                _data = gCode + _coordinate[0] + GetPosition(safetyPoint.coord1) +
                     sCh.blank + _coordinate[1] + GetPosition(safetyPoint.coord2);
            }


            return _data;
        }


        private string GCODE(string _feedRateType)
        {
            switch (_feedRateType)
            {
                case "mm/min":
                    return Gcode.G94.ToString();
                case "mm/rot":
                    return Gcode.G95.ToString();
                default:
                    return "";
            }
        }

        private string GetSpeedSpindle(Turning _turning)
        {
            string _data="";
            mCode = turning.tool.spindleControl.ToString();
            if (_turning.speedControlType == SpeedControl.ConstantSurfaceControl)
            {
                _data = Gcode.G92.ToString() + sCh.blank + "S" + _turning.spindleSpeed.ToString("####");
                blockNumber = blockNumber + 10;
                _data = _data + Environment.NewLine + "N" + blockNumber + sCh.blank;
                _data = _data + "S" + _turning.cuttingSpeed.ToString("####") + sCh.blank +
                    mCode + sCh.blank +
                    Gcode.G96.ToString();
               
            }else
            {
                
                _data = "S" + _turning.spindleSpeed.ToString("####") + sCh.blank +
                      mCode + sCh.blank + Gcode.G97.ToString();
            }

            return _data;
        }

        private string ChangeTool(Tool _tool)
        {
            string _data;
            if(_tool.localization < 10)
            {
                _data = "T0" + _tool.localization.ToString() + "0" + _tool.toolSet;
            }
            else
            {

                _data = "T" + _tool.localization.ToString() + _tool.toolSet;
            }
           
            return _data;
        }

        private string GCODE(WorkingPlane _wPlane)
        {
            wPlane = _wPlane;
            switch (wPlane)
            {
                case WorkingPlane.ZX:
                    return Gcode.G18.ToString();
                case WorkingPlane.XY:
                    return Gcode.G17.ToString();
                default:
                    return "";
                   
            }
        }

        
        private string[] GetCoordinateLabel(WorkingPlane _wPlane)
        {
            
            if (_wPlane== WorkingPlane.ZX)
            {
                string[] _coordinate = {
                    "X",
                    "Z",
                };
                return _coordinate;
            }
            else
            {
                string[] _coordinate = {
                    "X",
                    "Y",
                };
                return _coordinate;
            }
            
           
        }

        private string GetPosition(double coord1)
        {

            return coord1.ToString("###.000");
        }

        private string GCODE(Units _units)
        {
            if(_units == Units.mm)
            {
                return Gcode.G21.ToString();
            }else
            {
                return Gcode.G20.ToString();
            }
        }

        private string Comment(string _comment)
        {
            return ";" + _comment;
        }

        private void Add(string _data)
        {
            gCodeFile = gCodeFile + _data + sCh.blank;
        }

        private void NewBlock()
        {
            blockNumber = blockNumber + 10;
            gCodeFile = gCodeFile + Environment.NewLine + "N" + blockNumber + sCh.blank;
           
        }

        private void NewLine()
        {
            gCodeFile = gCodeFile + Environment.NewLine ;
        }
   
        internal void GetGCode(PathDefinition _gCodePathFile)
        {
            gCodePathFile = _gCodePathFile;
            File.WriteAllText(gCodePathFile.GetFullName(),gCodeFile);
        }
    }

   
}
