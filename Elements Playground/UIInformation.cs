using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground {
    class UIInformation {
        public string name;
        public string description;
        public int category;
        public bool visibleInMenu;
        public Color color;
        public int brushShape;

        public UIInformation(string name, string description, int category, 
            bool visibleInMenu, Color color, int brushShape = CONST.BRUSH_SHAPE_CIRCLE) {
            this.name = name;
            this.description = description;
            this.category = category;
            this.visibleInMenu = visibleInMenu;
            this.color = color;
            this.brushShape = brushShape;
        }
    }
}
