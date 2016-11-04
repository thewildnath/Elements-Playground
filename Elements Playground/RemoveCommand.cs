using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Elements_Playground {
    class RemoveCommand : Command {
        public int x;
        public int y;
        public int brushSize;

        public RemoveCommand(int step, int x, int y, int brushSize) {
            this.step = step;
            this.x = x;
            this.y = y;
            this.brushSize = brushSize;
        }

        public override void Execute(Simulation sim) {
            MethodInfo method = typeof(Simulation).GetMethod("Remove");
            object[] parameters = new object[] { x, y, false };
            sim.ApplyBrush(x, y, brushSize, CONST.BRUSH_SHAPE_CIRCLE, method, parameters);
        }
    }
}
