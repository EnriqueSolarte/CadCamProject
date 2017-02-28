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
        Turning turningOperation;
        StatusBar statusBarInformation;

        List<Profile> profileList;
        WorkSettings wSettings;
        Drawing drawing;

        public TurningPage(Main main, int index)
        {
            Main = main;
            MainPage = main;
            InitializeComponent();
            turningOperation = new Turning();
           
            turningOperation = turningOperation.GetParameters(MainPage, index);

            wSettings = new WorkSettings();
            wSettings = wSettings.GetParameters(MainPage);
            statusBarInformation = new StatusBar(wSettings.statusBar);
            drawing = new Drawing(wSettings.stock, 680, new Point(450, 330));
            fillingParameters();
            SetUpDrawing();
        }
        #region Drawing
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

            foreach (LineSegment line in drawing.drawingStock.list_A_ChuckDrawing)
            {
                chuckA.Segments.Add(line);
            }

            foreach (LineSegment line in drawing.drawingStock.list_B_ChuckDrawing)
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

        private void DrawProfileGeometry()
        {
            Profile profileOperation = turningOperation.profile;
            if (profileOperation.geometry.Count > 0)
            {
                Point startPoint = drawing.ProfileStartPoint;
                Point initialPoint = profileOperation.geometry[0].initialPosition.GetPoint(profileOperation.workingPlane);

                ProfilePath.StartPoint = new Point(startPoint.X + drawing.scale * initialPoint.X,
                                                   startPoint.Y - drawing.scale * initialPoint.Y);

                drawing.SetListProfileDrawing(profileOperation.geometry, profileOperation.workingPlane);

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

        private void buttonRefreshDrawing_Click(object sender, RoutedEventArgs e)
        {
            WindowsFunctions wfnc = new WindowsFunctions();
            DrawProfileGeometry();
            wfnc.animateProgressBar(progressBar, 1.5);
        }

        #endregion

        #region General Methods
        private void fillingParameters()
        {
            GettingInformation getInformation = new GettingInformation();
            WindowsFunctions fnc = new WindowsFunctions();

            profileList = getInformation.GetListProfiles(MainPage);
            comboBoxFeedRateType.ItemsSource = fnc.FeedRateTypeArray(wSettings.units);
            comboBoxFeedRateType.SelectedItem = turningOperation.feedRateType;
            comboBoxSpeedControlType.ItemsSource = fnc.SpeedControlArray();
            comboBoxSpeedControlType.SelectedItem = turningOperation.speedControlType;
            comboBoxProfiles.ItemsSource = getInformation.GetStringList(profileList);
            comboBoxProfiles.SelectedItem = turningOperation.profile.GetDataString();
            comboBoxTools.ItemsSource = wSettings.GetToolSettings();
            comboBoxTools.SelectedItem = turningOperation.tool.GetDataString();

            comboBoxMachiningRemovalType.ItemsSource = fnc.TurningRemovalTypeArray();
            comboBoxMachiningRemovalType.SelectedItem = turningOperation.turningRemovalType;

            comboBoxTuringType.ItemsSource = fnc.TurningTypeArray();
            comboBoxTuringType.SelectedItem  = turningOperation.turningType;

            
        }

        private void comboBoxSpeedControlType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxSpeedControlType.SelectedItem != null)
            {
                if ((SpeedControl)comboBoxSpeedControlType.SelectedItem != SpeedControl.ConstantSurfaceControl)
                {
                    wCuttingSpeed.IsEnabled = false;
                    textBlockSpindleSpeedControl.Text = "Spindle Speed";
                }
                else
                {
                    wCuttingSpeed.IsEnabled = true;
                    textBlockSpindleSpeedControl.Text = "Speed Limite";
                }
            }
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
            MainPage.listViewOperations.Items.Insert(turningOperation.Index,
                turningOperation.SetParameters(turningOperation, MainPage));
        }

        private void refreshData()
        {

            turningOperation.profile = profileList[comboBoxProfiles.SelectedIndex];
            turningOperation.tool = wSettings.toolSettings[comboBoxTools.SelectedIndex];
            double _feedRate, _cuttingSpeed, _allowanceX, _allowanceZ, _minimumX, _minimumZ,
                   _deltaMinimumX, _deltaMinimumZ, _spindleSpeed, _cuttingDepth,
                   _retracting; 
            double.TryParse(textBoxFeedRate.Text, out _feedRate);
            double.TryParse(textBoxCuttingSpeed.Text, out _cuttingSpeed);
            double.TryParse(textBoxAllowanceX.Text, out _allowanceX);
            double.TryParse(textBoxAllowanceZ.Text, out _allowanceZ);
            double.TryParse(textBlockMinimumX.Text, out _minimumX);
            double.TryParse(textBlockMinimumZ.Text, out _minimumZ);

            double.TryParse(textBoxApproachingPointX.Text, out _deltaMinimumX);
            double.TryParse(textBoxApproachingPointZ.Text, out _deltaMinimumZ);
            double.TryParse(textBoxSpindleSpeed.Text, out _spindleSpeed);
            double.TryParse(textBoxCuttingDepth.Text, out _cuttingDepth);
            double.TryParse(textBoxRetracting.Text, out _retracting);

            turningOperation.cuttingDepth = _cuttingDepth;
            turningOperation.retracting = _retracting;
            turningOperation.allowanceX = _allowanceX;
            turningOperation.allowanceX = _allowanceZ;
            turningOperation.feedRate = _feedRate;
            turningOperation.feedRateType = comboBoxFeedRateType.Text;
            turningOperation.cuttingSpeed = _cuttingSpeed;
            turningOperation.speedControlType = (SpeedControl)comboBoxSpeedControlType.SelectedItem;
            turningOperation.spindleSpeed = _spindleSpeed;
            turningOperation.apprachingPoint.coord1 = _minimumX + _deltaMinimumX;
            turningOperation.apprachingPoint.coord2 = _minimumZ + _deltaMinimumZ;

            turningOperation.turningType = (TurningType)comboBoxTuringType.SelectedItem;
            turningOperation.turningRemovalType = (TurningRemovaltype)comboBoxMachiningRemovalType.SelectedItem;
            turningOperation.Parameters = turningOperation.ShowingParameters();

        }
        private void ControlStatusBar()
        {

            #region statusBar ready
            if (statusBarInformation.statusBoolean)
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
            if (statusBarInformation.status == StateFile.Ready)
            {
               

            }
            else
            {
         
            }
            #endregion

            #region ProgressBar
            if (progressBar.Value == progressBar.Maximum)
            {
                progressBar.Visibility = Visibility.Hidden;
                statusBarInformation.status = StateFile.Ready;
            }
            #endregion

            LabelVersion.Content = statusBarInformation.version;
            LabelFile.Content = statusBarInformation.pathFile.fileName;
            LabelStatus.Content = statusBarInformation.status;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            ControlStatusBar();
        
        }

        #endregion

        private void comboBoxProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxProfiles.SelectedIndex != -1)
            {
                turningOperation.profile = profileList[comboBoxProfiles.SelectedIndex];
                DrawProfileGeometry();
                minimumTextBlockSetting();
            }

        }

        private void minimumTextBlockSetting()
        {
            int lastGeometry = turningOperation.profile.geometry.Count-1;
            textBlockMinimumX.Text = turningOperation.profile.geometry[lastGeometry].finalPosition.coord1.ToString();
            textBlockMinimumZ.Text = turningOperation.profile.geometry[0].initialPosition.coord2.ToString();
        }

        private void buttonGenerateGCode_Click(object sender, RoutedEventArgs e)
        {
            refreshData();
            WindowsFunctions wfnc = new WindowsFunctions();
            PostProcessor code = new PostProcessor(wSettings,turningOperation);
            PathDefinition gCodeFile = new PathDefinition();
            gCodeFile.directory = wSettings.statusBar.pathFile.directory;
            gCodeFile.fileName = "gCode" + wSettings.extensionGCode;
            code.GetGCode(gCodeFile);
            statusBarInformation.status = StateFile.Creating_GCODE;
            wfnc.animateProgressBar(progressBar, 3);
            ControlStatusBar();
        }

        private void comboBoxMachiningRemovalType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((TurningRemovaltype)comboBoxMachiningRemovalType.SelectedItem == TurningRemovaltype.byFollowingContour)
            {
                comboBoxTuringType.SelectedItem = TurningType.externalTurning;
                comboBoxTuringType.IsEnabled = false;
            }else
            {
                comboBoxTuringType.IsEnabled = true;
            }
        }
    }
}
