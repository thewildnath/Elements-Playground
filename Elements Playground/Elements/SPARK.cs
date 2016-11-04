using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class SPARK : Decorator {
        private static SPARK instance;

        public static SPARK Instance {
            get {
                if (instance == null) {
                    instance = new SPARK();
                }
                return instance;
            }
        }

        public static SPARK DecoratorInstance(Element baseElem) {
            return new SPARK(baseElem);
        }

        public static UIInformation GetUIInformation() {
            return new UIInformation(Instance.name,
                Instance.description,
                Instance.category,
                Instance.visibleInMenu,
                Instance.color);
        }

        public int propagationSpeed;
        public int currentWidth;
        public int interCurrentWidth;
        public int sparkTimer;

        public SPARK() : this(METAL.Instance){
            name = "SPARK";
        }

        public SPARK(Element baseElem) : base(baseElem) {
            name = "SPARK " + baseElem.name;
            category = CONST.CATEGORY_ELECTRONICS;
            visibleInMenu = true;
            color = Color.FromArgb(255, 255, 65);

            currentWidth = 2 * conductivity;
            interCurrentWidth = conductivity;
            propagationSpeed = currentWidth + conductivity;
            sparkTimer = propagationSpeed;

            Constructor = SPARK.constructor;
            Update = SPARK.update;
        }

        public static void constructor(Particle p, Simulation sim) {
            SPARK e = (p.elem as SPARK);
            p.color = Util.MixColors(e.baseElem.color, Color.FromArgb(255, 255, 65), 0.2);
            p.step = sim.step;
        }

        public static void update(Particle p, Simulation sim) {
            SPARK e = (p.elem as SPARK);
            e.baseElem.Update(p, sim);

            if (e.sparkTimer == 0) {
                sim.RemoveDecorator(p.x, p.y);
                p.timer = e.conductivity +
                    e.currentWidth + e.interCurrentWidth;
            } else {
                p.temperature += 0.5;

                if (e.sparkTimer == e.currentWidth) {
                    for (int y = -2; y <= 2; ++y) {
                        for (int x = -2; x <= 2; ++x) {
                            int nx = p.x + x;
                            int ny = p.y + y;
                            if (sim.BoundsCheck(nx, ny) && Math.Abs(x) + Math.Abs(y) != 4) {
                                if (sim.pmap[ny, nx] != null &&
                                (sim.pmap[ny, nx].elem.prop & CONST.PROP_CONDUCTIVE) != 0)
                                    sim.SetDecorator(nx, ny, typeof(SPARK));
                            }
                        }
                    }
                }

                --e.sparkTimer;
            }
        }
    }
}