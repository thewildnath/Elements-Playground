using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Elements {
    class VIRUS : Element {
        private static VIRUS instance;

        public static VIRUS Instance {
            get {
                if (instance == null) {
                    instance = new VIRUS();
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

        private VIRUS() {
            name = "VIRUS";
            description = "Transform everything is touches into a virus.";
            category = CONST.CATEGORY_LIQUID;
            visibleInMenu = true;
            color = Color.FromArgb(255, 65, 250);

            prop = CONST.TYPE_LIQUID | CONST.PROP_MOVABLE;
            temperature = CONST.ROOM_TEMP;
            density = 44;
            updateMoveIterations = 9;
            fallingStability = 90;

            lowTemperature = CONST.NO_LOW_CHANGE;
            lowTemperatureTransition = null;
            highTemperature = CONST.NO_HIGH_CHANGE;
            highTemperatureTransition = null;

            Constructor = VIRUS.constructor;
            Update = VIRUS.update;
        }

        public static void constructor(Particle p, Simulation sim) {
            p.timer = 1;
        }

        public static void update(Particle p, Simulation sim) {
            if (p.timer == 0) {
                for (int i = 0; i < 8; ++i) {
                    int nx = p.x + CONST.dx[i];
                    int ny = p.y + CONST.dy[i];
                    if (sim.pmap[ny, nx] != null &&
                        sim.pmap[ny, nx].elem != VIRUS.Instance && sim.GetRandom(5) == 0)
                        sim.SetElement(nx, ny, VIRUS.Instance, true);
                }
            }
        }
    }
}
