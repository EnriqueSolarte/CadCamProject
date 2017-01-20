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

        public wSettingPage(Main main, int index)
        { 
            Main = main;
            MainPage = main;
            InitializeComponent();
            workSettings = new WorkSettings();
            workSettings = workSettings.GetParameters(MainPage, index);
            fillingParameters();
        } 

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            Definition();
            MainPage.Buttons(true);
            Switcher.Switch(Main);
        }

        private void Definition()
        {
           
            workSettings.TypeImagineOperation = "/Images/WorkSettigs.png";
            workSettings.TypeOperation = "Work Settings";
            workSettings.Parameters = DateTime.Now.ToString("[DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss]");
            workSettings.upDate = DateTime.Now.ToString();
         
            MainPage.listViewOperations.Items.Insert(workSettings.Index,
                workSettings.SetParameters(workSettings, MainPage));     
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(Main);
        }

        private void fillingParameters()
        {
            textBlockVersion.Text = workSettings.upDate;

        }


        

        private void radioButtonFiles_Clicked(object sender, RoutedEventArgs e)
        {
            bool stateCheckBox = false;
            if (radioButtonExistingFile.IsChecked == true)
            {
                stateCheckBox = true;
                buttonLoadSave.Content = "Load";
            }
            else
            {

                stateCheckBox = false;
                buttonLoadSave.Content = "Save File";
            }

            checkBoxOperations.IsEnabled = stateCheckBox;
            checkBoxProfiles.IsEnabled = stateCheckBox;
            checkBoxBlanksDefinitions.IsEnabled = stateCheckBox;
            checkBoxSettingTools.IsEnabled = stateCheckBox;
            checkBoxWorkOffset.IsEnabled = stateCheckBox;
        }
    }
}
