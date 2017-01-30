using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CadCamProject.Pages;


namespace CadCamProject
{
    public class Profile
    {
        //General Parameters
        public string TypeImagineOperation { get; set; }
        public Operations TypeOperation { get; set; }
        public string Parameters { get; set; }
        public int Index { get; set; }
        public string dataContent { get; set; }
       

        //Specific parameters
        public string OperationName { get; set; }
        public int workOffsetIndex { get; set; }
        public wPlane workingPlane { get; set; }

        public Profile()
        {
            TypeImagineOperation = "/Images/Profile.png";
            TypeOperation = Operations.Profile;
            workingPlane = wPlane.XZ;
            workOffsetIndex = 0;


            dataContent = "vamos por buen camino kikin";
        }

        public Profile GetParameters(Main MainPage, int index)
        {
            Profile op = new Profile();

            if (index <= MainPage.listViewOperations.Items.Count)
            {
                var listOperation = MainPage.listViewOperations.Items.GetItemAt(index) as List<Profile>;
                op = listOperation.Last();
            }
            else
            {
                op.Index = MainPage.listViewOperations.Items.Count;
            }


            return op;
        }

        public List<Profile> SetParameters(Profile opParameters, Main MainPage)
        {
            List<Profile> listOperation = new List<Profile>();
            listOperation.Add(opParameters);
            if (opParameters.Index != MainPage.listViewOperations.Items.Count)
            {
                MainPage.listViewOperations.Items.RemoveAt(opParameters.Index);
            }
            return listOperation;
        }
    }

    class GeometryProfile
    {

    }

}
