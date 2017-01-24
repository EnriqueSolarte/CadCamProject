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
        public TypePosition typePosition { get; set; }
        public string Gcode { get; set; }
        public double Xpos { get; set; }
        public double Ypos { get; set; }
        public double Zpos { get; set; }
        public double Spindle_1 { get; set; }
        public double Spindle_2 { get; set; }

        public pointPosition(string index)
        {
            description = "New Work Offset " + index;
            typePosition = TypePosition.coordinateOffset;
            Gcode = "G54";
            Xpos = 543.231;
            Ypos = 0.00;
            Zpos = 285.235;
            Spindle_1 = 0;
            Spindle_2 = 0;
        }
    }

   
}
