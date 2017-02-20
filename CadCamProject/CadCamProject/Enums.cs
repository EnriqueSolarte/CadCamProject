﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadCamProject
{
    public enum extensionFiles
    {
        CAMprog,
        wstt,
        prf,
    }

    public enum StateFile
    {
        Saving,
        Loading,
        Saved,
        Unsaved,
        Loaded,
        Error,
        Fail,
        Ready,
        MissingData,
        Exporting,

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
        CP,   

    }

    public enum Mcode
    {
        M3,
        M4,
        M5,
    }

    public enum ArcDirection
    {
        CW,
        CCW,
    }

    public enum CuttingToolType
    {
        Turning,
        Grooving,
        Threading,
        Milling,
        Drilling, 

    }
                   
    public enum RadiusDefinition
    {
        byRadius,
        byCoordinateCenter,
    }

    public enum TypeTransitionGeometry
    {
        Round,
        Chamfer,
    }
    
    public enum TypeGeometry
    {
        Line,
        Arc,
    }

    public enum ReferenceToolDirection
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

    public class SpecialChart
    {
        public string chLF { get; }
        public string chRH { get; }
        public string blank { get; }

        public SpecialChart()
        {
            blank = " ";
            chLF = "[";
            chRH = "]";
        }
    } 

    public enum TypeOperations
    {
        Work_Settings,
        Profile, 
        Turning,
    }

    public enum WorkingPlane
    {
        ZX,
        XY,
    }
    public enum LabelCoordinate
    {
        X,
        I,
        Y,
        J,
        Z,
        K,
    }

    public enum TurningRemovaltype
    {
        byLongitudinal, 
        byTransverse,
        byFollowingContour,
    }

    public enum TurningType
    {
        externalTurning,
        internalTurning,
    }
}
