using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class LAVA : Decorator {
        private static LAVA instance;

        public static LAVA Instance {
            get {
                if (instance == null) {
                    instance = new LAVA();
                }
                return instance;
            }
        }

        public static LAVA DecoratorInstance(Element baseElem) {
            return new LAVA(baseElem);
        }

        public static UIInformation GetUIInformation() {
            return new UIInformation(Instance.name,
                Instance.description,
                Instance.category,
                Instance.visibleInMenu,
                Instance.color);
        }

        public LAVA() : this(STONE.Instance){
            name = "LAVA";
            temperature = CONST.K_TO_C + 1500;
        }

        public LAVA(Element baseElem) : base(baseElem) {
            name = "MOLTEN " + baseElem.name;
            category = CONST.CATEGORY_LIQUID;
            visibleInMenu = true;
            color = Color.FromArgb(255, 90, 20);

            prop = CONST.TYPE_LIQUID | CONST.PROP_MOVABLE;
            temperature = baseElem.highTemperature;
            density = 59;
            updateMoveIterations = 5;
            fallingStability = 99;

            lowTemperature = baseElem.highTemperature;
            lowTemperatureTransition = baseElem.GetType();
            highTemperature = CONST.NO_HIGH_CHANGE;
            highTemperatureTransition = null;

            GetColor = LAVA.getColor;
        }

        public static Color getColor(Particle p) {
            if (p.temperature < CONST.K_TO_C + 800)
                return Color.FromArgb(255, 90, 20);
            else if (p.temperature > CONST.K_TO_C + 2000)
                return Color.FromArgb(255, 212, 69);

            double ratio = 1 - (p.temperature - CONST.K_TO_C - 800) /
                (CONST.K_TO_C + 2000 - CONST.K_TO_C - 800);
            ratio = Util.Clamp(ratio, 0, 1);
            return Util.MixColors(Color.FromArgb(255, 90, 20), Color.FromArgb(255, 212, 69), ratio);
        }
    }
}