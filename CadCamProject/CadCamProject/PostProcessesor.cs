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
       
        private int blockNumber=0;
        private SpecialsChart sCh;


        private string gCodeFile = "";
        
        public PostProcessor(WorkSettings _wSettings, Turning _turningOp)
        {
            sCh = new SpecialsChart();           
            turningOperation(_wSettings,_turningOp);
        }

        private void turningOperation(WorkSettings _wSettings, Turning _turning)
        {

            Add(Comment(_wSettings.statusBar.version));
            Add("Turning Programing");
            NewLine();
            NewBlock();
            Add("G291");
            NewBlock();

            #region Header
            Add(_turning.profile.workOffset.ToString());
            Add("G90 G40");
            Add(GCODE(_turning.profile.workingPlane));
            Add(GCODE(_wSettings.units));
            Add(GCODE(_turning.feedRateType));
            #endregion

            #region Safety Position
            NewBlock();
            Add(TurningRapidMove(_wSettings.safetyPoint, true));
            #endregion

            #region Changing Tool
            NewBlock();
            Add(ChangeTool(_turning.tool));
            NewBlock();
            Add(GetSpeedSpindle(_turning));
            #endregion

            #region Machining
            NewBlock();
            Add(TurningRapidMove(_turning.apprachingPoint, false));
            NewBlock();
            Add(TurningCycle(_turning));
            NewBlock();
            Add(TurningRapidMove(_turning.profile.geometry[0].initialPosition,false));
            Add(ProfileDefinition(_turning));
            #endregion

        }

        private string ProfileDefinition(Turning _turning)
        {
            string _data = "";
            foreach (Geometry geometry in _turning.profile.geometry)
            {
                blockNumber = blockNumber + 10;
                _data = _data + Environment.NewLine + "N" + blockNumber + sCh.blank;
                switch (geometry.typeGeometry.Name)
                {
                    case "Line":
                        _data = _data + TurningLineMove(geometry);
                        break;
                    case "Arc":
                        _data = _data + TurningArcMove(geometry);
                        break;
                }
            }

            return _data;
        }

        private string TurningCycle(Turning _turning)
        {
            string _data="";

            #region First block
            switch (_turning.turningRemovalType)
            {
                case TurningRemovaltype.byLongitudinal:
                    _data = "G71 U" + _turning.cuttingDepth.ToString("##.000") + sCh.blank +
                        "R" + _turning.retracting.ToString("##.000");
                    break;
                case TurningRemovaltype.byTransverse:
                    _data = "G72 W" + _turning.cuttingDepth.ToString("##.000") + sCh.blank +
                        "R" + _turning.retracting.ToString("##.000");
                    break;
                case TurningRemovaltype.byFollowingContour:
                   
                    break;
            }
            #endregion

            blockNumber = blockNumber + 10;
            _data = _data + Environment.NewLine + "N" + blockNumber + sCh.blank;

            int initialProfile = blockNumber + 10;
            int finalProfile = initialProfile + _turning.profile.geometry.Count*10;

            #region Second block
            switch (_turning.turningRemovalType)
            {
                case TurningRemovaltype.byLongitudinal:
                    _data = "G71 P" + initialProfile.ToString("######") + sCh.blank +
                        "Q" + finalProfile.ToString("######") + sCh.blank;
                        if(_turning.turningType == TurningType.externalTurning)
                    {
                        _data = _data + "U" + _turning.allowanceX.ToString("####.000") + sCh.blank;
                    }
                    else
                    {
                        _data = _data + "U-" + _turning.allowanceX.ToString("####.000") + sCh.blank;
                    }
                    _data = _data + "W" + _turning.allowanceZ.ToString("#####.0000") + sCh.blank +
                        "F" + _turning.feedRate.ToString("#.0000") + sCh.blank;

                    break;
                case TurningRemovaltype.byTransverse:
                    _data = "G72 P" + initialProfile.ToString("######") + sCh.blank +
                       "Q" + finalProfile.ToString("######") + sCh.blank;
                    if (_turning.turningType == TurningType.externalTurning)
                    {
                        _data = _data + "U" + _turning.allowanceX.ToString("####.000") + sCh.blank;
                    }
                    else
                    {
                        _data = _data + "U-" + _turning.allowanceX.ToString("####.000") + sCh.blank;
                    }
                    _data = _data + "W" + _turning.allowanceZ.ToString("#####.0000") + sCh.blank +
                        "F" + _turning.feedRate.ToString("#.0000") + sCh.blank;

                    break;
                case TurningRemovaltype.byFollowingContour:
                    
                    break;
            }

            #endregion

           
            return _data;
        }

        private string TurningArcMove(Geometry _geometry)
        {
            string _data = "";
            switch (_geometry.arc.arcDirection)
            {
                case ArcDirection.CCW:
                    _data = _data + "G02 X" + _geometry.finalPosition.coord1.ToString("####.000") + sCh.blank +
                        "Z"+ _geometry.finalPosition.coord2.ToString("####.000")+ sCh.blank +
                        "I" + _geometry.arc.centerPoint.coord1.ToString("####.000") + sCh.blank +
                        "K" + _geometry.arc.centerPoint.coord2.ToString("####.000")
                        ;
                    break;
                case ArcDirection.CW:
                    _data = _data + "G03 X" + _geometry.finalPosition.coord1.ToString("####.000") + sCh.blank +
                      "Z" + _geometry.finalPosition.coord2.ToString("####.000") + sCh.blank +
                      "I" + _geometry.arc.centerPoint.coord1.ToString("####.000") + sCh.blank +
                      "K" + _geometry.arc.centerPoint.coord2.ToString("####.000");
                    break;
            }
            return _data;
        }

        private string TurningLineMove(Geometry _geometry)
        {
            string _data = "";
            _data = _data + "G01 X" + _geometry.finalPosition.coord1.ToString("####.000") + sCh.blank +
                        "Z"+ _geometry.finalPosition.coord2.ToString("####.000");
            return _data;
        }

        private string TurningRapidMove(CoordinatePoint _point, bool safatyMode)
        {
            string _data;
           
            if (safatyMode)
            {
                _data = "G00 X" + GetPosition(_point.coord1);
                blockNumber = blockNumber + 10;
                _data = _data + Environment.NewLine + "N" + blockNumber + sCh.blank;
                _data = _data +"Z" + GetPosition(_point.coord2);
            }
            else
            {
                _data ="G00 X" + GetPosition(_point.coord1) +
                     sCh.blank + "Z" + GetPosition(_point.coord2);
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
            string mCode = _turning.tool.spindleControl.ToString();
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
          
            switch (_wPlane)
            {
                case WorkingPlane.ZX:
                    return Gcode.G18.ToString();
                case WorkingPlane.XY:
                    return Gcode.G17.ToString();
                default:
                    return "";
                   
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

        private string NewBlock(bool GetString)
        {
            string _data="";
            blockNumber = blockNumber + 10;
            _data = _data + Environment.NewLine + "N" + blockNumber + sCh.blank;
            return _data;
        }

        private void NewLine()
        {
            gCodeFile = gCodeFile + Environment.NewLine ;
        }

        private string NewLine(bool GetString)
        {
            string _data = "";
            _data = _data + Environment.NewLine;
            return _data;
        }

        internal void GetGCode(PathDefinition _gCodePathFile)
        {
            File.WriteAllText(_gCodePathFile.GetFullName(),gCodeFile);
        }
    }

   
}
