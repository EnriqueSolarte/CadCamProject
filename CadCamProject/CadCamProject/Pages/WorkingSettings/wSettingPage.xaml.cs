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
            workSettings.upDate = DateTime.Now.ToString("ddMMyy.hhmmss");

            MainPage.listViewOperations.Items.Insert(workSettings.Index,
                workSettings.SetParameters(workSettings, MainPage));     
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(Main);
        }

        private void fillingParameters()
        { 

            if(workSettings.upDate == null)
            {
                statusBarInformation.version = DateTime.Now.ToString("ddMMyy.hhmmss");
                statusBarInformation.savedFile = false;
            }

            statusBarInformation.changed = true;
            statusBarInformation.fileName = workSettings.fileName;
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
                textBoxFileName.Text = System.IO.Path.GetFileNameWithoutExtension(workSettings.fileName)
                                     + workSettings.duplicatedFilePrefix+workSettings.extension;
            }else
            {
                textBoxFileName.IsEnabled = false;
                buttonLoadSave.Content = ButtonContents.Load;
                textBoxFileName.Text = System.IO.Path.GetFileNameWithoutExtension(workSettings.fileName)  
                                    + workSettings.extension;
            }

        }

        private void buttonBrowsePath_Click(object sender, RoutedEventArgs e)
        {
            WindowsFunctions funtions = new WindowsFunctions();
           
            if (radioButtonExistingFile.IsChecked == true)
            {
                PathDefinition path = new PathDefinition();
                path = funtions.fileBrowser("CAM prog (.opt)|*.opt");
                workSettings.fileName = path.fileName;
                workSettings.pathDirectory = path.directory;
                if (path != null)
                    statusBarInformation.readyToSave = true;
            }
            else
            {
                workSettings.pathDirectory = funtions.folderBrowser();
            }


            textBoxFileName.Text = workSettings.fileName + workSettings.extension;
            textBoxLocalPath.Text = workSettings.pathDirectory;

        }

        private void MouseMoveControl(object sender, MouseEventArgs e)
        {
            #region statusBar readyToSave
            if (statusBarInformation.readyToSave)
            {
                buttonLoadSave.IsEnabled = true;
            }else
            {
                buttonLoadSave.IsEnabled = false;
            }
            #endregion

            #region statusBar savedFile
            if (statusBarInformation.savedFile)
            {
                statusBarInformation.fileName = workSettings.fileName;
            }else
            {
                statusBarInformation.fileName = workSettings.fileName + "*";
            }
            #endregion

            #region ProgressBar
            if (progressBar.Value == progressBar.Maximum)
            {
                progressBar.Visibility = Visibility.Hidden;
                statusBarInformation.status = StateToFile.Ready;
            }
            #endregion

            #region statusBar changed
            if (statusBarInformation.changed)
            {
                statusBarInformation.changed = false;
                LabelVersion.Content = statusBarInformation.version;
                LabelStatusFile.Content = statusBarInformation.fileName;
                LabelStatusProgressBar.Content = statusBarInformation.status;
            }
            #endregion
        }

        private void buttonLoadSave_Click(object sender, RoutedEventArgs e)
        {
            if (statusBarInformation.readyToSave)
            {
                WindowsFunctions function = new WindowsFunctions();
                function.animateProgressBar(progressBar, 5);

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
            }
           
        }
       
        private void saveFileInformation()
        {
            string fileName = workSettings.pathDirectory + "\\" 
                             + System.IO.Path.GetFileNameWithoutExtension(textBoxFileName.Text) 
                             + workSettings.extension;
            //it is necessary create a method to create and staore the format in dataCOntent
            MessageBox.Show("It is necessary create a method tho save");
            File.WriteAllText(fileName, workSettings.dataContent);
            statusBarInformation.savedFile = true;
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
            newPointPosition.Add(new pointPosition());
            listViewWorkOffset.Items.Insert(listViewWorkOffset.Items.Count,newPointPosition);
            statusBarInformation.savedFile = false;
        }




        #endregion

        
    }

    
}
