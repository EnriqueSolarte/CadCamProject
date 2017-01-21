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
        public string upDate { get; set; }
        public string savingFormat { get; set; }

        //Specific parameters
        public string pathDirectory { get; set; }
        public string fileName { get; set; }
        public string extension { get; }
        public string duplicatedFilePrefix { get; }
        

        public WorkSettings()
        {
            pathDirectory = "C:\\";
            fileName = "defaultName";
            extension = ".opt";
            duplicatedFilePrefix = "_duplicated";
            savingFormat = "vamos por buen camino kikin";
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
}
