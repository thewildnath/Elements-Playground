using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class ICE : Element {
        private static ICE instance;

        public static ICE Instance {
            get {
                if (instance == null) {
                    instance = new ICE();
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

        private ICE() {
            name = "ICE";
            description = "";
            category = CONST.CATEGORY_SOLID;
            visibleInMenu = true;
            color = Color.FromArgb(228, 238, 242);

            prop = CONST.TYPE_SOLID | CONST.PROP_CONDUCTIVE;
            temperature = CONST.K_TO_C - 20;
            density = 75;
            conductivity = 6;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.K_TO_C;
            highTemperatureTransition = typeof(WATER);
        }
    }
}
