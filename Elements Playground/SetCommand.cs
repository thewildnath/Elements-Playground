using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elements_Playground {
    class SetCommand : Command {
        public int x;
        public int y;
        public int brushSize;
        public int brushShape;
        public string brush;

        public SetCommand(int step, int x, int y, int brushSize, int brushShape, string brush) {
            this.step = step;
            this.x = x;
            this.y = y;
            this.brushSize = brushSize;
            this.brushShape = brushShape;
            this.brush = brush;
        }

        public override void Execute(Simulation sim) {
            sim.InputBrush(x, y, brushSize, brushShape, brush);
        }
    }
}
