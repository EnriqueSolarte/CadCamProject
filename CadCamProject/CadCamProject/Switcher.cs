using System;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CadCamProject.Pages;

namespace CadCamProject
{
    class Switcher
    {
        public static MainWindow window;

        public static void Switch(UserControl newPage)
        {
            window.Navigate(newPage);
        }
       
    }

    
}
