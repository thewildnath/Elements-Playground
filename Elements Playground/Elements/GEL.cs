using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class GEL : Element {
        private static GEL instance;

        public static GEL Instance {
            get {
                if (instance == null) {
                    instance = new GEL();
                }
                return instance;
            }
        }

        public static UIInformation GetUIInformation() {
            return new UIInformation(Instance.name,
                Instance.description,
                Instance.category,
                Instance.visibleInMenu,
                Instance.color);
        }

        private GEL() {
            name = "GEL";
            description = "Gel";
            category = CONST.CATEGORY_LIQUID;
            visibleInMenu = true;
            color = Color.LimeGreen;

            prop = CONST.TYPE_LIQUID | CONST.PROP_MOVABLE;
            temperature = CONST.ROOM_TEMP;
            density = 45;
            updateMoveIterations = 2;
            fallingStability = 90;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.NO_HIGH_CHANGE;
            highTemperatureTransition = null;
        }
    }
}
