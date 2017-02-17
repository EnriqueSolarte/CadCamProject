using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CadCamProject
{
    [Serializable]
    public class WorkSettings
    {
        //General parameters
        public string TypeImagineOperation { get; set; }
        public TypeOperations typeOperation { get; set; }
        public string Parameters { get; set; }
        public int Index { get; set; }     
        public string dataContent { get; set; }
        public string version { get; set; }
        public StateToFile status { get; set; }

        //Specific parameters
        public PathDefinition file { get; set; }
        public string duplicatedFilePrefix { get; }
        public List<WorkOffsetPointPosition> workOffsets { get; set; }
        public List<Tool> toolSettings { get; set; }
        public int counterTools { get; set; }
        public BlackStock stock { get; set; }
        

        public WorkSettings()
        {
            TypeImagineOperation = "/Images/WorkSettigs.png";
            typeOperation = TypeOperations.Work_Settings;
            workOffsets = new List<WorkOffsetPointPosition>();
            workOffsets.Add(new WorkOffsetPointPosition(0));
            toolSettings = new List<Tool>();
            toolSettings.Add(new Tool(1));
            counterTools = 1;
            stock = new BlackStock();
            file = new PathDefinition();
            file.directory = "C:\\";
            file.fileName = "defaultName."+ extensionFiles.wstt;
            duplicatedFilePrefix = "_duplicated";
            dataContent = "vamos por buen camino kikin";
        }

        internal List<string> GetWorkOffsets()
        {
            // gets the defined work offsets by a string list to be used in a combobox
            SpecialChart sCh = new SpecialChart();
            List<string> wOffsets = new List<string>();
            
            for(int i=0; i < workOffsets.Count; i++)
            {
                wOffsets.Add(workOffsets[i].Gcode + sCh.blank +workOffsets[i].description);
            }

            return wOffsets;
        }
        internal List<string> GetToolSettings()
        {
            SpecialChart sCh = new SpecialChart();
            List<string> _toolSettings = new List<string>();
            foreach(Tool _tool in toolSettings)
            {
                string data = _tool.toolName + sCh.blank + sCh.chLF +
                               _tool.localization + sCh.blank + _tool.toolSet + sCh.chRH;
                _toolSettings.Add(data);
            }
            return _toolSettings;
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
                             wSettings.stock.internalDiameter.ToString() + sCh.blank +
                             wSettings.stock.finalPosition.ToString() + sCh.blank +
                             wSettings.stock.splindleLimit.ToString() + sCh.chRH +
                             sCh.chLF + "V" + wSettings.version + sCh.chRH;
            return dataOut;
        }
    }

    [Serializable]
    public  class WorkOffsetPointPosition
    {
        public string description { get; set; }
      
        public Gcode[] GcodeArray { get; set; }
        public Gcode Gcode { get; set; }
        public double Xpos { get; set; }
        public double Ypos { get; set; }
        public double Zpos { get; set; }
        public double Spindle_1 { get; set; }
        public double Spindle_2 { get; set; }

        public WorkOffsetPointPosition(int index)
        {
            WindowsFunctions functions = new WindowsFunctions();
            description = "New Work Offset " + index.ToString();
    
            GcodeArray = functions.GcodeArrayWorkOfset();
            Gcode =Gcode.G54;
            Xpos = 0.000;
            Ypos = 0.000;
            Zpos = 0.000;
            Spindle_1 = 0.000;
            Spindle_2 = 0.000;
        }
    }
    [Serializable]
    public class Tool
    {
        public int localization { get; set; }
        public CuttingToolType[] cuttingToolTypeArray { get; set; }
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
        public Mcode[] spindleControlArray { get; set; }
        public string spindleControl { get; set; }
        public bool isEdgeTool { get; set; }
        public int definedSetTools { get; set; }

        public Tool(int index)
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
            referenceDirectionArray = functions.ReferenceToolDirectionArray();
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
    [Serializable]
    public class BlackStock
    {
        public double externalDiameter { get; set; }
        public double internalDiameter { get; set; }
        public double initialPosition { get; set; }
        public double finalPosition { get; set; }
        public double splindleLimit { get; set; }

        public BlackStock()
        {
            externalDiameter = 60.000;
            internalDiameter = 0.000;
            initialPosition = 0.000;
            finalPosition = -80.000;
            splindleLimit = -50.000;
    }
        

}

    
}
