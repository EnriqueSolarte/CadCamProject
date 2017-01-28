using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CadCamProject.Pages;

namespace CadCamProject
{
    class WorkSettings
    {
        //General parameters
        public string TypeImagineOperation { get; set; }
        public string TypeOperation { get; set; }
        public string Parameters { get; set; }
        public int Index { get; set; }     
        public string dataContent { get; set; }

        //Specific parameters
        public string version { get; set; }
        public PathDefinition file { get; set; }
        public string duplicatedFilePrefix { get; }
        public List<pointPosition> workOffsets { get; set; }
        public List<tool> toolSettings { get; set; }
        public int counterTools { get; set; }
        public blackStock stock { get; set; }
        


        public WorkSettings()
        {
            TypeImagineOperation = "/Images/WorkSettigs.png";
            TypeOperation = "Work Settings";
            workOffsets = new List<pointPosition>();
            workOffsets.Add(new pointPosition(0));
            toolSettings = new List<tool>();
            toolSettings.Add(new tool(1));
            counterTools = 1;
            stock = new blackStock();
            file = new PathDefinition();
            file.directory = "C:\\";
            file.fileName = "defaultName";
            file.extension = ".opt";
            duplicatedFilePrefix = "_duplicated";
            dataContent = "vamos por buen camino kikin";
        }

        internal WorkSettings GetParameters(Main MainPage)
        {
            
            WorkSettings op = new WorkSettings();

            if (MainPage.listViewOperations.Items.Count != 0)
            {
                List<WorkSettings> listOperation = MainPage.listViewOperations.Items.GetItemAt(0) as List<WorkSettings>;
                op = listOperation.Last();
            }
      
            else
            {
                op.Index = MainPage.listViewOperations.Items.Count;
            }
            return op;
        }

        public List<WorkSettings> SetParameters(WorkSettings wSettings, Main MainPage)
        {
            List<WorkSettings> listOperation = new List<WorkSettings>();
            listOperation.Add(wSettings);
            if (wSettings.Index != MainPage.listViewOperations.Items.Count)
            {
                MainPage.listViewOperations.Items.RemoveAt(wSettings.Index);
            }
            return listOperation;
          
        }

        internal string ShowingParameters(WorkSettings wSettings)
        {
            SpecialChart sCh = new SpecialChart();
            string dataOut = sCh.chLF + wSettings.file.fileName + sCh.chRH +
                             sCh.chLF + "WO " + wSettings.workOffsets.Count.ToString() + sCh.chRH +
                             sCh.chLF + "TS " + wSettings.toolSettings.Count.ToString() + sCh.chRH +
                             sCh.chLF + "BK " + wSettings.stock.externalDiameter.ToString() + sCh.blank +
                             wSettings.stock.finalPosition.ToString() + sCh.blank +
                             wSettings.stock.splindleLimit.ToString() + sCh.chRH;
            return dataOut;
        }
    }

    class pointPosition
    {
        public string description { get; set; }
      
        public string[] GcodeArray { get; set; }
        public string GcodeDefault { get; set; }
        public double Xpos { get; set; }
        public double Ypos { get; set; }
        public double Zpos { get; set; }
        public double Spindle_1 { get; set; }
        public double Spindle_2 { get; set; }

        public pointPosition(int index)
        {
            WindowsFunctions functions = new WindowsFunctions();
            description = "New Work Offset " + index.ToString();
    
            GcodeArray = functions.GcodeArrayWorkOfset();
            GcodeDefault = Gcode.G54.ToString();
            Xpos = 543.231;
            Ypos = 0.00;
            Zpos = 285.235;
            Spindle_1 = 0;
            Spindle_2 = 0;
        }
    }
    
    class tool
    {
        public int localization { get; set; }
        public string[] cuttingToolTypeArray { get; set; }
        public string cuttingToolType { get; set; }
        public string toolName { get; set; }
        public int toolSet { get; set; }
        public double lengthX { get; set;}
        public double lengthZ { get; set; }
        public double radius { get; set; }
        public int[] referenceDirectionArray { get; set; }
        public int referenceDirection { get; set; }
        public double parm_1 { get; set; }
        public double parm_2 { get; set; }
        public double parm_3 { get; set; }
        public bool coolant { get; set; }
        public string[] spindleControlArray { get; set; }
        public string spindleControl { get; set; }
        public bool isEdgeTool { get; set; }
        public int definedSetTools { get; set; }

        public tool(int index)
        {
            WindowsFunctions functions = new WindowsFunctions();
            localization = index;
            cuttingToolTypeArray = functions.CuttingToolTypeArray();
            cuttingToolType = CuttingToolType.Turning.ToString();
            toolName = " New tool " + (index).ToString();
            toolSet = 1;
            lengthX = 0.000;
            lengthZ = 0.000;
            radius = 0.010;
            referenceDirectionArray = functions.ReferenceDirectionArray();
            referenceDirection = 3;
            parm_1 = 95.000;
            parm_2 = 80;
            parm_3 = 12.000;
            spindleControlArray = functions.SpindleControl();
            spindleControl = "M3";
            coolant = true;

            isEdgeTool = false;
            definedSetTools = 1;
        }


    }

    class blackStock
    {
        public double externalDiameter { get; set; }
        public double internalDiameter { get; set; }
        public double initialPosition { get; set; }
        public double finalPosition { get; set; }
        public double splindleLimit { get; set; }

        public blackStock()
        {
            externalDiameter = 60.000;
            internalDiameter = 0.000;
            initialPosition = 0.000;
            finalPosition = -80.000;
            splindleLimit = 10.000;
    }
}
}
