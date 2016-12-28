using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Interface.Pages
{

    public partial class Profile : UserControl
    {
        UserControl Main;
        Main MainPage;
        Operation profileOperation;

        public Profile(Main main, int index)
        { 
            Main = main;
            MainPage = main;
            InitializeComponent();
            profileOperation = new Operation();
            profileOperation = profileOperation.GetParameters(MainPage,index);
            fillingProfileParameters();
        } 

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            profileDefinition();
            Switcher.Switch(Main);
        }

        private void profileDefinition()
        {
            profileOperation.TypeImagineOperation ="/Images/Profile.png";
            profileOperation.TypeOperation = "Profile";
            profileOperation.Parameters = DateTime.Now.ToString("[DD=hh][MM=mm][YY=-hh][MM=mmss]");
            profileOperation.upDate = DateTime.Now.ToString();
         
            MainPage.listViewOperations.Items.Insert(profileOperation.Index,
                profileOperation.SetParameters(profileOperation, MainPage));     
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(Main);
        }

        private void fillingProfileParameters()
        {
            this.textBoxParameters.Text = profileOperation.Index + profileOperation.Parameters;

        }
    }
}
