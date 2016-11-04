using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Elements_Playground {
    static class Util {

        static Random random = new Random();

        public static int GetRandom(int max = 1 << 31) {
            return random.Next(max);
        }

        public static int GetRandom(int min = 0, int max = 1 << 31) {
            return random.Next(min, max);
        }

        public static int DistanceSquared(int ax, int ay, int bx, int by) {
            return (ax - bx) * (ax - bx) + (ay - by) * (ay - by);
        }

        public static double Distance(int ax, int ay, int bx, int by) {
            return Math.Sqrt((ax - bx) * (ax - bx) + (ay - by) * (ay - by));
        }

        public static int Clamp(int value, int min, int max) {
            if (value < min)
                value = min;
            if (value > max)
                value = max;
            return value;
        }

        public static double Clamp(double value, double min, double max) {
            if (value < min)
                value = min;
            if (value > max)
                value = max;
            return value;
        }

        public static Color MixColors(Color c1, Color c2, double ratio = 0.5) {
            return Color.FromArgb(
                (int)(c1.R * ratio + c2.R * (1 - ratio)),
                (int)(c1.G * ratio + c2.G * (1 - ratio)),
                (int)(c1.B * ratio + c2.B * (1 - ratio)));
        }

        public static Type GetType(string name) {
            Type type;
            if ((type = Type.GetType("Elements_Playground.Elements." + name)) == null)
                type = Type.GetType("Elements_Playground.Tools." + name);
            return type;
        }

        public static UIInformation GetUIInformation(string name) {
            Type type = GetType(name);
            MethodInfo uiinformation = type.GetMethod("GetUIInformation",
                BindingFlags.Public | BindingFlags.Static);
            return uiinformation.Invoke(null, null) as UIInformation;
        }

        public static Element GetElementInstance(Type elemType) {
            PropertyInfo instance = elemType.GetProperty("Instance",
                BindingFlags.Public | BindingFlags.Static);
            return instance.GetValue(null, null) as Element;
        }

        public static Element GetElementInstance(string name) {
            return GetElementInstance(GetType(name));
        }

        public static Decorator GetDecoratorInstance(Type decoratorType, Element baseElem) {
            MethodInfo instance = decoratorType.GetMethod("DecoratorInstance",
                BindingFlags.Public | BindingFlags.Static);
            object[] parameters = new object[] { baseElem };
            return instance.Invoke(null, parameters) as Decorator;
        }

        public static Element GetDecoratorInstance(string name, Element baseElem) {
            return GetDecoratorInstance(GetType(name), baseElem);
        }

        public static Tool GetToolInstance(Type toolType) {
            PropertyInfo instance = toolType.GetProperty("Instance",
                BindingFlags.Public | BindingFlags.Static);
            return instance.GetValue(null, null) as Tool;
        }

        public static Tool GetToolInstance(string name) {
            return GetToolInstance(GetType(name));
        }
    }
}
