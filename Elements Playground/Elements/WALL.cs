using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class WALL : Element {
        private static WALL instance;

        public static WALL Instance {
            get {
                if (instance == null) {
                    instance = new WALL();
                }
                return instance;
            }
        }

        public static UIInformation GetUIInformation() {
            return new UIInformation(Instance.name,
                Instance.description,
                Instance.category,
                Instance.visibleInMenu,
                Instance.color,
                CONST.BRUSH_SHAPE_SQUARE);
        }

        private WALL() {
            name = "WALL";
            description = "Blocks everything.";
            category = CONST.CATEGORY_WALL;
            visibleInMenu = true;
            color = Color.FromArgb(60, 60, 60);

            prop = CONST.TYPE_WALL;
            density = 101;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.NO_HIGH_CHANGE;
            highTemperatureTransition = null;
        }
    }
}
