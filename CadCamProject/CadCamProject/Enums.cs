using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadCamProject
{
    public enum TypePosition
    {
        coordinateOffset,
        coordinatePosition,
    }

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
}
