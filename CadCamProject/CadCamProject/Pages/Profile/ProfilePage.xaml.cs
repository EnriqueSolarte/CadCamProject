using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Xml.Serialization;

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
            profileOperation = profileOperation.GetParameters(MainPage, index);
            wSettings = new WorkSettings();
            wSettings = wSettings.GetParameters(MainPage);

            fillingParameters();
        }

        #region General Data Manage
        private void fillingParameters()
        {
            WindowsFunctions function = new WindowsFunctions();
            // Status Bar 
            statusBarInformation.fileName = wSettings.file.fileName;
            statusBarInformation.version = wSettings.version;
            if(profileOperation.version == null)
            {
                profileOperation.version = wSettings.version;
            }
            statusBarInformation.status = wSettings.status;
            textBoxProfileName.Text = profileOperation.OperationName;

            //Work Offsets - PLane W
            comboBoxWorkOffsets.ItemsSource = wSettings.GettingWorkOffsets(wSettings.workOffsets);
            comboBoxWorkOffsets.SelectedIndex = profileOperation.workOffsetIndex;
            comboBoxWorkingPlane.ItemsSource = function.wPlaneArray();
            comboBoxWorkingPlane.SelectedItem = profileOperation.workingPlane;

            //Radius Definition - Transition Between Geomeries - ArcDirection
            comboBoxRadiusDefinition.ItemsSource = function.RadiusDefinitionArray();
            comboBoxRadiusDefinition.SelectedItem = RadiusDefinition.byRadius;
            comboBoxTransitionNext.ItemsSource = function.TransitionGeometriesArray();
            comboBoxTransitionNext.SelectedItem = TypeTransitionGeometry.Round;
            comboBoxArcDirection.ItemsSource = function.ArcDirectionArray();
            comboBoxArcDirection.SelectedItem = ArcDirection.CW;

            //Binding ListView Geometry with list Geometry
            listViewGeometries.ItemsSource = profileOperation.geometry;
            listViewGeometries.Items.Refresh();
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
            refreshData();
            MainPage.listViewOperations.Items.Insert(profileOperation.Index,
                profileOperation.SetParameters(profileOperation, MainPage));
        }

        private void refreshData()
        {
           
            profileOperation.OperationName = textBoxProfileName.Text;
            int indexWO = comboBoxWorkOffsets.SelectedIndex;
            profileOperation.workOffset = wSettings.workOffsets[indexWO].Gcode;
            profileOperation.workOffsetIndex = indexWO;
            profileOperation.workingPlane = (WorkingPlane)comboBoxWorkingPlane.SelectedItem;
            profileOperation.Parameters = profileOperation.ShowingParameters(profileOperation);

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

            #region statusBar Status = ready
            if (statusBarInformation.status == StateToFile.Ready)
            {
                buttonExportProfile.IsEnabled = true;

            }else
            {
                buttonExportProfile.IsEnabled = false;
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
            ChekingListViewSelection();
            settingIntialGeometryPoint();
            CheckingProfileFileName();
        }
        #endregion

        #region Profile Data Manage
        private void comboBoxRadiusDefinition_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckingDefinitionGeometry();
        }
        private void CheckingDefinitionGeometry()
        {
            if (radioButtonAddArc.IsChecked == true)
            {
                wRadiusDefinition.Visibility = Visibility.Visible;
                wArcDirection.Visibility = Visibility.Visible;

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
                wArcDirection.Visibility = Visibility.Hidden;
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

       
        private void checkBoxTransitionNext_Clicked(object sender, RoutedEventArgs e)
        {
            wTransitionParameters.IsEnabled = (bool)checkBoxTransitionNext.IsChecked;
        }

        private void comboBoxWorkingPlane_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckingDefinitionGeometry();

            if ((WorkingPlane)comboBoxWorkingPlane.SelectedItem == WorkingPlane.XY)
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
                statusBarInformation.status = StateToFile.MissingData;
                statusBarInformation.ready = false;

            }
            else
            {
                statusBarInformation.status = wSettings.status;
                statusBarInformation.ready = true;

            }
            ControlStatusBar();
        }
        #endregion

        #region Definition of Geometries
        private void buttonDefineGeometry_Click(object sender, RoutedEventArgs e)
        {
            TransitionGeometry transition = GettingTransitionGeometry();
            

            Geometry geometry;
            if (radioButtonAddArc.IsChecked.Value)
            {
                //Geometry type Arc
                Arc arc = GettingArc();
                geometry = new Geometry(arc, transition, profileOperation.geometry.Count);
                profileOperation.geometry.Add(geometry);
            }
            else
            {
                //Geometry type Line
                Line line = GeetingLine();
                geometry = new Geometry(line, transition, profileOperation.geometry.Count);
                profileOperation.geometry.Add(geometry);
            }
            listViewGeometries.Items.Refresh();

            DrawGeometry();
            settingIntialGeometryPoint(); //get the initial point for the next geometry
        }

        private void settingIntialGeometryPoint()
        {
            if (TextboxInitialValues())
            {
                int lastIndex = listViewGeometries.Items.Count - 1;
                textBoxInitialCoord1.Text = profileOperation.geometry[lastIndex].finalPosition.coord1.ToString();
                textBoxInitialCoord2.Text = profileOperation.geometry[lastIndex].finalPosition.coord2.ToString();
            }

        }

        private bool TextboxInitialValues()
        {
            if (listViewGeometries.Items.Count != 0)
            {
                textBoxInitialCoord1.IsEnabled = false;
                textBoxInitialCoord2.IsEnabled = false;
                return true;
            }
            else
            {
                textBoxInitialCoord1.IsEnabled = true;
                textBoxInitialCoord2.IsEnabled = true;
                return false;
            }
        }

        private void DrawGeometry()
        {
            ProfilePath.StartPoint = profileOperation.drawing.Startpoint;
            ProfilePath.Segments.Clear();

            profileOperation.drawing.DrawPath(profileOperation.geometry);
          
            int count = profileOperation.drawing.drawingGeometry.Count;
            for (int i=0; i < count; i++)
            {
                if (profileOperation.drawing.drawingGeometry[i].typeGeometry.Name == TypeGeometry.Line.ToString())
                {
                    ProfilePath.Segments.Add(profileOperation.drawing.drawingGeometry[i].line.lineSegment);
                }else
                {
                    ProfilePath.Segments.Add(profileOperation.drawing.drawingGeometry[i].arc.arcSegment);
                }
            }

        }

        private Line GeetingLine()
        {
            Line line;
            double initalCoord1, initalCoord2, finalCoord1, finalCoord2;
            double.TryParse(textBoxInitialCoord1.Text, out initalCoord1);
            double.TryParse(textBoxInitialCoord2.Text, out initalCoord2);
            double.TryParse(textBoxFinalCoord1.Text, out finalCoord1);
            double.TryParse(textBoxFinalCoord2.Text, out finalCoord2);

            line = new Line(initalCoord1, initalCoord2, finalCoord1, finalCoord2);
            return line;
        }

        private Arc GettingArc()
        {
            Arc arc;
            double initalCoord1, initalCoord2, finalCoord1, finalCoord2, radius, centerCoord1, centerCoord2;
            double.TryParse(textBoxInitialCoord1.Text, out initalCoord1);
            double.TryParse(textBoxInitialCoord2.Text, out initalCoord2);
            double.TryParse(textBoxFinalCoord1.Text, out finalCoord1);
            double.TryParse(textBoxFinalCoord2.Text, out finalCoord2);
            double.TryParse(textBoxCentePosCoord1.Text, out centerCoord1);
            double.TryParse(textBoxCenterPosCoord2.Text, out centerCoord2);
            double.TryParse(textBoxRadius.Text, out radius);

            if ((RadiusDefinition)comboBoxRadiusDefinition.SelectedItem == RadiusDefinition.byRadius)
            {
                arc = new Arc(initalCoord1,initalCoord2,finalCoord1,finalCoord2,radius,(ArcDirection)comboBoxArcDirection.SelectedItem);
            }
            else
            {
                arc = new Arc(initalCoord1,initalCoord2,finalCoord1,finalCoord2,centerCoord1,centerCoord2,(ArcDirection)comboBoxArcDirection.SelectedItem);
            }

            return arc;
        }

        private TransitionGeometry GettingTransitionGeometry()
        {
            TransitionGeometry transition = new TransitionGeometry();
            double transitionParameter;
            transition.typeTransition = (TypeTransitionGeometry)comboBoxTransitionNext.SelectedItem;
            double.TryParse(textBoxValueTransitionParameter.Text, out transitionParameter);
            transition.parameter = transitionParameter;
            transition.enableTransition = checkBoxTransitionNext.IsChecked.Value;
            return transition;
        }
       
        private void listViewGeometries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChekingListViewSelection();
        }

        private void ChekingListViewSelection()
        {
           if(listViewGeometries.SelectedItem != null)
            {
                buttonEditGeometry.IsEnabled = true;
                buttonDelete.IsEnabled = true;
            }
            else
            {
                buttonEditGeometry.IsEnabled = false;
                buttonDelete.IsEnabled = false;
            }
        }



        #endregion

        #region Export and Import file
        private void buttonExport_Click(object sender, RoutedEventArgs e)
        {
            //maybe this part shoud be change whith the main status of the MAIN WINDOWS

            if (statusBarInformation.status == StateToFile.Ready)
            {
                string path = wSettings.file.directory + textBoxProfileName.Text + " v" + wSettings.version + "." + extensionFiles.prf;
                refreshData();
                ExportingProfile(path);
            }
            
        }

        private void ExportingProfile(string path)
        {
            ExportAndImportToFIle fnc = new ExportAndImportToFIle();
            WindowsFunctions wfnc = new WindowsFunctions();
            fnc.WriteToBinaryFile<Profile>(path, profileOperation);
            wfnc.animateProgressBar(progressBar, 1.5);
        }

        private void buttonImportProfile_Click(object sender, RoutedEventArgs e)
        {
            
                ExportAndImportToFIle fnc = new ExportAndImportToFIle();
                WindowsFunctions wfnc = new WindowsFunctions();
                PathDefinition path = new PathDefinition();
                path = wfnc.fileBrowser("Profile file(prf)|*.prf");
            if (path.GetFullName() != "")
            {
                int savedIndex = profileOperation.Index;
                profileOperation = fnc.ReadFromBinaryFile<Profile>(path.GetFullName());
                profileOperation.Index = savedIndex;
                wfnc.animateProgressBar(progressBar, 1.5);
                fillingParameters();
            }
        }
        #endregion

        private void button_Click(object sender, RoutedEventArgs e)
        {
            LineSegment line = new LineSegment();
            ArcSegment arc = new ArcSegment();
            arc.Point = new System.Windows.Point(500, 500);
            arc.Size = new Size(50, 50);
            arc.SweepDirection = SweepDirection.Clockwise;
            line.Point = new System.Windows.Point(250, 250);
            

            ProfilePath.Segments.Add(arc);

            

            Vector vbase = new Vector(10, 0);
            Vector vector2 = new Vector(10, 10);
            Vector vector1 = new Vector(0, 10);

            double a = Vector.AngleBetween(vbase, vector1);
            double a2 = Vector.AngleBetween(vbase, vector2);
            double x = (a + a2)/2;
            double l = 2.5*1/(Math.Tan(x * Math.PI / 180));
            vector2.Normalize();
            Vector test = Vector.Multiply(l, vector2);

        }
        
    }
}
