using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace CadCamProject
{
    public  class Operation
    {
        //General Parameters
        public string TypeImagineOperation { get; set; }
        public TypeOperations TypeOperation { get; set; }
        public string Parameters { get; set; }
        public int Index { get; set; }
        public string dataContent { get; set; }

        //Common Parameters
        public string OperationName { get; set; }
        public string version { get; set; }

        public Operation()
        {
            TypeImagineOperation = "/Images/Profile.png";
            TypeOperation = TypeOperations.Profile;


            dataContent = "vamos por buen camino kikin";
        }

        public Operation GetParameters(Main MainPage, int index)
        {
            Operation op = new Operation();

            if (index <= MainPage.listViewOperations.Items.Count)
            {
                var listOperation = MainPage.listViewOperations.Items.GetItemAt(index) as List<Operation>;
                op = listOperation.Last();
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
