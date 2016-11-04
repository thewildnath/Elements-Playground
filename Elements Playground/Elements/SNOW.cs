using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class SNOW : Element {
        private static SNOW instance;

        public static SNOW Instance {
            get {
                if (instance == null) {
                    instance = new SNOW();
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

        private SNOW() {
            name = "SNOW";
            description = "Snow";
            category = CONST.CATEGORY_POWDER;
            visibleInMenu = true;
            color = Color.FromArgb(196, 233, 245);

            prop = CONST.TYPE_POWDER | CONST.PROP_MOVABLE | CONST.PROP_CONDUCTIVE;
            temperature = CONST.K_TO_C - 8;
            density = 68;
            stability = 95;
            fallingStability = 50;
            conductivity = 7;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.K_TO_C;
            highTemperatureTransition = typeof(WATER);
        }
    }
}
