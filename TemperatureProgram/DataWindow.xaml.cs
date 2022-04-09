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
using System.Windows.Shapes;

namespace TemperatureProgram
{
    /// <summary>
    /// Interaction logic for DataWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {
        public DataWindow()
        {
            InitializeComponent();
        }

        private void RandomFill_Click(object sender, RoutedEventArgs e)
        {
            DateTime time = DateTime.Now;
            Random rnd = new Random();
            for (int hour = 0; hour < 24 * 30; hour++) //hours*days
            {
                double temperature = rnd.NextDouble()*35+0;
                SqlConnection.AddElement(time, temperature);
                time = time.AddHours(1);
            }
            MessageBox.Show("Done!");
        }
    }
}
