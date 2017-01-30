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

        public Gcode[] GcodeArrayWorkOfset()
        {
            Gcode[] strArray =
            {
                Gcode.G54,
                Gcode.G55,
                Gcode.G56,
                Gcode.G57,
                Gcode.G58,
                Gcode.G59,
                Gcode.CPP,
            };
            return strArray;
        }

        public wPlane[] wPlaneArray()
        {
            wPlane[] strArray =
            {
              wPlane.XY,
              wPlane.XZ,
            };
            return strArray;
        }

        public CuttingToolType[] CuttingToolTypeArray()
        {
            CuttingToolType[] strArray =
        {
                CuttingToolType.Turning,
                CuttingToolType.Drilling,
                CuttingToolType.Grooving,
                CuttingToolType.Milling,
                CuttingToolType.Threading,
               

            };
        return strArray;
    }

        public int[] ReferenceToolDirectionArray()
        {
            int[] strArray =
            {
                (int)ReferenceToolDirection.pos1,
                (int)ReferenceToolDirection.pos2,
                (int)ReferenceToolDirection.pos3,
                (int)ReferenceToolDirection.pos4,
                (int)ReferenceToolDirection.pos5,
                (int)ReferenceToolDirection.pos6,
                (int)ReferenceToolDirection.pos7,
                (int)ReferenceToolDirection.pos8,
                (int)ReferenceToolDirection.pos9,
                
            };
            return strArray;
        }

        public Mcode[] SpindleControl()
        {
            Mcode[] strArray =
       {
                Mcode.M3,
                Mcode.M4,

            };
            return strArray;
        }

        public RadiusDefinition[] RadiusDefinitionArray()
        {
            RadiusDefinition[] Array =
            {
                RadiusDefinition.byRadius,
                RadiusDefinition.byCoordinateCenter,
            };
            return Array;
        }
        
        public TransitionGeometries[] TransitionGeometriesArray()
        {
            TransitionGeometries[] Array =
            {
               TransitionGeometries.Round,
               TransitionGeometries.Chamfer,
            };
            return Array;
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
            ready = true;
        }
    }
}
