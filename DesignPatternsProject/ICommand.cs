using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;

namespace DesignPatternsProject
{
    interface ICommand
    {
        void Execute();
    }

    class AddShape : ICommand
    {
        private Canvas mainCanvas;
        private String text;
        private MouseButtonEventArgs e;

        public AddShape(Canvas mainCanvas, String text, MouseButtonEventArgs e)
        {
            this.mainCanvas = mainCanvas;
            this.text = text;
            this.e = e;
        }

        public void Execute()
        {
            if (text == "Rectangle")
            {
                BasicShape rectangle = new BasicShape(new AddRectangle());
                rectangle.AddShapeToCanvas(mainCanvas, e.GetPosition(mainCanvas), 75, 75);
            }
            else if (text == "Ellipse")
            {
                BasicShape ellipse = new BasicShape(new AddEllipse());
                ellipse.AddShapeToCanvas(mainCanvas, e.GetPosition(mainCanvas), 75, 75);
            }
        }
    }

    class MoveShapes : ICommand
    {
        private Canvas mainCanvas;
        private Point selectedElementPosition = new Point();
        private List<Shape> selectedShapes = new List<Shape>();
        private Point originMousePosition = new Point();
        private System.Windows.Input.MouseEventArgs e;

        public MoveShapes(Point selectedElementPosition, Point originMousePosition, System.Windows.Input.MouseEventArgs e, List<Shape> selectedShapes)
        {
            this.selectedShapes = selectedShapes;
            this.selectedElementPosition = selectedElementPosition;
            this.originMousePosition = originMousePosition;
            this.e = e;
        }

        public void Execute()
        {
            Point newMousePosition = e.GetPosition(mainCanvas);
            Shape currentShape = (Shape)e.Source;

            selectedElementPosition.X += newMousePosition.X - originMousePosition.X;
            selectedElementPosition.Y += newMousePosition.Y - originMousePosition.Y;

            for (int i = 0; i < selectedShapes.Count(); i++)
            {
                double offsetTempShapeX = Canvas.GetLeft(selectedShapes[i]) - Canvas.GetLeft(currentShape);
                double offsetTempShapeY = Canvas.GetTop(selectedShapes[i]) - Canvas.GetTop(currentShape);

                if (currentShape != selectedShapes[i])
                {
                    Canvas.SetLeft(selectedShapes[i], selectedElementPosition.X + offsetTempShapeX);
                    Canvas.SetTop(selectedShapes[i], selectedElementPosition.Y + offsetTempShapeY);
                }
            }

            Canvas.SetLeft(currentShape, selectedElementPosition.X);
            Canvas.SetTop(currentShape, selectedElementPosition.Y);

            originMousePosition.X = newMousePosition.X;
            originMousePosition.Y = newMousePosition.Y;
        }
    }

    class CreateGroup : ICommand
    {
        List<Shape> selectedShapes = new List<Shape>();

        public CreateGroup(List<Shape> selectedShapes)
        {
            this.selectedShapes = selectedShapes;
        }

        public void Execute()
        {
            if (selectedShapes.Count() <= 1) return;

            Composite group = new Composite();
            for(int i = 0; i < selectedShapes.Count(); i++)
                group.Add(selectedShapes[i]);

            Console.WriteLine("Create group..");
            List<Shape> groupShapes = group.getChildren();

            for (int i = 0; i < groupShapes.Count(); i++)
                Console.WriteLine(groupShapes[i]);
        }
    }

    class DisbandGroup : ICommand
    {
        List<Shape> selectedShapes = new List<Shape>();

        public DisbandGroup(List<Shape> selectedShapes)
        {
            this.selectedShapes = selectedShapes;
        }

        public void Execute()
        {

        }
    }

    class UndoRedo : ICommand
    {
        User user = new User();
        Canvas mainCanvas = new Canvas();
        private string undoRedo;

        public UndoRedo(User user, Canvas mainCanvas, string undoRedo)
        {
            this.user = user;
            this.mainCanvas = mainCanvas;
            this.undoRedo = undoRedo;
        }

