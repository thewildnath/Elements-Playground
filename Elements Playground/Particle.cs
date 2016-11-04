using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground
{
    class Particle {
        public Element elem;
        public int x;
        public int y;

        public float vx;
        public float vy;

        public double temperature;
        public Color color;
        public int step;
        public int life;
        public int timer;

        public Particle(Element elem, int x, int y, Simulation sim) {
            this.elem = elem;
            this.x = x;
            this.y = y;
            this.vx = 0;
            this.vy = 0;
            this.temperature = elem.temperature;
            this.color = elem.color;
            this.step = 0;
            this.life = 
            this.timer = 0;

            elem.Constructor(this, sim);
        }

        public void ChangeElement(Element elem, Simulation sim) {
            this.elem = elem;
            this.color = elem.color;

            elem.Constructor(this, sim);
        }
    }
}
