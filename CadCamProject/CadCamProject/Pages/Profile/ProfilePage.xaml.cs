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
            comboBoxRadiusDefinition.SelectedItem = RadiusDefinition.byRadius; 
            comboBoxTransitionNext.ItemsSource = function.TransitionGeometriesArray();
            comboBoxTransitionNext.SelectedItem = TransitionGeometries.Round;


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
                buttonAccept.IsEnabled = true;
            }
            else
            {
                //state no ready
                buttonAccept.IsEnabled = false;
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
            CheckingDefinitionGeometry();
            wTransitionParameters.IsEnabled = (bool)checkBoxTransitionNext.IsChecked;
            CheckingProfileFileName();
        }

        private void comboBoxRadiusDefinition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckingDefinitionGeometry();
        }
        private void CheckingDefinitionGeometry()
        {
            if (radioButtonAddArc.IsChecked == true)
            {
                wRadiusDefinition.Visibility = Visibility.Visible;

                if ((RadiusDefinition)comboBoxRadiusDefinition.SelectedItem == RadiusDefinition.byRadius)
                {
                    wRadius.Visibility = Visibility.Visible;
                    wCenterPosCoord1.Visibility = Visibility.Hidden;
                    wCenterPosCoord2.Visibility = Visibility.Hidden;

                }
                else
                {
                    wRadius.Visibility = Visibility.Hidden;
                    wCenterPosCoord1.Visibility = Visibility.Visible;
                    wCenterPosCoord2.Visibility = Visibility.Visible;
                }
            }
            else
            {
                wRadiusDefinition.Visibility = Visibility.Hidden;
                wRadius.Visibility = Visibility.Hidden;
                wCenterPosCoord1.Visibility = Visibility.Hidden;
                wCenterPosCoord2.Visibility = Visibility.Hidden;
            }    
        }

        private void radioButtonAddArc_Clicked(object sender, RoutedEventArgs e)
        {
            CheckingDefinitionGeometry();
        }
        private void radioButtonAddLine_Clicked(object sender, RoutedEventArgs e)
        {
            CheckingDefinitionGeometry();
        }

        private void comboBoxTransitionNext_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((TransitionGeometries)comboBoxTransitionNext.SelectedItem == TransitionGeometries.Round)
            {
                labelTransitionParameter.Content = TransitionParameter.Rnd;
            }
            else
            {
                labelTransitionParameter.Content = TransitionParameter.Chm;
            }
        }
        private void checkBoxTransitionNext_Clicked(object sender, RoutedEventArgs e)
        {
            wTransitionParameters.IsEnabled = (bool)checkBoxTransitionNext.IsChecked;
        }

        private void comboBoxWorkingPlane_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckingDefinitionGeometry();

            if((wPlane)comboBoxWorkingPlane.SelectedItem == wPlane.XY)
            {
                labelInitialCoord1.Content = labelFinalCoord1.Content = LabelCoordinate.X;
                labelInitialCoord2.Content = labelFinalCoord2.Content = LabelCoordinate.Y;
                labelCentePosCoord1.Content = LabelCoordinate.I;
                labelCenterPosCoord2.Content = LabelCoordinate.J;
            }
            else
            {
                labelInitialCoord1.Content = labelFinalCoord1.Content = LabelCoordinate.X;
                labelInitialCoord2.Content = labelFinalCoord2.Content = LabelCoordinate.Z;
                labelCentePosCoord1.Content = LabelCoordinate.I;
                labelCenterPosCoord2.Content = LabelCoordinate.K;
            }
        }
     
        private void textBoxProfileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckingProfileFileName();
        }

        private void CheckingProfileFileName()
        {
            if (textBoxProfileName.Text == "")
            {
                statusBarInformation.status = StateToFile.Without_Name;
                statusBarInformation.ready = false;
               
            }
            else
            {
                statusBarInformation.status = wSettings.status;
                statusBarInformation.ready = true;
               
            }
            ControlStatusBar();
        }

        
    }

}
