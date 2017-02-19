using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Animation;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

namespace CadCamProject
{
    public class WindowsFunctions
    {

        public PathDefinition fileBrowser(string filter)
        {
            PathDefinition file = new PathDefinition();
            System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

           
            dialog.Filter = filter;
            dialog.FilterIndex = 1;
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();


            if (result == System.Windows.Forms.DialogResult.OK)
            {
                file.fileName = System.IO.Path.GetFileName(dialog.FileName);
                file.directory = System.IO.Path.GetDirectoryName(dialog.FileName)+"\\";
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
                directory = dialog.SelectedPath+"\\";
                
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
                Gcode.CP,
            };
            return strArray;
        }

        public WorkingPlane[] wPlaneArray()
        {
            WorkingPlane[] strArray =
            {
              WorkingPlane.XY,
              WorkingPlane.ZX,
            };
            return strArray;
        }

        internal ArcDirection[] ArcDirectionArray()
        {
            ArcDirection[] Array =
            {
              ArcDirection.CW,
              ArcDirection.CCW,
            };
            return Array;
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
        
        public TypeTransitionGeometry[] TransitionGeometriesArray()
        {
            TypeTransitionGeometry[] Array =
            {
               TypeTransitionGeometry.Round,
               TypeTransitionGeometry.Chamfer,
            };
            return Array;
        }

        public TurningRemovaltype[] TurningRemovalTypeArray()
        {
            TurningRemovaltype[] Array =
            {
               TurningRemovaltype.byLongitudinal,
               TurningRemovaltype.byTransverse,
               TurningRemovaltype.byFollowingContour,
            };
            return Array;
        } 
        public TurningType[] TurningTypeArray()
        {
            TurningType[] data =
            {
                TurningType.externalTurning,
                TurningType.internalTurning,
            };
            return data;
        }

    }
    [Serializable]
    public class PathDefinition
    {
        public string directory { get; set; }
        public string fileName { get; set; }  
        
        public string GetFullName()
        {

            string fullName;
            fullName = directory + fileName;
            return fullName;
        }      
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

    public static class MyExtensions
    {
        public static Vector GetNormalVector_RA(this Vector _vector, ArcDirection _direction)
        {
            Vector normalVector;
            if (_direction == ArcDirection.CW)
            {

                normalVector = new Vector(-_vector.Y, _vector.X);
            }
            else
            {
              
                normalVector = new Vector(_vector.Y, -_vector.X);
            }
            return normalVector;
        }

        public static Vector GetNormalVector_AR(this Vector _vector, ArcDirection _direction)
        {
            Vector normalVector;
            if (_direction == ArcDirection.CCW)
            {
                normalVector = new Vector(-_vector.Y, _vector.X);
            }
            else
            {
                normalVector = new Vector(_vector.Y, -_vector.X);
            }
            return normalVector;
        }


    }

    public class ExportAndImportToFIle
    {
        #region Save DATA
        public void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, objectToWrite);
            }
        }

        /// <summary>
        /// Reads an object instance from a binary file.
        /// </summary>
        /// <typeparam name="T">The type of object to read from the XML.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the binary file.</returns>
        public T ReadFromBinaryFile<T>(string filePath)
        {
            using (Stream stream = File.Open(filePath, FileMode.Open))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (T)binaryFormatter.Deserialize(stream);
            }
        }
        #endregion saveDATa
    }

    public class GettingInformation
    {
        public List<Profile> GetListProfiles(Main MainPage)
        {
            List<Profile> _listProfile = new List<Profile>();

            int count = MainPage.listViewOperations.Items.Count;

            foreach (var operation in (dynamic)MainPage.listViewOperations.Items)
            {
                if (operation[0].typeOperation == TypeOperations.Profile)
                {
                    _listProfile.Add((Profile)operation[0]);
                }
            }

            return _listProfile;
        }

        public List<string> GetStringList(List<Profile> profileList)
        {
            List<string> _profileList = new List<string>();
            

            foreach (Profile _profile in profileList)
            {  
                _profileList.Add(_profile.GetDataString());
            }
            return _profileList;
        }

    }
}
