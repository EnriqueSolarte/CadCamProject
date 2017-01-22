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


namespace CadCamProject.Pages
{

    public partial class wSettingPage : UserControl
    {
        UserControl Main;
        Main MainPage;
        WorkSettings workSettings;

        public wSettingPage(Main main, int index)
        { 
            Main = main;
            MainPage = main;
            InitializeComponent();
            workSettings = new WorkSettings();
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
            workSettings.upDate = DateTime.Now.ToString();
         
            MainPage.listViewOperations.Items.Insert(workSettings.Index,
                workSettings.SetParameters(workSettings, MainPage));     
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(Main);
        }

        private void fillingParameters()
        {
            textBlockVersion.Text = workSettings.upDate;

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
                buttonLoadSave.Content = "Save File";
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
                buttonLoadSave.Content = "Save File";
                textBoxFileName.Text = System.IO.Path.GetFileNameWithoutExtension(workSettings.fileName) + workSettings.duplicatedFilePrefix+workSettings.extension;
            }else
            {
                textBoxFileName.IsEnabled = false;
                buttonLoadSave.Content = "Load";
                textBoxFileName.Text = System.IO.Path.GetFileNameWithoutExtension(workSettings.fileName) + workSettings.extension;
            }

        }

        private void buttonBrowsePath_Click(object sender, RoutedEventArgs e)
        {
            if (radioButtonExistingFile.IsChecked == true)
            {
                fileBrowser();
            }
            else
            {
                folderBrowser();
            }
            textBoxFileName.Text = workSettings.fileName + workSettings.extension;
            textBoxLocalPath.Text = workSettings.pathDirectory;

        }

        private void fileBrowser()
        {
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

            dialog.Filter = "CAM prog (.opt)|*.opt";
            dialog.FilterIndex = 1;
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();


            if (result == System.Windows.Forms.DialogResult.OK)
            {
                workSettings.pathDirectory = dialog.FileName;
                workSettings.fileName = System.IO.Path.GetFileNameWithoutExtension(workSettings.pathDirectory);
                workSettings.pathDirectory = System.IO.Path.GetDirectoryName(workSettings.pathDirectory);
            }

        }

        private void folderBrowser()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                workSettings.pathDirectory = dialog.SelectedPath;
                workSettings.fileName = System.IO.Path.GetFileNameWithoutExtension(textBoxFileName.Text);
            }
        }

        private void buttonLoadSave_Click(object sender, RoutedEventArgs e)
        {
            if (radioButtonExistingFile.IsChecked == true)
            {
                loadFileInformation();
            }
            else
            {
                saveFileInformation();
            }
            
        }

        private void saveFileInformation()
        {
            string fileName = workSettings.pathDirectory + "\\" + System.IO.Path.GetFileNameWithoutExtension(textBoxFileName.Text) + workSettings.extension;
            File.WriteAllText(fileName, workSettings.savingFormat);
        }

        private void loadFileInformation()
        {
            MessageBox.Show("Is not implement yet >> :-)");
        }

        #endregion


    }
}
