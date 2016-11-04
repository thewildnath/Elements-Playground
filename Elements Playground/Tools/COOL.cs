using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground.Tools {
    class COOL : Tool {
        private static COOL instance;

        public static COOL Instance {
            get {
                if (instance == null) {
                    instance = new COOL();
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

        private COOL() {
            name = "COOL";
            description = "COOL.";
            category = CONST.CATEGORY_TOOL;
            visibleInMenu = true;
            color = Color.FromArgb(87, 188, 255);
        }
        /*
        public override void Perform(int x, int y, int radius, Simulation sim) {
            int rr = radius * radius;
            for (int ry = -radius; ry <= radius; ++ry) {
                for (int rx = -radius; rx <= radius; ++rx) {
                    if (sim.BoundsCheck(x + rx, y + ry) &&
                        sim.pmap[y + ry, x + rx] != null &&
                        Util.DistanceSquared(rx, ry, 0, 0) <= rr) {
                        sim.pmap[y + ry, x + rx].temperature -= 3;
                        sim.pmap[y + ry, x + rx].temperature =
                        Util.Clamp(sim.pmap[y + ry, x + rx].temperature, CONST.MIN_TEMP, CONST.MAX_TEMP);
                    }
                }
            }
        }
        */
        public override void Perform(int x, int y, Simulation sim) {
            if (sim.BoundsCheck(x, y) &&
                sim.pmap[y, x] != null) {
                sim.pmap[y, x].temperature -= 3;
                sim.pmap[y, x].temperature = Util.Clamp(sim.pmap[y, x].temperature, CONST.MIN_TEMP, CONST.MAX_TEMP);
            }
        }
    }
}
