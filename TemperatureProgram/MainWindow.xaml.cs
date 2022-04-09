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
        private static DateTime CurrentDate;

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            SqlConnection.Initialize();
            DatePicker.DisplayDate = DateTime.Now;
            DatePicker.SelectedDate = DateTime.Now;
            SetCurrentDate(DateTime.Now);
            SqlConnection.OpenConnection();
        }

        private void SetCurrentDate(DateTime time)
        {
            CurrentDate = time;
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            SetCurrentDate(DatePicker.SelectedDate.GetValueOrDefault());
        }

        private void DatePickerTextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DatePicker.IsDropDownOpen = true;
        }

        private void DatePicker_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
            SetCurrentDate(DatePicker.DisplayDate);
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            CanvasDrawer.DrawBorder(Canvas.ActualWidth);
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            DataWindow window = new DataWindow();
            window.Show();
        }
    }
}
