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
        public const int YAxisMarkerCount = 4;

        public static void Draw(double width, double height, DayTemperature dayTemperature)
        {
            MainWindow.Instance.Canvas.Children.Clear();

            double minTemp = dayTemperature.GetMinTemperature();
            double maxTemp = dayTemperature.GetMaxTemperature();

            #region Drawing axis lines
            DrawLine(0, height, width, height, thickness: 1); //X axis
            DrawLine(0, 0, 0, height, thickness: 1); //Y axis
            #endregion

            double length = (maxTemp - minTemp);
            double heightPerTemp = height / length;
            double difference = maxTemp - length;

            List<Point> points = new List<Point>();

            #region Drawing x crossings and values
            double xSteps = width / 24d;
            for (int i = 0; i < 24; i++)
            {
                double X = xSteps * i;
                DrawLine(X, height, X, height - 10, 1); //TimeMarker lines

                Label timeMarker = CreateText($"{i}:00", 10);
                timeMarker.RenderTransform = new RotateTransform(-45);

                Canvas.SetLeft(timeMarker, X - xSteps);
                Canvas.SetTop(timeMarker, height + 20);

                //Marking points
                if (dayTemperature.Temperature.ContainsKey(i))
                {
                    double temp = dayTemperature.Temperature[i];
                    double Y = height - ((temp - difference) * heightPerTemp);
                    points.Add(new Point(X, Y));

                    Ellipse pointMarker = DrawEllipse(Brushes.Blue.Color, 8);

                    ToolTip tooltip = new ToolTip();
                    tooltip.Content = temp + "°C";

                    pointMarker.ToolTip = tooltip;
                    Canvas.SetLeft(pointMarker, X - (pointMarker.Width / 2));
                    Canvas.SetTop(pointMarker, Y - (pointMarker.Height / 2));
                }
            }
            #endregion

            #region Drawing y axis crossings and markers
            double ySteps = height / YAxisMarkerCount;
            for(int i = 0; i <= YAxisMarkerCount; i++)
            {
                double Y = height - ySteps * i;
                DrawLine(0, Y, 10, Y, 1);

                double temp = (length / YAxisMarkerCount) * i;

                string tempString = string.Format("{0:0.0}", temp + difference);
                Label tempMarker = CreateText($"{tempString} °C", 10);
                tempMarker.HorizontalContentAlignment = HorizontalAlignment.Right;
                tempMarker.HorizontalAlignment = HorizontalAlignment.Right;
                Canvas.SetRight(tempMarker, width+5);
                Canvas.SetTop(tempMarker, Y - 15);
            }
            #endregion

            //Connecting the points
            DrawPolyline(points, Brushes.Aqua.Color, 2);
        }

        private static Line DrawLine(double X1, double Y1, double X2, double Y2, double thickness = 4)
        {
            return DrawLine(X1, Y1, X2, Y2, Brushes.Black.Color, thickness);
        }

        private static Line DrawLine(double X1, double Y1, double X2, double Y2, Color color, double thickness = 4)
        {
            Line line = new Line();
            line.StrokeThickness = thickness;
            line.Stroke = new SolidColorBrush() { Color = color };
            line.X1 = X1;
            line.Y1 = Y1;
            line.X2 = X2;
            line.Y2 = Y2;
            MainWindow.Instance.Canvas.Children.Add(line);
            return line;
        }

        private static Ellipse DrawEllipse(Color color, double thickness = 4)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Stroke = new SolidColorBrush() { Color = color };
            ellipse.Width = thickness;
            ellipse.Height = thickness;
            ellipse.Fill = new SolidColorBrush() { Color = color };
            MainWindow.Instance.Canvas.Children.Insert(0, ellipse);
            Canvas.SetZIndex(ellipse, 1);
            return ellipse;
        }

        private static Label CreateText(string content, double fontSize)
        {
            Label label = new Label();
            label.Content = content;
            label.FontSize = fontSize;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
            MainWindow.Instance.Canvas.Children.Add(label);
            return label;
        }

        private static void DrawPolyline(List<Point> points, Color color, double thickness)
        {
            Polyline polyline = new Polyline();
            polyline.Stroke = new SolidColorBrush() { Color = color };
            polyline.StrokeThickness = thickness;

            PointCollection pointCollection = new PointCollection();
            points.ForEach((point) => pointCollection.Add(point));

            polyline.Points = pointCollection;
            MainWindow.Instance.Canvas.Children.Add(polyline);
        }

    }
}
