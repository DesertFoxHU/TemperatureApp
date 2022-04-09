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
            Line line = new Line();
            line.StrokeThickness = thickness;
            line.Stroke = System.Windows.Media.Brushes.Black;
            line.X1 = X1;
            line.Y1 = Y1;
            line.X2 = X2;
            line.Y2 = Y2;
            MainWindow.Instance.Canvas.Children.Add(line);
            return line;
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

        public static void DrawBorder(double width)
        {
            double xSteps = width / 24d;
            for (int i = 0; i < 24; i++)
            {
                double X = xSteps * i;
                DrawLine(X, 234, X, 224, 1);
                Label label = CreateText($"{i}:00", 10);
                label.RenderTransform = new RotateTransform(-45);

                MainWindow.Instance.Canvas.Children.Add(label);
                Canvas.SetLeft(label, X - xSteps);
                Canvas.SetTop(label, 234 + 15);
            }
        }

    }
}
