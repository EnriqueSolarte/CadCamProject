﻿using System;
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
       
      
        public wSettingPage(Main main)
        { 
            Main = main;  
            MainPage = main;
            InitializeComponent();
            workSettings = new WorkSettings();
           
            workSettings = workSettings.GetParameters(MainPage);
            fillingParameters();
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
            double externalDiameter, internalDiameter, initialPosition, finalPosition, splindleLimit;
            WindowsFunctions funtions = new WindowsFunctions();

            double.TryParse(textBoxExternalDiam.Text,out externalDiameter);
            workSettings.stock.externalDiameter = externalDiameter;

            double.TryParse(textBoxInternalDiam.Text, out internalDiameter);
            workSettings.stock.internalDiameter = internalDiameter;

            double.TryParse(textBoxInitial_Z.Text, out initialPosition);
            workSettings.stock.initialPosition = initialPosition;

            double.TryParse(textBoxFinal_Z.Text, out finalPosition);
            workSettings.stock.finalPosition = finalPosition;

            double.TryParse(textBoxFinal_SZ.Text, out splindleLimit);
            workSettings.stock.splindleLimit = splindleLimit;

            workSettings.Parameters = workSettings.ShowingParameters(workSettings);

            MainPage.listViewOperations.Items.Insert(workSettings.Index,
                     workSettings.SetParameters(workSettings, MainPage));
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            Switcher.Switch(Main);
        }

        private void MouseMoveControl(object sender, MouseEventArgs e)
        {
            ControlStatusBar();
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
