using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CadCamProject.Pages;

namespace CadCamProject
{
    public  class Operation
    {
        public string TypeImagineOperation { get; set; }
        public string TypeOperation { get; set; }
        public string Parameters { get; set; }

        public int Index { get; set; }

        public string upDate { get; set; }

        public Operation GetParameters(Main MainPage, int index)
        {
            Operation op = new Operation();

            if (index <= MainPage.listViewOperations.Items.Count)
            {
                var listProfileOperation = MainPage.listViewOperations.SelectedValue as List<Operation>;
                op = listProfileOperation.Last();
            }
            else
            {
                op.Index = MainPage.listViewOperations.Items.Count;
            }
            return op;
        }

        public List<Operation> SetParameters(Operation opParameters, Main MainPage)
        {
            List<Operation> listOperation = new List<Operation>();
            listOperation.Add(opParameters);
            if (opParameters.Index != MainPage.listViewOperations.Items.Count)
            {
                MainPage.listViewOperations.Items.RemoveAt(opParameters.Index);
            }
            return listOperation;
        }

    }
}
