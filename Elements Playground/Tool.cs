using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground {
    class Tool {
        public string name;
        public string description;
        public int category;
        public bool visibleInMenu;
        public Color color;

        //public virtual void Perform(int x, int y, int radius, Simulation sim) { }

        public virtual void Perform(int x, int y, Simulation sim) { }
    }
}
