using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Elements_Playground.Elements;
using Elements_Playground.Tools;

namespace Elements_Playground {
    class Simulation {
        public int height;
        public int width;

        public Particle[,] pmap;

        double[,] heatmap;

        public int step;

        Random random;

        int edgeMode;

        public Simulation(System.Drawing.Size size, int seed) {
            height = size.Height;
            width = size.Width;
            pmap = new Particle[height + 2, width + 2];
            heatmap = new double[height + 2, width + 2];

            step = 0;
            random = new Random(seed);
            edgeMode = CONST.EDGE_MODE_SOLID;
        }

        void SetEdgeMode(int newEdgeMode) {
            edgeMode = newEdgeMode;
            ReinitializeEdges();
        }

        public void Update() {
            ++step;

            ReinitializeEdges();

            Parallel.For(1, height + 1, y => {
                for (int x = 0; x <= width + 1; ++x) {
                    if (pmap[y, x] != null)
                        heatmap[y, x] = pmap[y, x].temperature;
                    else
                        heatmap[y, x] = 0;
                }
            });
            // Pre-update
            for (int y = 1; y <= height; ++y) {
                for (int x = 1; x <= width; ++x) {
                    Particle p = pmap[y, x];
                    if (p != null)
                        UpdateHeat(p);
                    if (p != null && p.timer > 0)
                        --p.timer;
                }
            }

            for (int y = 1; y <= height; ++y) {
                for (int x = 1; x <= width; ++x) {
                    Particle p;
                    if (step % 2 == 0)
                        p = pmap[y, x];
                    else
                        p = pmap[y, width - x + 1];

                    if (p != null && p.step != step) {
                        p.step = step;

                        p.temperature = Util.Clamp(p.temperature, CONST.MIN_TEMP, CONST.MAX_TEMP);
                        if (p.temperature < p.elem.lowTemperature)
                            ChangeElement(p, p.elem.lowTemperatureTransition);
                        if (p.temperature > p.elem.highTemperature)
                            ChangeElement(p, p.elem.highTemperatureTransition);

                        if ((p.elem.prop & CONST.PROP_MOVABLE) != 0) // && !stable                      -!!!!!!!
                            UpdateMove(p);
                        p.elem.Update(p, this);
                    }
                }
            }
        }

        void UpdateHeat(Particle p) {
            double temperature = heatmap[p.y, p.x] * 0.10;
            Particle r;

            for (int i = 0; i < 8; ++i) {
                r = pmap[p.y + CONST.dy[i], p.x + CONST.dx[i]];
                if (r != null && r.elem.GetType() != typeof(WALL)) {
                    r.temperature += temperature;
                    pmap[p.y, p.x].temperature -= temperature;
                }
            }
        }

        void UpdateMove(Particle p) {
            if ((p.elem.prop & CONST.TYPE_POWDER) != 0)
                UpdateMovePowder(p);
            else if ((p.elem.prop & CONST.TYPE_LIQUID) != 0)
                UpdateMoveLiquid(p);
        }

        void UpdateMoveLiquid(Particle p) {
            int mask = GetMask(p.x, p.y, p.elem.density);
            if ((mask & CONST.MASK_S) != 0) {
                if (GetRandom(100) < p.elem.fallingStability)
                    FallDown(p.x, p.y);
                else {
                    if (!FallLeftOrRight(p.x, p.y, mask))
                        FallDown(p.x, p.y);
                }
            }
            else {
                int steps = GetRandom(p.elem.updateMoveIterations) + 1;
                int maskDown = GetMaskDown(p.x, p.y, p.elem.density);
                int maskSides;
                int dir;
                int dirMask;
                if ((mask & CONST.MASK_V) != 0 && (mask & CONST.MASK_E) != 0) {
                    dir = -1;
                    dirMask = CONST.MASK_V;
                    if (GetRandom(100) < 50) {
                        dir = 1;
                        dirMask = CONST.MASK_E;
                    }
                } else {
                    if ((mask & CONST.MASK_V) != 0) {
                        dir = -1;
                        dirMask = CONST.MASK_V;
                    }
                    else if ((mask & CONST.MASK_E) != 0) {
                        dir = 1;
                        dirMask = CONST.MASK_E;
                    }
                    else
                        return;
                }

                for (int i = 1; i <= steps && maskDown == 0; ++i) {
                    maskSides = GetMaskSides(p.x, p.y, p.elem.density);
                    if ((maskSides & dirMask) != 0)
                        Swap(p.x, p.y, p.x + dir, p.y);
                    if (p.x == 0 || p.x == width + 1)
                        return;
                    maskDown = GetMaskDown(p.x, p.y, p.elem.density);
                }

                FallLeftOrRight(p.x, p.y, maskDown);
            }
        }

