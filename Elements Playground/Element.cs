using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Elements_Playground {
    abstract class Element {
        public string name;
        public string description;
        public int category;
        public bool visibleInMenu;
        public Color color;

        public int prop;
        public double temperature;
        public int density; // 0-100
        //0-40 gases
        //41-60 liquids
        //61-80 powders
        //81-100 solids
        public int updateMoveIterations; // Only liquids and gases
        public int stability; // 0-100
        public int fallingStability; // 0-100
        public int conductivity; // 0-1
        //public int hardness;
        // -> should be in CONST as PROP_
        //public bool flammable;
        //public bool explosive;
        //public bool meltable;
        
        public double lowTemperature;
        public Type lowTemperatureTransition;
        public double highTemperature;
        public Type highTemperatureTransition;

        public delegate void ConstructorDelegate(Particle p, Simulation sim);
        public delegate void UpdateDelegate(Particle p, Simulation sim);
        public delegate Color GetColorDelegate(Particle p);
        public ConstructorDelegate Constructor = e_constructor;
        public UpdateDelegate Update = e_update;
        public GetColorDelegate GetColor = e_getColor;

        public static void e_constructor(Particle p, Simulation sim) { }
        public static void e_update(Particle p, Simulation sim) { }
        public static Color e_getColor(Particle p) {
            return p.color;
        }
    }
}
