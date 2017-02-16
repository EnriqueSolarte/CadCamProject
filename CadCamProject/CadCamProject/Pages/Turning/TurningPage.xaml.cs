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

        WorkSettings wSettings;
        Drawing drawing;


        public TurningPage(Main main, int index)
        {
            Main = main;
            MainPage = main;
            InitializeComponent();
            turningOperation = new Turning();
            statusBarInformation = new StatusBar();

            turningOperation = turningOperation.GetParameters(MainPage, index);

            wSettings = new WorkSettings();
            wSettings = wSettings.GetParameters(MainPage);

            fillingParameters();

            drawing = new Drawing(wSettings.stock, 650, new Point(485, 330));
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
            //if (profileOperation.geometry.Count > 0)
            //{
            //    Point startPoint = drawing.ProfileStartPoint;
            //    Point initialPoint = profileOperation.geometry[0].initialPosition.GetPoint((WorkingPlane)comboBoxWorkingPlane.SelectedItem);

            //    ProfilePath.StartPoint = new Point(startPoint.X + drawing.scale * initialPoint.X,
            //                                       startPoint.Y - drawing.scale * initialPoint.Y);

            //    drawing.SetListProfileDrawing(profileOperation.geometry, (WorkingPlane)comboBoxWorkingPlane.SelectedItem);

            //    ProfilePath.Segments.Clear();

            //    #region Drawing Profile
            //    foreach (Drawing draw in drawing.listProfileDrawing)
            //    {
            //        if (draw.type.Name == TypeGeometry.Line.ToString())
            //        {
            //            ProfilePath.Segments.Add(draw.line);
            //        }
            //        else
            //        {
            //            ProfilePath.Segments.Add(draw.arc);
            //        }
            //    }
            //    #endregion
            //}
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

            MainPage.listViewOperations.Items.Insert(turningOperation.Index,
                turningOperation.SetParameters(turningOperation, MainPage));
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

            }
            else
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

        }

        #endregion

        private List<Profile> GetListProfile()
        {
            List<Profile> _listProfile = new List<Profile>();


            return _listProfile;
        }



    }
}
