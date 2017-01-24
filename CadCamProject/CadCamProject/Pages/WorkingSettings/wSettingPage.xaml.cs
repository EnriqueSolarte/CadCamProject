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


namespace CadCamProject.Pages
{

    public partial class wSettingPage : UserControl
    {
        UserControl Main;
        Main MainPage;
        WorkSettings workSettings;
        StatusBar statusBarInformation;

        public wSettingPage(Main main, int index)
        { 
            Main = main;
            MainPage = main;
            InitializeComponent();
            workSettings = new WorkSettings();
            statusBarInformation = new StatusBar();
            workSettings = workSettings.GetParameters(MainPage, index);
            fillingParameters();
        }

        private void fillingParameters()
        {
            if (workSettings.verision == null)
            {
                // If the file is new
                workSettings.verision = DateTime.Now.ToString("ddMMyy.hhmmss");
                
            }
            else
            {
                // If the file is not new is gonna change worksettings
            }

            //fill information from worksettings
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
           
            workSettings.TypeImagineOperation = "/Images/WorkSettigs.png";
            workSettings.TypeOperation = "Work Settings";
            workSettings.Parameters = DateTime.Now.ToString("[DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss][DD=hh][MM=mm][YY=-hh][MM=mmss]");
            workSettings.verision = DateTime.Now.ToString("ddMMyy.hhmmss");

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
            function.animateProgressBar(progressBar, 5);
           
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

            statusBarInformation.version = workSettings.verision;
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
            List<pointPosition> newPointPosition = new List<pointPosition>();
            newPointPosition.Add(new pointPosition(listViewWorkOffset.Items.Count.ToString()));
            listViewWorkOffset.Items.Insert(listViewWorkOffset.Items.Count,newPointPosition);

           
        }






        #endregion

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewWorkOffset.Items.Count != 0) {
                workSettings.workOffsets.Clear();
                for (int i = 0; i < listViewWorkOffset.Items.Count; i++)
                {
                    workSettings.workOffsets.AddRange(listViewWorkOffset.Items.GetItemAt(i) as List<pointPosition>);
                }     
            }
        }
            
    }

    
}
