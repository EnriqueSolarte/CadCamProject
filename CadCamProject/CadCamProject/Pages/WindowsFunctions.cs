using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;

namespace CadCamProject.Pages
{
    class WindowsFunctions
    {

        public PathDefinition fileBrowser(string filter)
        {
            PathDefinition file = new PathDefinition();
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

            //dialog.Filter = "CAM prog (.opt)|*.opt";
            dialog.Filter = filter;
            dialog.FilterIndex = 1;
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();


            if (result == System.Windows.Forms.DialogResult.OK)
            {
                file.fileName = System.IO.Path.GetFileNameWithoutExtension(dialog.FileName);
                file.directory = System.IO.Path.GetDirectoryName(dialog.FileName);
            }
            return file;
        }

        public string folderBrowser()
        {
            string directory="";
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                directory = dialog.SelectedPath;
                
            }
            return directory;
        }

        public void animateProgressBar(ProgressBar progressBar, double time)
        {
            progressBar.Visibility = Visibility.Visible;
            progressBar.SetCurrentValue(ProgressBar.ValueProperty, 0.00);
            Duration duration = new Duration(TimeSpan.FromSeconds(time));
            DoubleAnimation doubleAnimation = new DoubleAnimation(200.0, duration);
            progressBar.BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);

        }

        public string[] GcodeArrayWorkOfset()
        {
            string[] strArray =
            {
                Gcode.G54.ToString(),
                Gcode.G55.ToString(),
                Gcode.G56.ToString(),
                Gcode.G57.ToString(),
                Gcode.G58.ToString(),
                Gcode.G59.ToString(),
    };
            return strArray;
        }
    }

    public class PathDefinition
    {
        public string directory { get; set; }
        public string fileName { get; set; }
        public string extension { get; set; }
        

    }

    public class StatusBar
    {
        public string version { get; set; }
        public string fileName { get; set; }
        public StateToFile status { get; set; }
        public bool ready { get; set; }
        
        public StatusBar()
        {
            status = StateToFile.Unsaved;
        }
    }
}