        void UpdateMovePowder(Particle p) {
            int maskDown = GetMaskDown(p.x, p.y, p.elem.density);
            if ((maskDown & CONST.MASK_S) != 0) {
                if (GetRandom(100) < p.elem.fallingStability)
                    FallDown(p.x, p.y);
                else {
                    if (!FallLeftOrRight(p.x, p.y, maskDown))
                        FallDown(p.x, p.y);
                }
            } else if (maskDown != 0 && GetRandom(100) > p.elem.stability)
                FallLeftOrRight(p.x, p.y, maskDown);
        }

        void ReinitializeEdges() {
            if (edgeMode == CONST.EDGE_MODE_SOLID) {
                for (int x = 0; x <= width + 1; ++x) {
                    SetElement(x, 0, WALL.Instance);
                    SetElement(x, height + 1, WALL.Instance);
                }
                for (int y = 0; y <= height + 1; ++y) {
                    SetElement(0, y, WALL.Instance);
                    SetElement(width + 1, y, WALL.Instance);
                }
            } else if (edgeMode == CONST.EDGE_MODE_LOOP) {
                for (int x = 0; x <= width + 1; ++x) {
                    SetElement(x, 0, WALL.Instance);
                    SetElement(x, height + 1, WALL.Instance);
                }
                for (int y = 0; y <= height + 1; ++y) {
                    SetElement(0, y, WALL.Instance);
                    SetElement(width + 1, y, WALL.Instance);
                }
            } else if (edgeMode == CONST.EDGE_MODE_VOID) {
                for (int x = 0; x <= width + 1; ++x) {
                    Remove(x, 0);
                    Remove(x, height + 1);
                }
                for (int y = 0; y <= height + 1; ++y) {
                    Remove(0, y);
                    Remove(width + 1, y);
                }
            }
        }

        // 'Unsafe'
        // Fall functions make no checks 
        bool FallDown(int x, int y) {
            Swap(x, y, x, y + 1);
            return true;
        }

        bool FallLeftOrRight(int x, int y, int mask) {
            if ((mask & CONST.MASK_SE) != 0) {
                if ((mask & CONST.MASK_SV) != 0 &&
                    GetRandom(100) < 50)
                    Swap(x, y, x - 1, y + 1);
                else
                    Swap(x, y, x + 1, y + 1);
                return true;
            } else if ((mask & CONST.MASK_SV) != 0) {
                Swap(x, y, x - 1, y + 1);
                return true;
            } else
                return false;
        }

        bool MoveLeftOrRight(int x, int y, int mask) {
            if ((mask & CONST.MASK_V) != 0) {
                if ((mask & CONST.MASK_E) != 0 && 
                    GetRandom(100) < 50)
                    Swap(x, y, x + 1, y);
                else
                    Swap(x, y, x - 1, y);
                return true;
            } else if ((mask & CONST.MASK_E) != 0) {
                Swap(x, y, x + 1, y);
                return true;
            } else
                return false;
        }

        public int GetMask(int x, int y) {
            int mask = 0;
            for (int i = 0; i < 8; ++i) {
                if (pmap[y + CONST.dy[i], x + CONST.dx[i]] == null)
                    mask |= 1 << i;
            }
            return mask;
        }

        public int GetMask(int x, int y, int density) {
            int mask = 0;
            for (int i = 0; i < 8; ++i) {
                if (pmap[y + CONST.dy[i], x + CONST.dx[i]] == null ||
                    pmap[y + CONST.dy[i], x + CONST.dx[i]].elem.density < density)
                    mask |= 1 << i;
            }
            return mask;
        }

        public int GetMaskDown(int x, int y, int density) {
            int mask = 0;
            for (int i = 3; i <= 5; ++i) {
                if (pmap[y + CONST.dy[i], x + CONST.dx[i]] == null ||
                    pmap[y + CONST.dy[i], x + CONST.dx[i]].elem.density < density)
                    mask |= 1 << i;
            }
            return mask;
        }

        public int GetMaskSides(int x, int y, int density) {
            int mask = 0;
            if (pmap[y, x - 1] == null ||
                pmap[y, x - 1].elem.density < density)
                mask |= CONST.MASK_V;
            if (pmap[y, x + 1] == null ||
                pmap[y, x + 1].elem.density < density)
                mask |= CONST.MASK_E;
            return mask;
        }

