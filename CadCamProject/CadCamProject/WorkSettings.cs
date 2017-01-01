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
        public string TypeImagineOperation = "/Images/workSettings.png";
        public string TypeOperation = "Work Settings";
        public string Parameters { get; set; }

        public int Index = 0;

        public string upDate { get; set; }

        public WorkSettings GetParameters(Main MainPage)
        {
          
            List<WorkSettings> workSettings = new List<WorkSettings>();

            workSettings = MainPage.listViewOperations.Items.GetItemAt(0) as List<WorkSettings>;
            

            return workSettings;
        }

        public List<WorkSettings> SetParameters(WorkSettings opParameters, Main MainPage)
        {
            List<WorkSettings> listOperation = new List<WorkSettings>();
            listOperation.Add(opParameters);
            if (opParameters.Index != MainPage.listViewOperations.Items.Count)
            {
                MainPage.listViewOperations.Items.RemoveAt(opParameters.Index);
            }
            return listOperation;
        }
    }
}