        public void Execute()
        {
            if(undoRedo == "redo")
            {
                if (user.currentStateIndex >= user.commandStates.Count()-1) return;
                user.currentStateIndex++;
                Console.WriteLine(user.currentStateIndex);
                Console.WriteLine(user.commandStates.Count());
            }
            else if (undoRedo == "undo")
            {
                if (user.currentStateIndex <= 0) return;
                user.currentStateIndex--;
            }

            //Clear canvas of shapes
            mainCanvas.Children.RemoveRange(0, mainCanvas.Children.Count);

            string[] words = user.commandStates[user.currentStateIndex].Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                double temp_x, temp_y;
                int width, height;
                Point temp_point = new Point(0, 0);
                if (words[i] == "rectangle" || words[i] == "ellipse")
                {
                    temp_x = Convert.ToDouble(words[i + 1]);
                    temp_y = Convert.ToDouble(words[i + 2]);
                    width = Int32.Parse(words[i + 3]);
                    height = Int32.Parse(words[i + 4]);
                    temp_point = new Point(temp_x + (width / 2), temp_y + (height / 2));
                    if (words[i] == "rectangle")
                    {
                        BasicShape rectangle = new BasicShape(new AddRectangle());
                        rectangle.AddShapeToCanvas(mainCanvas, temp_point, width, height);
                    }
                    else if (words[i] == "ellipse")
                    {
                        BasicShape ellipse = new BasicShape(new AddEllipse());
                        ellipse.AddShapeToCanvas(mainCanvas, temp_point, width, height);
                    }
                }
            }
        }
    }

    class RemoveShape : ICommand
    {
        public void Execute()
        {

        }
    }

    class ResizePlus : ICommand
    {
        private List<Shape> selectedShapes = new List<Shape>();

        public ResizePlus(List<Shape> selectedShapes)
        {
            this.selectedShapes = selectedShapes;
        }

        public void Execute()
        {
            for (int i = 0; i < selectedShapes.Count; i++)
            {
                selectedShapes[i].Width += 5;
                selectedShapes[i].Height += 5;
            }
        }
    }

    class ResizeMinus : ICommand
    {
        private List<Shape> selectedShapes = new List<Shape>();

        public ResizeMinus(List<Shape> selectedShapes)
        {
            this.selectedShapes = selectedShapes;
        }

        public void Execute()
        {
            for (int i = 0; i < selectedShapes.Count; i++)
            {
                selectedShapes[i].Width -= 5;
                selectedShapes[i].Height -= 5;
            }
        }
    }

    class Import : ICommand
    {
        private Canvas mainCanvas;

        public Import(Canvas mainCanvas)
        {
            this.mainCanvas = mainCanvas;
        }

        public void Execute()
        {
            Console.WriteLine("Import..");

            string result;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string path = openFileDialog.FileName;

                using (StreamReader sr = new StreamReader(path))
                {
                    result = sr.ReadToEnd();
                    string[] words = result.Split(' ');
                    for (int i = 0; i < words.Length; i++)
                    {
                        double temp_x, temp_y;
                        int width, height;
                        Point temp_point = new Point(0, 0);
                        if (words[i] == "rectangle" || words[i] == "ellipse")
                        {
                            temp_x = Convert.ToDouble(words[i + 1]);
                            temp_y = Convert.ToDouble(words[i + 2]);
                            width = Int32.Parse(words[i + 3]);
                            height = Int32.Parse(words[i + 4]);
                            temp_point = new Point(temp_x + (width / 2), temp_y + (height / 2));
                            if (words[i] == "rectangle")
                            {
                                BasicShape rectangle = new BasicShape(new AddRectangle());
                                rectangle.AddShapeToCanvas(mainCanvas, temp_point, width, height);
                            }
                            else if (words[i] == "ellipse")
                            {
                                BasicShape ellipse = new BasicShape(new AddEllipse());
                                ellipse.AddShapeToCanvas(mainCanvas, temp_point, width, height);
                            }
                        }
                    }
                }
                Console.WriteLine("Imported!");
            }
        }
    }

    class Export : ICommand
    {
        private Canvas mainCanvas;

        public Export(Canvas mainCanvas)
        {
            this.mainCanvas = mainCanvas;
        }

        public void Execute()
        {
            List<string> export_array = new List<string>();
            string temp_type = "";
            foreach (Shape shape in mainCanvas.Children.OfType<Shape>())
            {
                if (shape is System.Windows.Shapes.Ellipse)
                {
                    temp_type += "ellipse ";
                }
                else if (shape is System.Windows.Shapes.Rectangle)
                {
                    temp_type += "rectangle ";
                }
                temp_type += Canvas.GetLeft(shape) + " ";
                temp_type += Canvas.GetTop(shape) + " ";
                temp_type += shape.Width + " ";
                temp_type += shape.Height + " \r\n ";
            }
            export_array.Add(temp_type);

            Console.WriteLine("Exporting..");

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamWriter stream_writer = new StreamWriter(saveFileDialog.FileName))
                {
                    stream_writer.Write(export_array[0]);
                    stream_writer.Close();
                }
                Console.WriteLine("Exported!");
            }
        }
    }
}
