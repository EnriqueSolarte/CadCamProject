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
        public string verision { get; set; }
        public string dataContent { get; set; }

        //Specific parameters
        public PathDefinition file { get; set; }
        public string duplicatedFilePrefix { get; }
        public List<pointPosition> workOffsets { get; set; }


        public WorkSettings()
        {
            workOffsets = new List<pointPosition>();
            file = new PathDefinition();
            file.directory = "C:\\";
            file.fileName = "defaultName";
            file.extension = ".opt";
            duplicatedFilePrefix = "_duplicated";
            dataContent = "vamos por buen camino kikin";
        }

        public WorkSettings GetParameters(Main MainPage, int index)
        {
            
            WorkSettings op = new WorkSettings();

            if (index <= MainPage.listViewOperations.Items.Count)
            {
                var listOperation = MainPage.listViewOperations.Items.GetItemAt(0) as List<WorkSettings>;
                op = listOperation.Last();
            }
      
            else
            {
                op.Index = MainPage.listViewOperations.Items.Count;
            }
            return op;
        }

        public List<WorkSettings> SetParameters(WorkSettings parameters, Main MainPage)
        {
            List<WorkSettings> listOperation = new List<WorkSettings>();
            listOperation.Add(parameters);
            if (parameters.Index != MainPage.listViewOperations.Items.Count)
            {
                MainPage.listViewOperations.Items.RemoveAt(parameters.Index);
            }
            return listOperation;
          
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
        public int referenceDirectory { get; set; }
        public double parm_1 { get; set; }
        public double parm_2 { get; set; }
        public double parm_3 { get; set; }
        public bool coolant { get; set; }
        public string[] spindleControlArray { get; set; }
        public string spindleControl { get; set; }

        public tool(int index)
        {
            WindowsFunctions functions = new WindowsFunctions();
            localization = index+1;
            cuttingToolTypeArray = functions.CuttingToolTypeArray();
            cuttingToolType = CuttingToolType.Turning.ToString();
            toolName = " New tool " + (index+1).ToString();
            toolSet = 1;
            lengthX = 0.000;
            lengthZ = 0.000;
            radius = 0.010;
            referenceDirectionArray = functions.ReferenceDirectionArray();
            referenceDirectory = 3;
            parm_1 = 95.000;
            parm_2 = 80;
            parm_3 = 12.000;
            spindleControlArray = functions.SpindleControl();
            spindleControl = "M3";
            coolant = true;

        }


    }

}
