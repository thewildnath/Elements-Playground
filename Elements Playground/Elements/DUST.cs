using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class DUST : Element {
        private static DUST instance;

        public static DUST Instance {
            get {
                if (instance == null) {
                    instance = new DUST();
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

        private DUST() {
            name = "DUST";
            description = "Dust.";
            category = CONST.CATEGORY_POWDER;
            visibleInMenu = true;
            color = Color.FromArgb(255, 224, 160);

            prop = CONST.TYPE_POWDER | CONST.PROP_MOVABLE;
            temperature = CONST.ROOM_TEMP;
            density = 63;
            updateMoveIterations = 0;
            stability = 0;
            fallingStability = 60;
            conductivity = 0;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.NO_HIGH_CHANGE;
            highTemperatureTransition = null;
        }
    }
}
