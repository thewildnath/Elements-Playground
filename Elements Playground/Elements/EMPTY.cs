using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class EMPTY : Element {
        private static EMPTY instance;

        public static EMPTY Instance {
            get {
                if (instance == null) {
                    instance = new EMPTY();
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

        private EMPTY() {
            name = "EMPTY";
            description = "Empty element.";
            category = 0;
            visibleInMenu = false;
            color = Color.FromArgb(255, 255, 255);

            prop = 0;
            temperature = CONST.ROOM_TEMP;
            density = 0;
            updateMoveIterations = 0;
            stability = 0;
            fallingStability = 0;
            conductivity = 0;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.NO_HIGH_CHANGE;
            highTemperatureTransition = null;
        }
    }
}
