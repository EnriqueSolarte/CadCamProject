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
using CadCamProject;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MessageBox.Show(GetAngule());
        }

        private string GetAngule()
        {
            CadCamProject.Line line1 = new CadCamProject.Line(10, 25, 30,45);
            Vector vector1 = new Vector(10, 25);
            Vector vector2 = new Vector(30, 45);
            

            Vector R = Vector.Subtract(vector2, vector1);
            R = line1.ToVector();
            return R.ToString();
        }
    }
}
