using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace DesignPatternsProject
{
    //https://en.wikipedia.org/wiki/Composite_pattern

    abstract class Component
    {
        public abstract void Add(Shape shape);
        public abstract void Remove(Shape shape);
        public abstract List<Shape> getChildren();
    }
}
