using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DesignPatternsProject
{
    class BasicShape
    {
        private BasicShapeStrategy _shape;
        private Canvas mainCanvas;

        public BasicShape(BasicShapeStrategy shape)
        {
            this._shape = shape;
        }

        public void AddShapeToCanvas(Canvas MainCanvas, Point position, int width, int height)
        {
            Console.WriteLine("Adding shape..");
            mainCanvas = MainCanvas;
            Shape shapeToAdd = _shape.add();

            shapeToAdd.Stroke = new SolidColorBrush(Colors.Black);
            shapeToAdd.Fill = new SolidColorBrush(Colors.Red);

            shapeToAdd.Width = width;
            shapeToAdd.Height = height;

            shapeToAdd.MouseLeftButtonDown += shape_MouseLeftButtonDown;
            shapeToAdd.MouseLeftButtonUp += shape_MouseLeftButtonUp;
            shapeToAdd.MouseMove += shape_MouseMove;

            double centerTop = position.Y-(shapeToAdd.Height/2);
            double centerLeft = position.X-(shapeToAdd.Width/2);

            Canvas.SetTop(shapeToAdd, centerTop);
            Canvas.SetLeft(shapeToAdd, centerLeft);

            MainCanvas.Children.Add(shapeToAdd);
        }

        private void shape_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void shape_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void shape_MouseMove(object sender, MouseEventArgs e)
        {
            
        }
    }
}
