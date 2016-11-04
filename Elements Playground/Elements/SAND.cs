using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class SAND : Element {
        private static SAND instance;

        public static SAND Instance {
            get {
                if (instance == null) {
                    instance = new SAND();
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

        private SAND() {
            name = "SAND";
            description = "Sand";
            category = CONST.CATEGORY_POWDER;
            visibleInMenu = true;
            color = Color.FromArgb(241, 163, 94);

            prop = CONST.TYPE_POWDER | CONST.PROP_MOVABLE;
            temperature = CONST.ROOM_TEMP;
            density = 70;
            stability = 0;
            fallingStability = 99;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.K_TO_C + 1723;
            highTemperatureTransition = typeof(GLASS);

            Constructor = SAND.constructor;
            GetColor = SAND.getColor;
        }

        public static void constructor(Particle p, Simulation sim) {
            int rnd = Util.GetRandom(100);
            if (rnd > 65) {
                if (rnd > 90)
                    p.color = Color.FromArgb(253, 180, 108);
                else
                    p.color = Color.FromArgb(228, 151, 85);
            }
        }

        public static Color getColor(Particle p) {
            if (p.temperature < CONST.K_TO_C + 1100)
                return p.color;
            else if (p.temperature > p.elem.highTemperature)
                return Color.FromArgb(255, 69, 0);

            double ratio = 1 - (p.temperature - CONST.K_TO_C - 1100) /
                (p.elem.highTemperature - CONST.K_TO_C - 1100);
            ratio = Util.Clamp(ratio, 0, 1);
            return Util.MixColors(p.color, Color.FromArgb(255, 69, 0), ratio);
        }
    }
}
