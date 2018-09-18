using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Media;

namespace DesignPatternsProject
{
    public class AddEllipse : BasicShapeStrategy
    {
        public override Shape add()
        {
            return new System.Windows.Shapes.Ellipse();
        }
    }
}
