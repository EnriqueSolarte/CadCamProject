using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace CadCamProject
{

    public partial class TurningPage : UserControl
    {
        UserControl Main;
        Main MainPage;
        Profile profileOperation;
        StatusBar statusBarInformation;
        SpecialChart specialChart;
        WorkSettings wSettings;
        Drawing drawing;
       

        public TurningPage(Main main, int index)
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

            drawing = new Drawing(wSettings.stock, 650, new Point(485, 330));
            SetUpDrawing();
        }

        private void SetUpDrawing()
        {
            labelScale.Content = drawing.scaleLabel;
            SettingOriginDraw(drawing.ProfileStartPoint, drawing.scaleOrigin); // method of this class ProfilePage
            ProfilePath.StartPoint = drawing.ProfileStartPoint;

            DrawStockGeometry();
            DrawChuckGeometry();
            
        }

        private void DrawChuckGeometry()
        {
            chuckA.StartPoint = drawing.drawingStock.A_ChuckStartPoint;
            chuckB.StartPoint = drawing.drawingStock.B_ChuckStartPoint;
            chuckA.Segments.Clear();
            chuckB.Segments.Clear();

            foreach(LineSegment line in drawing.drawingStock.list_A_ChuckDrawing)
            {
                chuckA.Segments.Add(line);
            }

            foreach(LineSegment line in drawing.drawingStock.list_B_ChuckDrawing)
            {
                chuckB.Segments.Add(line);
            }
        }

        private void DrawStockGeometry()
        {
            Point startPoint = drawing.drawingStock.StockStartPoint;
            externalStock.StartPoint = startPoint;
            internalStock.StartPoint = startPoint;

            externalStock.Segments.Clear();
            internalStock.Segments.Clear();
            foreach (LineSegment line in drawing.drawingStock.listExtStockDrawing)
            {
                externalStock.Segments.Add(line);
            }

            foreach (LineSegment line in drawing.drawingStock.listIntStockDrawing)
            {
                internalStock.Segments.Add(line);
            }
        }

        private void SettingOriginDraw(Point _startPoint, double _size)
        {


            Point pOrigin1 = new Point(_startPoint.X + _size, _startPoint.Y);
            Point pOrigin2 = new Point(_startPoint.X, _startPoint.Y - _size);
            Point pOrigin3 = new Point(_startPoint.X - _size, _startPoint.Y);
            Point pOrigin4 = new Point(_startPoint.X, _startPoint.Y + _size);

            Point point1 = new Point(_startPoint.X, _startPoint.Y - _size);
            Point point2 = new Point(_startPoint.X - _size, _startPoint.Y);
            Point point3 = new Point(_startPoint.X, _startPoint.Y + _size);
            Point point4 = new Point(_startPoint.X + _size, _startPoint.Y);

            Size size = new Size(_size, _size);

            Origin1.StartPoint = pOrigin1;
            Origin2.StartPoint = pOrigin2;
            Origin3.StartPoint = pOrigin3;
            Origin4.StartPoint = pOrigin4;

            arc1.Point = point1;
            arc1.Size = size;
            arc2.Point = point2;
            arc2.Size = size;
            arc3.Point = point3;
            arc3.Size = size;
            arc4.Point = point4;
            arc4.Size = size;

            line1.Point = _startPoint;
            line2.Point = _startPoint;
            line3.Point = _startPoint;
            line4.Point = _startPoint;
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
            checkBoxTransitionNext.IsChecked = false;

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

            DrawProfileGeometry();
            settingIntialGeometryPoint(); //get the initial point for the next geometry
        }
        //difiniendo AQUI

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

        private void DrawProfileGeometry()
        {
            if (profileOperation.geometry.Count > 0)
            {
                Point startPoint = drawing.ProfileStartPoint;
                Point initialPoint = profileOperation.geometry[0].initialPosition.GetPoint((WorkingPlane)comboBoxWorkingPlane.SelectedItem);

                ProfilePath.StartPoint = new Point(startPoint.X + drawing.scale * initialPoint.X,
                                                   startPoint.Y - drawing.scale * initialPoint.Y);

                drawing.SetListProfileDrawing(profileOperation.geometry, (WorkingPlane)comboBoxWorkingPlane.SelectedItem);

                ProfilePath.Segments.Clear();

                #region Drawing Profile
                foreach (Drawing draw in drawing.listProfileDrawing)
                {
                    if (draw.type.Name == TypeGeometry.Line.ToString())
                    {
                        ProfilePath.Segments.Add(draw.line);
                    }
                    else
                    {
                        ProfilePath.Segments.Add(draw.arc);
                    }
                }
                #endregion
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
                arc = new Arc(initalCoord1,initalCoord2,finalCoord1,finalCoord2,centerCoord1,centerCoord2,(ArcDirection)comboBoxArcDirection.SelectedItem,(bool)checkBoxIsLargeArc.IsChecked);

            }
            //Point center = arc.GetCenterCircule((WorkingPlane)comboBoxWorkingPlane.SelectedItem);
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

        private void buttonRefreshDrawing_Click(object sender, RoutedEventArgs e)
        {
            WindowsFunctions wfnc = new WindowsFunctions();
            DrawProfileGeometry();
            wfnc.animateProgressBar(progressBar, 1.5);
        }

      
    }
}
