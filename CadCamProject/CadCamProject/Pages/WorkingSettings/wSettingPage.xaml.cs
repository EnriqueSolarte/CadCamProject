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
using System.IO;
using System.Threading;
using System.Windows.Media.Animation;
using System.Globalization;

namespace CadCamProject
{

    public partial class wSettingPage : UserControl
    {
        UserControl Main;
        Main MainPage;
        WorkSettings workSettings;
        Drawing drawing;


        public wSettingPage(Main main)
        { 
            Main = main;  
            MainPage = main;
            InitializeComponent();
            workSettings = new WorkSettings();
            
            workSettings = workSettings.GetParameters(MainPage);
            drawing = new Drawing(workSettings.stock, 450, new Point(600, 221));
         
            fillingParameters();
            SetUpDrawing();
        }
         
        private void fillingParameters()
        {
            if (workSettings.statusBar.version == null)
            {
                // If the file is new
                workSettings.statusBar.version = DateTime.Now.ToString("ddMMyy.hhmmss");
               
            }else
            {
                radioButtonExistingFile.IsChecked = true;
                
            }

            
            RadioButtonsBehaivor();

            // If the file is not new is gonna change worksettings
            //changing listViews
            listViewWorkOffset.ItemsSource = workSettings.workOffsets;
            listViewToolSettings.ItemsSource= workSettings.toolSettings;
            // Blank stock information 
            textBoxExternalDiam.Text = workSettings.stock.externalDiameter.ToString();
            textBoxInternalDiam.Text = workSettings.stock.internalDiameter.ToString();
            textBoxInitial_Z.Text = workSettings.stock.initialPosition.ToString();
            textBoxFinal_Z.Text = workSettings.stock.finalPosition.ToString();
            textBoxFinal_SZ.Text = workSettings.stock.splindleLimit.ToString();

            comboBoxDefaultWS.ItemsSource = workSettings.GetWorkOffsets(true);
            comboBoxDefaultWS.SelectedItem = workSettings.stock.workOffset;
            //Filling file information
            textBoxFileName.Text = workSettings.statusBar.pathFile.fileName;
            textBoxLocalPath.Text = workSettings.statusBar.pathFile.directory;
            
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            Definition();               
            MainPage.Buttons(true);
            Switcher.Switch(Main);
            
        }

        private void Definition()
        {
            UpDateSotck();

            workSettings.Parameters = workSettings.ShowingParameters(workSettings);

            MainPage.listViewOperations.Items.Insert(workSettings.Index,
                     workSettings.SetParameters(workSettings, MainPage));
        }

        private void UpDateSotck()
        {
            double externalDiameter, internalDiameter, initialPosition, finalPosition, splindleLimit;
            WindowsFunctions funtions = new WindowsFunctions();

            double.TryParse(textBoxExternalDiam.Text, out externalDiameter);
            workSettings.stock.externalDiameter = externalDiameter;

            double.TryParse(textBoxInternalDiam.Text, out internalDiameter);
            workSettings.stock.internalDiameter = internalDiameter;

            double.TryParse(textBoxInitial_Z.Text, out initialPosition);
            workSettings.stock.initialPosition = initialPosition;

            double.TryParse(textBoxFinal_Z.Text, out finalPosition);
            workSettings.stock.finalPosition = finalPosition;

            double.TryParse(textBoxFinal_SZ.Text, out splindleLimit);
            workSettings.stock.splindleLimit = splindleLimit;

            

                    
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(Main);
        }

        private void MouseMoveControl(object sender, MouseEventArgs e)
        {
            ControlStatusBar();
            updateDrawing();
            comboBoxDefaultWS.ItemsSource = workSettings.GetWorkOffsets(true);
        }

        private void ControlStatusBar()
        {

            #region statusBar ready
            if (workSettings.statusBar.statusBoolean)
            {
                buttonSave.IsEnabled = true;
                workSettings.statusBar.status = workSettings.statusBar.status;
            }
            else
            {
                buttonSave.IsEnabled = false;

            }
            #endregion

            #region ProgressBar
            if (progressBar.Value == progressBar.Maximum)
            {
                progressBar.Visibility = Visibility.Hidden;
                workSettings.statusBar.status = StateFile.Ready;
            }
            #endregion

            LabelVersion.Content = workSettings.statusBar.version;
            LabelFile.Content = workSettings.statusBar.pathFile.fileName;
            LabelStatus.Content = workSettings.statusBar.status;
        }

        #region Worksettings
        private void radioButtonFiles_Clicked(object sender, RoutedEventArgs e)
        {
            RadioButtonsBehaivor();
        }

        private void RadioButtonsBehaivor()
        {
            if (radioButtonExistingFile.IsChecked == true)
            { 
                buttonSave.Visibility = Visibility.Hidden;
                textBoxFileName.IsEnabled = false;
              
            }
            else
            {
               buttonSave.Visibility = Visibility.Visible;
                textBoxFileName.IsEnabled = true;
            }
           

        }    

        private void buttonBrowsePath_Click(object sender, RoutedEventArgs e)
        {
            WindowsFunctions funtions = new WindowsFunctions();

            if (radioButtonExistingFile.IsChecked == true)
            {
                PathDefinition path = new PathDefinition();
                path = funtions.fileBrowser("Work Settings(wstt)|*.wstt");
                
                if (path.directory != null)
                {
                    textBoxFileName.Text = path.fileName;
                    textBoxLocalPath.Text = path.directory;
                    workSettings.statusBar.statusBoolean = true;
                    workSettings.statusBar.status = StateFile.Loading;
                    loadFileInformation();
                    WindowsFunctions function = new WindowsFunctions();
                    function.animateProgressBar(progressBar, 1);
                }
            }
            else
            {
                string auxPath = funtions.folderBrowser();
                if (auxPath != "")
                {
                    textBoxLocalPath.Text = auxPath;
                    workSettings.statusBar.statusBoolean = true;
                  

                }
            }         

        }

