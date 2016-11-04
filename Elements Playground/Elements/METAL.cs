using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class METAL : Element {
        private static METAL instance;

        public static METAL Instance {
            get {
                if (instance == null) {
                    instance = new METAL();
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

        private METAL() {
            name = "METAL";
            description = "";
            category = CONST.CATEGORY_ELECTRONICS;
            visibleInMenu = true;
            color = Color.FromArgb(110, 110, 110);

            prop = CONST.TYPE_SOLID | CONST.PROP_CONDUCTIVE;
            temperature = CONST.ROOM_TEMP;
            density = 80;
            conductivity = 3;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.K_TO_C + 1200;
            highTemperatureTransition = typeof(LAVA);

            GetColor = METAL.getColor;
        }

        public static Color getColor(Particle p) {
            if (p.temperature < CONST.K_TO_C + 700)
                return p.color;
            else if (p.temperature > p.elem.highTemperature)
                return Color.FromArgb(255, 69, 0);

            double ratio = 1 - (p.temperature - CONST.K_TO_C - 700) / 
                (p.elem.highTemperature - CONST.K_TO_C - 700);
            ratio = Util.Clamp(ratio, 0, 1);
            return Util.MixColors(p.color, Color.FromArgb(255, 69, 0), ratio);
        }
    }
}
