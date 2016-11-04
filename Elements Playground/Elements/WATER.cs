using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class WATER : Element {
        private static WATER instance;

        public static WATER Instance {
            get {
                if (instance == null) {
                    instance = new WATER();
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

        private WATER() {
            name = "WATER";
            description = "Water";
            category = CONST.CATEGORY_LIQUID;
            visibleInMenu = true;
            color = Color.FromArgb(57, 153, 237);

            prop = CONST.TYPE_LIQUID | CONST.PROP_MOVABLE | CONST.PROP_CONDUCTIVE;
            temperature = CONST.ROOM_TEMP;
            density = 40;
            updateMoveIterations = 20;
            fallingStability = 90;
            conductivity = 6;

            lowTemperature = CONST.K_TO_C;
            lowTemperatureTransition = typeof(ICE);
            highTemperature = CONST.NO_HIGH_CHANGE;
            highTemperatureTransition = null;
        }
    }
}
