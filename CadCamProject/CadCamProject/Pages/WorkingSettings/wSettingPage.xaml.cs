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

namespace CadCamProject.Pages
{

    public partial class wSettingPage : UserControl
    {
        UserControl Main;
        Main MainPage;
        WorkSettings workSettings;
        StatusBar statusBarInformation;
        SpecialChart specialChart;

        public wSettingPage(Main main)
        { 
            Main = main;  
            MainPage = main;
            InitializeComponent();
            workSettings = new WorkSettings();
            statusBarInformation = new StatusBar();
            specialChart = new SpecialChart();
            workSettings = workSettings.GetParameters(MainPage);
            fillingParameters();
        }

        private void fillingParameters()
        {
            if (workSettings.version == null)
            {
                // If the file is new
                workSettings.version = DateTime.Now.ToString("ddMMyy.hhmmss");
                statusBarInformation.ready = false;
            }

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
            
            //Filling file information
            textBoxFileName.Text = workSettings.file.fileName + workSettings.file.extension;
            textBoxLocalPath.Text = workSettings.file.directory;
            statusBarInformation.fileName = workSettings.file.fileName;
        }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            Definition();               
            MainPage.Buttons(true);
            Switcher.Switch(Main);
            
        }

        private void Definition()
        {
            WindowsFunctions funtions = new WindowsFunctions();
            
            workSettings.stock.externalDiameter = Double.Parse(textBoxExternalDiam.Text, CultureInfo.InvariantCulture); 
            workSettings.stock.internalDiameter = Double.Parse(textBoxInternalDiam.Text, CultureInfo.InvariantCulture);
            workSettings.stock.initialPosition = Double.Parse(textBoxInitial_Z.Text, CultureInfo.InvariantCulture);
            workSettings.stock.finalPosition = Double.Parse(textBoxFinal_Z.Text, CultureInfo.InvariantCulture);
            workSettings.stock.splindleLimit = Double.Parse(textBoxFinal_SZ.Text, CultureInfo.InvariantCulture);

            workSettings.Parameters = workSettings.ShowingParameters(workSettings);

            MainPage.listViewOperations.Items.Insert(workSettings.Index,
                     workSettings.SetParameters(workSettings, MainPage));
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(Main);
        }

        #region Worksettings
        private void radioButtonFiles_Clicked(object sender, RoutedEventArgs e)
        {
            bool stateCheckBox = false;
            if (radioButtonExistingFile.IsChecked == true)
            {
                stateCheckBox = true;
                checkBoxDuplicateFile.IsEnabled = true;
                checkBoxDuplicateFile_clicked(sender, e);
              
            }
            else
            {

                stateCheckBox = false;
                checkBoxDuplicateFile.IsEnabled = false;
                buttonLoadSave.Content = ButtonContents.Save;
                if (checkBoxDuplicateFile.IsChecked == false)
                {
                    textBoxFileName.IsEnabled = true;
                }
            }

            checkBoxOperations.IsEnabled = stateCheckBox;
            checkBoxProfiles.IsEnabled = stateCheckBox;
            checkBoxBlanksDefinitions.IsEnabled = stateCheckBox;
            checkBoxSettingTools.IsEnabled = stateCheckBox;
            checkBoxWorkOffset.IsEnabled = stateCheckBox;
        }

        private void checkBoxDuplicateFile_clicked(object sender, RoutedEventArgs e)
        {
            if (checkBoxDuplicateFile.IsChecked == true)
            {
                textBoxFileName.IsEnabled = true;
                buttonLoadSave.Content = ButtonContents.Save;
                textBoxFileName.Text = System.IO.Path.GetFileNameWithoutExtension(workSettings.file.fileName)
                                     + workSettings.duplicatedFilePrefix+workSettings.file.extension;
            }else
            {
                textBoxFileName.IsEnabled = false;
                buttonLoadSave.Content = ButtonContents.Load;
                textBoxFileName.Text = System.IO.Path.GetFileNameWithoutExtension(workSettings.file.fileName)  
                                    + workSettings.file.extension;
            }

        }

        private void buttonBrowsePath_Click(object sender, RoutedEventArgs e)
        {
            WindowsFunctions funtions = new WindowsFunctions();
           
            if (radioButtonExistingFile.IsChecked == true)
            {
                PathDefinition path = new PathDefinition();
                path = funtions.fileBrowser("CAM prog (.opt)|*.opt");
                
                if (path.directory != null)
                {
                    textBoxFileName.Text = path.fileName;
                    textBoxLocalPath.Text = path.directory;
                    statusBarInformation.ready = true;
                }
            }
            else
            {
                string auxPath = funtions.folderBrowser();
                if (auxPath != "")
                {
                    textBoxLocalPath.Text = auxPath;
                    statusBarInformation.ready = true;
                }
            }         

        }

        private void MouseMoveControl(object sender, MouseEventArgs e)
        {
            ControlStatusBar();
        }

        private void ControlStatusBar()
        {

            #region statusBar ready
            if (statusBarInformation.ready)
            {
                buttonLoadSave.IsEnabled = true;
                
            }
            else
            {
                buttonLoadSave.IsEnabled = false;
               
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

        private void buttonLoadSave_Click(object sender, RoutedEventArgs e)
        {
             
                if (radioButtonExistingFile.IsChecked == true)
                {
                    statusBarInformation.status = StateToFile.Loading;
                    loadFileInformation();
                }
                else
                {
                    statusBarInformation.status = StateToFile.Saving;
                    saveFileInformation();
                }

            WindowsFunctions function = new WindowsFunctions();
            function.animateProgressBar(progressBar, 1);
           
        }

        private void saveFileInformation()
        {
           
            string fileName = textBoxLocalPath.Text + "\\" 
                             + System.IO.Path.GetFileNameWithoutExtension(textBoxFileName.Text) 
                             + workSettings.file.extension;
            //it is necessary create a method to create and staore the format in dataCOntent
            //MessageBox.Show("It is necessary create a method to save");
            File.WriteAllText(fileName, workSettings.dataContent);

            ControlStatusBar();
            workSettings.file.directory = textBoxLocalPath.Text;
            workSettings.file.fileName = System.IO.Path.GetFileNameWithoutExtension(textBoxFileName.Text);

            statusBarInformation.version = workSettings.version;
            statusBarInformation.fileName = workSettings.file.fileName;
        } 

        private void loadFileInformation()
        {
            MessageBox.Show("Is not implement yet >> :-)");
        }

        #endregion

        #region WorkOffset
        private void buttonAddItemListViewWorkOffset_Click(object sender, RoutedEventArgs e)
        {
           
            workSettings.workOffsets.Add(new pointPosition(listViewWorkOffset.Items.Count));
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
            workSettings.toolSettings.Add(new tool(workSettings.counterTools));
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
              
                tool newEdgeCuttingTool = new tool(0);
                newEdgeCuttingTool.toolName = "New Edge Tool";
                newEdgeCuttingTool.localization = workSettings.toolSettings[index].localization;
                newEdgeCuttingTool.toolSet= workSettings.toolSettings[index].definedSetTools + 1;
                newEdgeCuttingTool.isEdgeTool = true;
                newEdgeCuttingTool.definedSetTools= workSettings.toolSettings[index].definedSetTools + 1;
                workSettings.toolSettings.Insert(index +1,newEdgeCuttingTool);
                workSettings.toolSettings[index].definedSetTools++;
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

        
      }

    
}