        public void ChangeElement(Particle p, Type elemType) {
            RemoveDecorator(p.x, p.y);
            if (elemType.IsSubclassOf(typeof(Decorator)))
                SetDecorator(p.x, p.y, elemType);
            else
                p.ChangeElement(Util.GetElementInstance(elemType), this);
        }

        public void InputBrush(int x, int y, int radius, int shape, string brush, bool overwrite = false) {
            Type brushType = Util.GetType(brush);
            MethodInfo method = null;
            object[] parameters = null;

            if (brushType.IsSubclassOf(typeof(Decorator)) && brushType != typeof(LAVA)) { // Decorator, not LAVA
                method = this.GetType().GetMethod("SetDecorator");
                parameters = new object[] { 0, 0, brushType };
            } else if (brushType.IsSubclassOf(typeof(Element))) { // Element
                method = this.GetType().GetMethod("SetElement");
                parameters = new object[] { 0, 0, Util.GetElementInstance(brush), overwrite };
            } else if (brushType.IsSubclassOf(typeof(Tool))) { // Tool
                method = this.GetType().GetMethod("PerformTool");
                parameters = new object[] { 0, 0, Util.GetToolInstance(brushType) };
            }

            ApplyBrush(x, y, radius, shape, method, parameters);
        }

        public void ApplyBrush(int x, int y, int radius, int shape, MethodInfo method, object[] parameters) {
            int rr = radius * radius;
            for (int ry = -radius; ry <= radius; ++ry) {
                for (int rx = -radius; rx <= radius; ++rx) {
                    if (shape == CONST.BRUSH_SHAPE_SQUARE || 
                        Util.DistanceSquared(rx, ry, 0, 0) <= rr) {
                        parameters[0] = x + rx;
                        parameters[1] = y + ry;
                        method.Invoke(this, parameters);
                    }
                }
            }
        }

        public void SetElement(int x, int y, Element elem, bool overwrite = false) {
            if (BoundsCheck(x, y) &&
                (pmap[y, x] == null || (overwrite == true && (pmap[y, x].elem.prop & CONST.TYPE_WALL) == 0))) {
                pmap[y, x] = new Particle(elem, x, y, this);
            }
        }

        public void Remove(int x, int y, bool overwriteWalls = false) {
            if (BoundsCheck(x, y) && pmap[y, x] != null &&
                ((pmap[y, x].elem.prop & CONST.TYPE_WALL) == 0 || overwriteWalls == true)) {
                pmap[y, x] = null;
            }
        }

        public void SetDecorator(int x, int y, Type decoratorType) {
            if (BoundsCheck(x, y) &&
                pmap[y, x] != null && (pmap[y, x].elem.prop & CONST.TYPE_WALL) == 0) {
                if (decoratorType == typeof(SPARK)) {
                    if ((pmap[y, x].elem.prop & CONST.PROP_CONDUCTIVE) != 0 && pmap[y, x].timer == 0) {
                        Decorator decorator = Util.GetDecoratorInstance(decoratorType, pmap[y, x].elem);
                        pmap[y, x].ChangeElement(decorator, this);
                    }
                } else {
                    Decorator decorator = Util.GetDecoratorInstance(decoratorType, pmap[y, x].elem);
                    pmap[y, x].ChangeElement(decorator, this);
                }
            }
        }

        public void RemoveDecorator(int x, int y) {
            if (BoundsCheck(x, y) && pmap[y, x] != null) {
                while (pmap[y, x].elem is Decorator) {
                    Element elem = ((Decorator)pmap[y, x].elem).baseElem;
                    pmap[y, x].ChangeElement(elem, this);
                }
            }
        }

        public void PerformTool(int x, int y, Tool tool) {
            if (BoundsCheck(x, y))
                tool.Perform(x, y, this);
        }

        public string GetInfo(int x, int y) {
            string text = "";
            if (BoundsCheck(x, y)) {
                if (pmap[y, x] == null)
                    text = "AIR";
                else {
                    text = pmap[y, x].elem.name;
                    if ((pmap[y, x].elem.prop & CONST.TYPE_WALL) == 0)
                        text += " " + Math.Round(pmap[y, x].temperature - CONST.K_TO_C, 2).ToString() + " C";
                }
            }
            return text;
        }

        public void Swap(int ax, int ay, int bx, int by) {
            Particle c = pmap[by, bx];
            MoveTo(pmap[ay, ax], bx, by);
            MoveTo(c, ax, ay);
        }

