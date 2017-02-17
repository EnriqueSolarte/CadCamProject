using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace CadCamProject
{
    public  class Turning
    {
        //General Parameters
        public string TypeImagineOperation { get; set; }
        public TypeOperations typeOperation { get; set; }
        public string Parameters { get; set; }
        public int Index { get; set; }
        public string version { get; set; }

        //Common Parameters
        public Profile profile { get; set; }
        public Tool tool { get; set; }
        public double feedRate { get; set; }
        public double cuttingSpeed { get; set; }

        public TurningType turningType { get; set; }
        public double allowanceX { get; set; }
        public double allowanceZ { get; set; }

        public TurningRemovaltype turningRemovalType { get; set; }


        public Turning()
        {
            TypeImagineOperation = "/Images/Turning.png";
            typeOperation = TypeOperations.Turning;
            profile = new Profile();
        }

        public Turning GetParameters(Main MainPage, int index)
        {
            Turning op = new Turning();

            if (index <= MainPage.listViewOperations.Items.Count)
            {
                var listOperation = MainPage.listViewOperations.Items.GetItemAt(index) as List<Turning>;
                op = listOperation.Last();
            }
            else
            {
                op.Index = MainPage.listViewOperations.Items.Count;
            }
            return op;
        }

        public List<Turning> SetParameters(Turning opParameters, Main MainPage)
        {
            List<Turning> listOperation = new List<Turning>();
            listOperation.Add(opParameters);
            if (opParameters.Index != MainPage.listViewOperations.Items.Count)
            {
                MainPage.listViewOperations.Items.RemoveAt(opParameters.Index);
            }
            return listOperation;
        }

        internal string ShowingParameters()
        {
            SpecialChart sCh = new SpecialChart();
            string dataOut = sCh.chLF + turningType.ToString() + sCh.chRH +
                             sCh.chLF + "PRF " + profile.OperationName + sCh.blank + profile.workOffset + sCh.chRH +
                             sCh.chLF + turningRemovalType.ToString() + sCh.chRH;
                          
            return dataOut;
        }

    }
}
