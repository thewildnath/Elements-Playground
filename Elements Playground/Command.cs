using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elements_Playground {
    abstract class Command {
        public int step;

        public abstract void Execute(Simulation sim);
    }
}
