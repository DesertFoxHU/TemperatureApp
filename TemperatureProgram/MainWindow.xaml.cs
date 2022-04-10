﻿using Microsoft.Data.Sqlite;
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
        private DayTemperature dayTemperature;

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            SqlConnection.Initialize();
            DatePicker.DisplayDate = DateTime.Now;
            DatePicker.SelectedDate = DateTime.Now;
            SqlConnection.OpenConnection();

            this.StateChanged += (sender, e) => SetCurrentDate(CurrentDate);
            this.SizeChanged += (sender, e) => SetCurrentDate(CurrentDate);
        }

        private void SetCurrentDate(DateTime time)
        {
            CurrentDate = time;
            dayTemperature = SqlConnection.QueryDay(time);
            CanvasDrawer.Draw(Canvas.ActualWidth, Canvas.ActualHeight, dayTemperature);
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
            SetCurrentDate(DateTime.Now);
        }

        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            DataWindow window = new DataWindow();
            window.Show();
        }
    }
}