        void MoveTo(Particle p, int x, int y) {
            if (p != null) {
                p.x = x;
                p.y = y;
                pmap[y, x] = p;
            } else
                pmap[y, x] = null;
        }

        public int GetRandom(int max = 1 << 31) {
            return random.Next(max);
        }
        
        public bool BoundsCheck(int x, int y) {
            return 0 <= x && x <= width + 1 &&
                0 <= y && y <= height + 1;
        }
    }
}

/*
        public void Set(int x, int y, int radius, int shape, string brush, bool overwrite = false) {
            Type brushType = Util.GetType(brush);

            if (brushType == typeof(LAVA)) {
                SetElement(x, y, radius, shape, Util.GetElementInstance(brush), overwrite);
            } else {
                if (brushType.IsSubclassOf(typeof(Decorator)))
                    SetDecorator(x, y, radius, shape, brushType);
                else if (brushType.IsSubclassOf(typeof(Element)))
                    SetElement(x, y, radius, shape, Util.GetElementInstance(brush), overwrite);
                else if (brushType.IsSubclassOf(typeof(Tool)))
                    PerformTool(x, y, radius, Util.GetToolInstance(brush));
            }
        }

        public void SetElement(int x, int y, int radius, int shape, Element elem, bool overwrite = false) {
            int rr = radius * radius;
            for (int ry = -radius; ry <= radius; ++ry)
                for (int rx = -radius; rx <= radius; ++rx)
                    if (shape == CONST.BRUSH_STYLE_SQUARE || Util.DistanceSquared(rx, ry, 0, 0) <= rr)
                        SetElement(x + rx, y + ry, elem, overwrite);
        }

        public bool SetElement(int x, int y, Element elem, bool overwrite = false) {
            if (BoundsCheck(x, y) &&
                (pmap[y, x] == null || (overwrite == true && (pmap[y, x].elem.prop & CONST.TYPE_WALL) == 0))) {
                pmap[y, x] = new Particle(elem, x, y, this);
                return true;
            } else
                return false;
        }

        public void SetDecorator(int x, int y, int radius, int shape, Type decoratorType) {
            int rr = radius * radius;
            for (int ry = -radius; ry <= radius; ++ry)
                for (int rx = -radius; rx <= radius; ++rx)
                    if (shape == CONST.BRUSH_STYLE_SQUARE || Util.DistanceSquared(rx, ry, 0, 0) <= rr)
                        SetDecorator(x + rx, y + ry, decoratorType);
        }

        public bool SetDecorator(int x, int y, Type decoratorType) {
            if (BoundsCheck(x, y) && 
                (pmap[y, x] != null && (pmap[y, x].elem.prop & CONST.TYPE_WALL) == 0) &&
                !(pmap[y, x].elem.GetType()).IsSubclassOf(typeof(Decorator)) && pmap[y, x].timer == 0) {
                if (decoratorType == typeof(SPARK) &&
                    (pmap[y, x].elem.prop & CONST.PROP_CONDUCTIVE) == 0)
                    return false;
                else {
                    Decorator decorator = Util.GetDecoratorInstance(decoratorType, pmap[y, x].elem);
                    pmap[y, x].ChangeElement(decorator, this);
                    return true;
                }
            } else
                return false;
        }

        public void RemoveDecorator(int x, int y) {
            if (BoundsCheck(x, y) && pmap[y, x] != null) {
                while (pmap[y, x].elem is Decorator) {
                    Element elem = ((Decorator)pmap[y, x].elem).baseElem;
                    pmap[y, x].ChangeElement(elem, this);
                }
            }
        }

        public void Remove(int x, int y, int radius, int shape, bool overwriteWalls = false) {
            int rr = radius * radius;
            for (int ry = -radius; ry <= radius; ++ry)
                for (int rx = -radius; rx <= radius; ++rx)
                    if (shape == CONST.BRUSH_STYLE_SQUARE || Util.DistanceSquared(rx, ry, 0, 0) <= rr)
                        Remove(x + rx, y + ry);
        }

        public bool Remove(int x, int y, bool overwriteWalls = false) {
            if (BoundsCheck(x, y) && pmap[y, x] != null &&
                ((pmap[y, x].elem.prop & CONST.TYPE_WALL) == 0 || overwriteWalls == true)) {
                pmap[y, x] = null;
                return true;
            }
            else
                return false;
        }

        public void PerformTool(int x, int y, int radius, Tool tool) {
            tool.Perform(x, y, radius, this);
        }
        */
