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
    public partial class DataManipulationWindow : Window
    {
        public DataManipulationWindow()
        {
            InitializeComponent();
            List<int> options = new List<int>();
            for(int i = 0; i < 24; i++)
            {
                options.Add(i);
            }
            HourPicker.ItemsSource = options;
        }

        private void RandomFill_Click(object sender, RoutedEventArgs e)
        {
            DateTime time = DateTime.Now;
            Random rnd = new Random();
            for (int hour = 0; hour < 24 * 7; hour++) //hours*days
            {
                double temperature = rnd.NextDouble()*35+0;
                SqlConnection.AddOrUpdateElement(time, temperature);
                time = time.AddHours(1);
            }
            MessageBox.Show("Kész!");
            MainWindow.Instance.DrawDiagram();
        }

        private void RemoveElements_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection.ClearDatabase();
            MessageBox.Show("Kész!");
            MainWindow.Instance.DrawDiagram();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (!DatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Jelölj ki egy dátumot!");
                return;
            }

            DateTime time = DatePicker.SelectedDate.Value;
            string hour = HourPicker.Text;
            string temperatureText = TemperatureBox.Text;
            if(hour == "" || hour == null)
            {
                MessageBox.Show("Jelölj meg egy óra számot!");
                return;
            }

            if(temperatureText == "" || temperatureText == null)
            {
                MessageBox.Show("Nem hagyhatod üresen a hőmérséklet mezőt!");
                return;
            }

            try
            {
                int hourCount = int.Parse(hour);
                time = new DateTime(time.Year, time.Month, time.Day, hourCount, 0, 0);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Nem lehetett konvertálni az időt! Hibakód: " + ex.StackTrace);
                return;
            }

            double temperature = 0;
            try
            {
                temperatureText = temperatureText.Replace(" ", String.Empty);
                temperatureText = temperatureText.Replace("C", String.Empty);
                temperatureText = temperatureText.Replace("°", String.Empty);
                temperatureText = temperatureText.Replace(".", ",");
                temperature = Double.Parse(temperatureText);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Nem lehetett konvertálni a hőmérsékletet! Hibakód: " + ex.StackTrace);
                return;
            }

            SqlConnection.AddOrUpdateElement(time, temperature);
            MessageBox.Show("Sikeresen hozzáadva/frissítve!");
            MainWindow.Instance.DrawDiagram();
        }
    }
}
