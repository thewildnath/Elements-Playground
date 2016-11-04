using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class GLASS : Element {
        private static GLASS instance;

        public static GLASS Instance {
            get {
                if (instance == null) {
                    instance = new GLASS();
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

        private GLASS() {
            name = "GLASS";
            description = "";
            category = CONST.CATEGORY_SOLID;
            visibleInMenu = true;
            color = Color.FromArgb(189, 198, 198);

            prop = CONST.TYPE_SOLID;
            temperature = CONST.ROOM_TEMP;
            density = 80;
            conductivity = 3;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.K_TO_C + 1723;
            highTemperatureTransition = typeof(LAVA);

            GetColor = GLASS.getColor;
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
