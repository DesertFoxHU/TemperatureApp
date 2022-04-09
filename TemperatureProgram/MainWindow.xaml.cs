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
        public static DateTime CurrentDate = DateTime.Now;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RefreshDateLabel()
        {
            CurrentDateLabel.Content = CurrentDate.ToString("yyyy.MM.dd");
        }

        private void PreviousWeekBttn_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddDays(-7);
            RefreshDateLabel();
        }

        private void NextWeekBttn_Click(object sender, RoutedEventArgs e)
        {
            CurrentDate = CurrentDate.AddDays(7);
            RefreshDateLabel();
        }
    }
}
