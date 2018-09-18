using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace DesignPatternsProject
{
    //https://en.wikipedia.org/wiki/Composite_pattern

    class Composite : Component
    {
        private readonly List<Shape> _children;

        public Composite()
        {
            _children = new List<Shape>();
        }

        public override void Add(Shape c)
        {
            _children.Add(c);
        }

        public override void Remove(Shape c)
        {
            _children.Remove(c);
        }

        public override List<Shape> getChildren()
        {
            return _children;
        }
    }
}
