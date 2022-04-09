using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TemperatureProgram
{
    internal class CanvasDrawer
    {
        private static Line DrawLine(double X1, double Y1, double X2, double Y2, double thickness = 4)
        {
            return DrawLine(X1, Y1, X2, Y2, Brushes.Black, thickness);
        }

        private static Line DrawLine(double X1, double Y1, double X2, double Y2, SolidColorBrush color, double thickness = 4)
        {
            Line line = new Line();
            line.StrokeThickness = thickness;
            line.Stroke = color;
            line.X1 = X1;
            line.Y1 = Y1;
            line.X2 = X2;
            line.Y2 = Y2;
            MainWindow.Instance.Canvas.Children.Add(line);
            return line;
        }

        private static Ellipse DrawEllipse(SolidColorBrush color, double thickness = 4)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = color;
            ellipse.StrokeThickness = thickness;
            MainWindow.Instance.Canvas.Children.Add(ellipse);
            return ellipse;
        }

        private static Label CreateText(string content, double fontSize)
        {
            Label label = new Label();
            label.Content = content;
            label.FontSize = fontSize;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            return label;
        }

        public static void DrawValues(double width, double height, DayTemperature dayTemperature)
        {
            MainWindow.Instance.Canvas.Children.Clear();

            double minTemp = dayTemperature.GetMinTemperature();
            double maxTemp = dayTemperature.GetMaxTemperature();

            #region Drawing axises
            DrawLine(0, height, width, height, thickness: 1); //X axis
            DrawLine(0, 0, 0, height, thickness: 1); //Y axis
            #endregion

            double tempPerPixel = height / maxTemp;

            #region Drawing inner lines and values
            double xSteps = width / 24d;
            for (int i = 1; i <= 24; i++)
            {
                double X = xSteps * i;
                DrawLine(X, height, X, height-10, 1);
                Label timeMarker = CreateText($"{i-1}:00", 10);
                timeMarker.RenderTransform = new RotateTransform(-45);

                MainWindow.Instance.Canvas.Children.Add(timeMarker);
                Canvas.SetLeft(timeMarker, X - xSteps);
                Canvas.SetTop(timeMarker, height + 15);

                //Points
                double temp = dayTemperature.Temperature.GetValueOrDefault(i-1, 0);
                double Y = temp * tempPerPixel;
                DrawLine(X, height - 10, X, Y, Brushes.Red, 0.5);
            }
            #endregion

            #region Min and Max temperature marker
            Label minMarker = CreateText($"{minTemp} °C", 10);
            MainWindow.Instance.Canvas.Children.Add(minMarker);
            Canvas.SetLeft(minMarker, -50);
            Canvas.SetTop(minMarker, height - 15);

            Label maxMarker = CreateText($"{maxTemp} °C", 10);
            MainWindow.Instance.Canvas.Children.Add(maxMarker);
            Canvas.SetLeft(maxMarker, -50);
            Canvas.SetTop(maxMarker, 0);
            #endregion
        }

    }
}
