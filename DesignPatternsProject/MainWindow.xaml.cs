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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isMouseLeftDown = false;
        private Canvas mainCanvas;
        private User user = new User();
        private List<Shape> selectedShapes = new List<Shape>();
        private Point originMousePosition, selectedElementPosition;

        public MainWindow()
        {
            user.commandStates.Add("");
            InitializeComponent();
        }

        private string saveState()
        {
            List<string> export_array = new List<string>();
            string temp_type = "";
            foreach (Shape shape in MainCanvas.Children.OfType<Shape>())
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
            return temp_type;
        }

        private void ExecuteCommand(ICommand command, bool undoable = false)
        {
            user.SetCommand(command, undoable);
            user.Action(user);

            if(!undoable)
            {
                user.currentStateIndex = user.commandStates.Count();
                user.commandStates.Add(saveState());
            }
        }

        public void mouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source is Canvas)
            {
                ICommand addShape = new AddShape(MainCanvas, ShapeSelector.Text,e);
                ExecuteCommand(addShape);
                clearShapeSelection();
            }
            else if (e.Source is Shape)
            {
                isMouseLeftDown = true;
                originMousePosition = e.GetPosition(mainCanvas);

                Shape currentShape = (Shape)e.Source;

                selectedElementPosition.X = Canvas.GetLeft(currentShape);
                selectedElementPosition.Y = Canvas.GetTop(currentShape);

                if (Keyboard.Modifiers != ModifierKeys.Control) clearShapeSelection();

                if(!selectedShapes.Contains(currentShape))
                {
                    selectedShapes.Add(currentShape);
                    currentShape.Fill = new SolidColorBrush(Colors.Blue);
                }

                ShapeResizePlus.Visibility = Visibility.Visible;
                ShapeResizeMinus.Visibility = Visibility.Visible;
            }
        }

        public void undo_mouseLeftDown(object sender, RoutedEventArgs e)
        { 
            ICommand undo = new UndoRedo(user, MainCanvas,"undo");
            ExecuteCommand(undo, undoable: true);
        }

        public void redo_mouseLeftDown(object sender, RoutedEventArgs e)
        {
            ICommand redo = new UndoRedo(user, MainCanvas,"redo");
            ExecuteCommand(redo, undoable: true);
        }

        public void group_mouseLeftDown(object sender, RoutedEventArgs e)
        {
            ICommand createGroup = new CreateGroup(selectedShapes);
            ExecuteCommand(createGroup);
        }

        public void disband_mouseLeftDown(object sender, RoutedEventArgs e)
        {
            ICommand disbandGroup = new DisbandGroup(selectedShapes);
            ExecuteCommand(disbandGroup);
        }

        public void export_mouseLeftDown(object sender, RoutedEventArgs e)
        {
            ICommand export = new Export(MainCanvas);
            ExecuteCommand(export);
        }

        public void import_mouseLeftDown(object sender, RoutedEventArgs e)
        {
            ICommand import = new Import(MainCanvas);
            ExecuteCommand(import);
        }

        public void plus_mouseLeftDown(object sender, RoutedEventArgs e)
        {
            ICommand resizePlus = new ResizePlus(selectedShapes);
            ExecuteCommand(resizePlus);
        }

        public void minus_mouseLeftDown(object sender, RoutedEventArgs e)
        {
            ICommand resizeMinus = new ResizeMinus(selectedShapes);
            ExecuteCommand(resizeMinus);
        }

        public void clearShapeSelection()
        {
            for (int i = 0; i < selectedShapes.Count; i++)
            {
                selectedShapes[i].Fill = new SolidColorBrush(Colors.Red);
            }
            ShapeResizePlus.Visibility = Visibility.Collapsed;
            ShapeResizeMinus.Visibility = Visibility.Collapsed;
            selectedShapes.Clear();
        }

        public void mouseLeftUp(object sender, MouseButtonEventArgs e)
        {
            isMouseLeftDown = false;
        }

        public void mouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.Source is Canvas) Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
            if (e.Source is Shape)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Hand;
                if (isMouseLeftDown)
                {
                    ICommand moveShapes = new MoveShapes(selectedElementPosition, originMousePosition, e, selectedShapes);
                    ExecuteCommand(moveShapes, undoable:true);
                }
                
            }
        }
    }
}
