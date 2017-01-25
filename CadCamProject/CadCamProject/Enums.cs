using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadCamProject
{
   
    public enum StateToFile
    {
        Saving,
        Loading,
        Saved,
        Unsaved,
        Loaded,
        Error,
        Fail,
        Ready,

    }

    public enum ButtonContents
    {
        Save,
        Load,
        Cancel,
        Acept,
        Ok,

    }

    public enum Gcode
    {
        G54,
        G55,
        G56,
        G57,
        G58,
        G59,
        CPP,   

    }

    public enum Mcode
    {
        M3,
        M4,
        M5,
    }

    public enum CuttingToolType
    {
        Turning,
        Grooving,
        Threading,
        Milling,
        Drilling, 

    }
                   
    public enum ReferenceDirection
    {
        pos1=1,
        pos2=2,
        pos3=3,
        pos4=4,
        pos5=5,
        pos6=6,
        pos7=7,
        pos8=8,
        pos9=9,
    }
}
