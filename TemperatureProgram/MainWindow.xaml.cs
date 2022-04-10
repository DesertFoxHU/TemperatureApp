using Microsoft.Data.Sqlite;
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

namespace TemperatureProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        private DayTemperature dayTemperature;
        private DataManipulationWindow dataWindow;

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            SqlConnection.Initialize();
            DatePicker.DisplayDate = DateTime.Now;
            DatePicker.SelectedDate = DateTime.Now;
            DayOfWeekLabel.Content = DateTime.Now.DayOfWeek.ToString();
            SqlConnection.OpenConnection();
            dataWindow = new DataManipulationWindow();
            
            this.Closing += (sender, e) => dataWindow.Close();
        }

        public void DrawDiagram()
        {
            if (!DatePicker.SelectedDate.HasValue) return;
            DrawDiagram(DatePicker.SelectedDate.Value);
        }

        public void DrawDiagram(DateTime time)
        {
            dayTemperature = SqlConnection.QueryDay(time);
            CanvasDrawer.Draw(Canvas.ActualWidth, Canvas.ActualHeight, dayTemperature);
            string dayName = new System.Globalization.CultureInfo("hu-HU").DateTimeFormat.DayNames[(int)time.DayOfWeek];
            dayName = char.ToUpper(dayName[0]) + dayName.Substring(1);
            DayOfWeekLabel.Content = dayName;

            if (dayTemperature.Temperature.Count == 0) NoData.Visibility = Visibility.Visible;
            else NoData.Visibility = Visibility.Hidden;

            Avarage.Content = string.Format("Átlag: {0:0.0}°C", dayTemperature.GetAvarage());
            Deviation.Content = string.Format("Szórás: {0:0.0}", dayTemperature.GetDeviation());
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            DrawDiagram(DatePicker.SelectedDate.GetValueOrDefault());
        }

        private void DatePickerTextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DatePicker.IsDropDownOpen = true;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            DrawDiagram(DateTime.Now);
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            if (dataWindow == null || !dataWindow.IsActive) dataWindow = new DataManipulationWindow();
            dataWindow.Show();
        }
    }
}
