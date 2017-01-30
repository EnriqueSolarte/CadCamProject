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

    public partial class ProfilePage : UserControl
    {
        UserControl Main;
        Main MainPage;
        Profile profileOperation;
        StatusBar statusBarInformation;
        SpecialChart specialChart;
        WorkSettings wSettings;

        public ProfilePage(Main main, int index)
        { 
            Main = main;
            MainPage = main;
            InitializeComponent();
            profileOperation = new Profile();
            statusBarInformation = new StatusBar();
            specialChart = new SpecialChart();
            profileOperation = profileOperation.GetParameters(MainPage,index);
            wSettings = new WorkSettings();
            wSettings = wSettings.GetParameters(MainPage);
           
            fillingParameters();
        }

        private void fillingParameters()
        {
            WindowsFunctions function = new WindowsFunctions();
            // Status Bar 
            statusBarInformation.fileName = wSettings.file.fileName;
            statusBarInformation.version = wSettings.version;
            statusBarInformation.status = wSettings.status;

            //Work Offsets - PLane W
            comboBoxWorkOffsets.ItemsSource = wSettings.GettingWorkOffsets(wSettings.workOffsets);
            comboBoxWorkOffsets.SelectedIndex = profileOperation.workOffsetIndex;
            comboBoxWorkingPlane.ItemsSource = function.wPlaneArray();
            comboBoxWorkingPlane.SelectedItem = profileOperation.workingPlane;

            //Radius Definition - Transition Between Geomeries
            comboBoxRadiusDefinition.ItemsSource = function.RadiusDefinitionArray();
            // 
            comboBoxTransitionNext.ItemsSource = function.TransitionGeometriesArray();
            //


        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            Definition();
            Switcher.Switch(Main);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(Main);
        }

        private void Definition()
        {

            

            MainPage.listViewOperations.Items.Insert(profileOperation.Index,
                profileOperation.SetParameters(profileOperation, MainPage));
        }

        private void ControlStatusBar()
        {

            #region statusBar ready
            if (statusBarInformation.ready)
            {               
                //state ready
            }
            else
            {
               //state no ready
            }
            #endregion

            #region ProgressBar
            if (progressBar.Value == progressBar.Maximum)
            {
                progressBar.Visibility = Visibility.Hidden;
                statusBarInformation.status = StateToFile.Ready;
            }
            #endregion

            LabelVersion.Content = statusBarInformation.version;
            LabelFile.Content = statusBarInformation.fileName;
            LabelStatus.Content = statusBarInformation.status;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            ControlStatusBar();
        }

        private void radioButtonAddArc_Checked(object sender, RoutedEventArgs e)
        {
            CheckingDefinitionGeometry();
        }

        private void CheckingDefinitionGeometry()
        {
            if (radioButtonAddArc.IsChecked.Value)
            {

            }

            if (radioButtonAddLine.IsChecked.Value)
            {

            }

            if (checkBoxTransitionNext.IsChecked.Value)
            {

            }
        }
    }

}
