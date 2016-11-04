using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elements_Playground {
    static class CONST {
        public static readonly Tuple<int, string>[] categories = new Tuple<int, string>[7] {
            new Tuple<int, string>(CATEGORY_WALL, "WALLS"),
            new Tuple<int, string>(CATEGORY_ELECTRONICS, "ELECTRONICS"),
            new Tuple<int, string>(CATEGORY_POWDER, "POWDERS"),
            new Tuple<int, string>(CATEGORY_LIQUID, "LIQUIDS"),
            new Tuple<int, string>(CATEGORY_SOLID, "SOLIDS"),
            new Tuple<int, string>(CATEGORY_GAS, "GASES"),
            new Tuple<int, string>(CATEGORY_TOOL, "TOOLS")
        };

        public const int CATEGORY_SOLID  = 1;
        public const int CATEGORY_LIQUID = 2;
        public const int CATEGORY_GAS    = 3;
        public const int CATEGORY_POWDER = 4;
        public const int CATEGORY_WALL   = 5;
        public const int CATEGORY_TOOL = 6;
        public const int CATEGORY_ELECTRONICS = 7;

        public const int TYPE_WALL = 1 << 0;
        public const int TYPE_SOLID  = 1 << 1;
        public const int TYPE_LIQUID = 1 << 2;
        public const int TYPE_GAS     = 1 << 3;
        public const int TYPE_POWDER  = 1 << 4;
        public const int TYPE_ENERGY = 1 << 5;
        public const int PROP_MOVABLE = 1 << 6;
        public const int PROP_CONDUCTIVE = 1 << 7;

        public const int AIR_DENSITY = 20;

        public const double K_TO_C = 273.15;
        public const double ROOM_TEMP = K_TO_C + 23;
        public const double MIN_TEMP = 0;
        public const double MAX_TEMP = K_TO_C + 10000;
        public const double NO_LOW_CHANGE = MIN_TEMP - 1;
        public const double NO_HIGH_CHANGE = MAX_TEMP + 1;
        
        public const int MASK_N  = 1 << 0;
        public const int MASK_NE = 1 << 1;
        public const int MASK_E  = 1 << 2;
        public const int MASK_SE = 1 << 3;
        public const int MASK_S  = 1 << 4;
        public const int MASK_SV = 1 << 5;
        public const int MASK_V  = 1 << 6;
        public const int MASK_NV = 1 << 7;

        //                    N, NE, E, SE, S, SV, V, NV
        public static readonly int[] dx = new int[8] { 0, 1, 1, 1, 0, -1, -1, -1 };
        public static readonly int[] dy = new int[8] { -1, -1, 0, 1, 1, 1, 0, -1 };

        public const int BRUSH_SHAPE_CIRCLE = 0;
        public const int BRUSH_SHAPE_SQUARE = 1;

        public const int PLAY_MODE_RECORD = 0;
        public const int PLAY_MODE_REPLAY = 1;

        public const int EDGE_MODE_VOID  = 0;
        public const int EDGE_MODE_SOLID = 1;
        public const int EDGE_MODE_LOOP = 2;
    }
}
