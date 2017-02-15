using System;
using System.Collections.Generic;
using System.Data;
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


namespace CadCamProject
{
  
    public partial class Main : UserControl
    {
        

        public Main()
        {
            InitializeComponent();
            this.labelTime.Content = "V 2.15.17";
            Buttons(false);
        }

        public void Buttons(bool v)
        {
            buttonAxialMilling.IsEnabled = v;
            buttonDrilling.IsEnabled = v;
            buttonTurning.IsEnabled = v;
            buttonProfile.IsEnabled = v;
            buttonRadialMilling.IsEnabled = v;
            buttonThreading.IsEnabled = v;
            buttonTrasnferSpindle.IsEnabled = v;
            buttonGrooving.IsEnabled = v;
           
        }

        private void doubleCLlick(object sender, MouseButtonEventArgs e)
        {
            int index = this.listViewOperations.SelectedIndex;

            if (index != -1)
            {
                var selectedItem = (dynamic)listViewOperations.Items[index];
                TypeOperations typeOperation = (TypeOperations)selectedItem[0].typeOperation;

                switch (typeOperation)
                {
                    case TypeOperations.Work_Settings:
                        Switcher.Switch(new wSettingPage(this));
                        break;
                    case TypeOperations.Profile:
                        Switcher.Switch(new ProfilePage(this, index));
                        break;

                    case TypeOperations.Turning:
                        Switcher.Switch(new TurningPage(this, index));
                        break;
                }

            } 

        }
        private void gotoWorkSettings()
        {
            Switcher.Switch(new wSettingPage(this));
        }
       
        #region Rezing ListView
        private void ChangeSize(object sender, SizeChangedEventArgs e)
        {
            double remainingSpace = this.listViewOperations.ActualWidth;

            if (remainingSpace > 0)
            {
                (this.listViewOperations.View as GridView).Columns[0].Width = 50;
                (this.listViewOperations.View as GridView).Columns[1].Width = 50;
                (this.listViewOperations.View as GridView).Columns[2].Width = 200;
                (this.listViewOperations.View as GridView).Columns[3].Width = Math.Ceiling(remainingSpace - 50 -50 - 215);

            }
        }


        #endregion

        private void buttonWorkSettings_Click(object sender, RoutedEventArgs e)
        {
            gotoWorkSettings();
        }

        private void buttonProfile_Click(object sender, RoutedEventArgs e)
        {
            int index = this.listViewOperations.Items.Count + 1;

            Switcher.Switch(new ProfilePage(this, index));

        }

        private void buttonTurning_Click(object sender, RoutedEventArgs e)
        {
            int index = this.listViewOperations.Items.Count + 1;

            Switcher.Switch(new TurningPage(this, index));
        }
    }
}
 
