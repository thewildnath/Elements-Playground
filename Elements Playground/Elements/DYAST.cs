using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class DYAST : Element {
        private static DYAST instance;

        public static DYAST Instance {
            get {
                if (instance == null) {
                    instance = new DYAST();
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

        private DYAST() {
            name = "DYAST";
            description = "Dead Yeast.";
            category = CONST.CATEGORY_POWDER;
            visibleInMenu = false;
            color = Color.FromArgb(196, 181, 169);
            prop = CONST.TYPE_POWDER | CONST.PROP_MOVABLE;
            temperature = CONST.ROOM_TEMP;
            density = 66;
            updateMoveIterations = 0;
            stability = 0;
            fallingStability = 98;
            conductivity = 0;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.K_TO_C + 200;
            highTemperatureTransition = typeof(DUST);

            Constructor = DYAST.constructor;
        }

        public static void constructor(Particle p, Simulation sim) {
            p.timer = 1;
        }
    }
}
