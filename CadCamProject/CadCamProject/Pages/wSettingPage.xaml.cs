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

namespace CadCamProject.Pages
{

    public partial class wSettingPage : UserControl
    {
        UserControl Main;
        Main MainPage;
        WorkSettings workSettings;

        public wSettingPage(Main main)
        { 
            Main = main;
            MainPage = main;
            InitializeComponent();
            workSettings = new WorkSettings();
            workSettings = workSettings.GetParameters(MainPage);
            fillingProfileParameters();
        } 

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            profileDefinition();
            Switcher.Switch(Main);
        }

        private void profileDefinition()
        {
           
            workSettings.Parameters = DateTime.Now.ToString("[DD=hh][MM=mm][YY=-hh][MM=mmss]");
            workSettings.upDate = DateTime.Now.ToString();
         
            MainPage.listViewOperations.Items.Insert(workSettings.Index,
                workSettings.SetParameters(workSettings, MainPage));     
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(Main);
        }

        private void fillingProfileParameters()
        {
            

        }

      
    }
}
