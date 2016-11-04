using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class STONE : Element {
        private static STONE instance;

        public static STONE Instance {
            get {
                if (instance == null) {
                    instance = new STONE();
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

        private STONE() {
            name = "STONE";
            description = "Stone";
            category = CONST.CATEGORY_POWDER;
            visibleInMenu = true;
            color = Color.FromArgb(186, 186, 186);

            prop = CONST.TYPE_POWDER | CONST.PROP_MOVABLE;
            temperature = CONST.ROOM_TEMP;
            density = 77;
            stability = 0;
            fallingStability = 100;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.K_TO_C + 800;
            highTemperatureTransition = typeof(LAVA);

            Constructor = STONE.constructor;
            GetColor = STONE.getColor;
        }

        public static void constructor(Particle p, Simulation sim) {
            int rnd = Util.GetRandom(100);
            if (rnd > 65) {
                if (rnd > 90)
                    p.color = Color.FromArgb(172, 172, 172);
                else
                    p.color = Color.FromArgb(197, 197, 197);
            }
        }

        public static Color getColor(Particle p) {
            if (p.temperature < CONST.K_TO_C + 500)
                return p.color;
            else if (p.temperature > p.elem.highTemperature)
                return Color.FromArgb(255, 69, 0);

            double ratio = 1 - (p.temperature - CONST.K_TO_C - 500) /
                (p.elem.highTemperature - CONST.K_TO_C - 500);
            ratio = Util.Clamp(ratio, 0, 1);
            return Util.MixColors(p.color, Color.FromArgb(255, 69, 0), ratio);
        }
    }
}