        private void buttonLoadSave_Click(object sender, RoutedEventArgs e)
        {



                workSettings.statusBar.status = StateFile.Saving;
                saveFileInformation();
            

            WindowsFunctions function = new WindowsFunctions();
            function.animateProgressBar(progressBar, 1);
            
        }

        private void saveFileInformation()
        {


            ExportAndImportToFIle fnc = new ExportAndImportToFIle();

            workSettings.statusBar.pathFile.directory = textBoxLocalPath.Text;
            workSettings.statusBar.pathFile.fileName = System.IO.Path.GetFileNameWithoutExtension(textBoxFileName.Text)+"." + extensionFiles.wstt;
            ControlStatusBar();
            
            fnc.WriteToBinaryFile<WorkSettings>(workSettings.statusBar.pathFile.GetFullName(),workSettings);
        } 

        private void loadFileInformation()
        {

            ExportAndImportToFIle fnc = new ExportAndImportToFIle();
            string path = textBoxLocalPath.Text + textBoxFileName.Text;
            workSettings = fnc.ReadFromBinaryFile<WorkSettings>(path);
            fillingParameters();
        }

        private void comboBoxDefaultWS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxDefaultWS.SelectedItem != null)
            {
                workSettings.stock.workOffset = (Gcode)comboBoxDefaultWS.SelectedItem;
            }
        }

        #endregion

        #region WorkOffset
        private void buttonAddItemListViewWorkOffset_Click(object sender, RoutedEventArgs e)
        {
           
            workSettings.workOffsets.Add(new WorkOffsetPointPosition(listViewWorkOffset.Items.Count));
            listViewWorkOffset.Items.Refresh();
           
        }
        private void buttonDeleteItemListViewWorkOffset_Click(object sender, RoutedEventArgs e)
        {
            if (listViewWorkOffset.SelectedItem != null)
            {
                int index = listViewWorkOffset.SelectedIndex;
                workSettings.workOffsets.RemoveAt(index);
                listViewWorkOffset.Items.Refresh();
            }
        }

        #endregion

        #region Tool Settings

        private void buttonNewTool_Click(object sender, RoutedEventArgs e)
            {
            workSettings.counterTools++;   
            workSettings.toolSettings.Add(new Tool(workSettings.counterTools));
            listViewToolSettings.Items.Refresh();
         
        }

        private void buttonDeleteToolItem_Click(object sender, RoutedEventArgs e)
        {
            if (listViewToolSettings.SelectedItem != null)
            {
                int index = listViewToolSettings.SelectedIndex;
                if (workSettings.toolSettings[index].isEdgeTool)
                {
                    workSettings.toolSettings.RemoveAt(index);
                }
                else
                {
                    if (MessageBox.Show("If the current Tool is eliminated every Set-Tool related with it will be also eliminated. Continue?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    { 
                        int localization = workSettings.toolSettings[index].localization;
                        for (int i =0; i < workSettings.toolSettings.Count; i++)
                        {
                            if(workSettings.toolSettings[i].localization == localization)
                            {
                                workSettings.toolSettings.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }

               OrderListTools();
              
               
            }
        }

        private void OrderListTools()
        {
            int count = workSettings.toolSettings.Count;
            workSettings.counterTools = 0;
            for (int i = 0; i < count; i++)
            {
                if (workSettings.toolSettings[i].isEdgeTool == false)
                {
                    workSettings.counterTools++;  
                }
                workSettings.toolSettings[i].localization = workSettings.counterTools;
            }
            listViewToolSettings.Items.Refresh();
        }

        private void buttonNewEdge_Click(object sender, RoutedEventArgs e)
        {
            if(listViewToolSettings.SelectedItem != null)
            {
                
                int index = listViewToolSettings.SelectedIndex;
                index=selectMainTool(index);
              
                Tool newEdgeCuttingTool = new Tool(0);
                newEdgeCuttingTool.toolName = "New Edge Tool";
                newEdgeCuttingTool.localization = workSettings.toolSettings[index].localization;
                newEdgeCuttingTool.toolSet= workSettings.toolSettings[index].toolSet + 1;
                newEdgeCuttingTool.isEdgeTool = true;
                newEdgeCuttingTool.toolSet= workSettings.toolSettings[index].toolSet + 1;
                workSettings.toolSettings.Insert(index +1,newEdgeCuttingTool);
                workSettings.toolSettings[index].toolSet++;
                listViewToolSettings.Items.Refresh();
                listViewToolSettings.SelectedIndex = index;
                
                
                
            }
        }

        private int selectMainTool(int index)
        {
            
            while (workSettings.toolSettings[index].isEdgeTool == true)
            {
                index--;
            }
            return index;
        }


        #endregion

        #region Drawing
        private void SetUpDrawing()
        {
            labelScale.Content = drawing.scaleLabel;
         
            SettingOriginDraw(drawing.ProfileStartPoint, drawing.scaleOrigin*2); // method of this class ProfilePage
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

        private void updateDrawing()
        {
            UpDateSotck();
            drawing = new Drawing(workSettings.stock, 450, new Point(600, 221));
            SetUpDrawing();
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

        #endregion

      
    }


}
