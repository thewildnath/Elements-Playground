using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace Elements_Playground {
    class Renderer {
        public void Render(Graphics canvas, Simulation sim, int pixelSize, int width, int height, Point mousePosition, int brushSize, int brushShape) {
            Bitmap bmp = GetSimulationFrame(sim, pixelSize, width, height, mousePosition, brushSize, brushShape);

            canvas.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            canvas.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            canvas.DrawImage(bmp, 0, 0, width, height);
        }

        Bitmap GetSimulationFrame(Simulation sim, int pixelSize, int width, int height, Point mousePosition, int brushSize, int brushShape) {
            Bitmap bmp = new Bitmap(sim.width, sim.height);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = data.Stride;

            int mX = mousePosition.X / pixelSize;
            int mY = mousePosition.Y / pixelSize;
            double rmax = brushSize * brushSize;
            double rmin = brushSize == 0 ? -1 : (brushSize - 1) * (brushSize - 1);
            double rmin2 = brushSize == 0 ? -1 : (brushSize - 2) * (brushSize - 2);

            unsafe {
                byte* ptr = (byte*)data.Scan0;
                Parallel.For(0, sim.height, y => {
                    for (int x = 0; x < sim.width; ++x) {
                        if (sim.pmap[y + 1, x + 1] != null) {
                            Color color = sim.pmap[y + 1, x + 1].elem.GetColor(sim.pmap[y + 1, x + 1]);
                            ptr[(x * 3) + y * stride] = color.B;
                            ptr[(x * 3) + y * stride + 1] = color.G;
                            ptr[(x * 3) + y * stride + 2] = color.R;
                        } else {
                            ptr[(x * 3) + y * stride] = 255;
                            ptr[(x * 3) + y * stride + 1] = 255;
                            ptr[(x * 3) + y * stride + 2] = 255;
                        }
                    }
                });
                //*
                if (brushShape == CONST.BRUSH_SHAPE_CIRCLE) {
                    Parallel.For(mY - brushSize, mY + brushSize + 1, y => {
                        for (int x = mX - brushSize; x <= mX + brushSize; ++x) {
                            if (0 <= x && x < sim.width && 0 <= y && y < sim.height) {
                                double dist = Util.DistanceSquared(x, y, mX, mY);
                                //double dist = Util.Distance(x, y, mX, mY) - brushSize;
                                if (rmin < dist && dist <= rmax) {
                                    ptr[(x * 3) + y * stride] = 0;
                                    ptr[(x * 3) + y * stride + 1] = 0;
                                    ptr[(x * 3) + y * stride + 2] = 0;
                                }
                            }
                        }
                    });
                } else if (brushShape == CONST.BRUSH_SHAPE_SQUARE) {
                    for (int y = mY - brushSize; y <= mY + brushSize; ++y) {
                        int x = mX - brushSize;
                        if (0 <= x && x < sim.width && 0 <= y && y < sim.height) {
                            ptr[(x * 3) + y * stride] = 0;
                            ptr[(x * 3) + y * stride + 1] = 0;
                            ptr[(x * 3) + y * stride + 2] = 0;
                        }
                        x = mX + brushSize;
                        if (0 <= x && x < sim.width && 0 <= y && y < sim.height) {
                            ptr[(x * 3) + y * stride] = 0;
                            ptr[(x * 3) + y * stride + 1] = 0;
                            ptr[(x * 3) + y * stride + 2] = 0;
                        }
                    }
                    for (int x = mX - brushSize; x <= mX + brushSize; ++x) {
                        int y = mY - brushSize;
                        if (0 <= x && x < sim.width && 0 <= y && y < sim.height) {
                            ptr[(x * 3) + y * stride] = 0;
                            ptr[(x * 3) + y * stride + 1] = 0;
                            ptr[(x * 3) + y * stride + 2] = 0;
                        }
                        y = mY + brushSize;
                        if (0 <= x && x < sim.width && 0 <= y && y < sim.height) {
                            ptr[(x * 3) + y * stride] = 0;
                            ptr[(x * 3) + y * stride + 1] = 0;
                            ptr[(x * 3) + y * stride + 2] = 0;
                        }
                    }
                }
            }

            bmp.UnlockBits(data);
            return bmp;
        }

        unsafe void putpixel(byte* ptr, int stride, Simulation sim, int x, int y) {
            if (0 <= x && x < sim.width && 0 <= y && y < sim.height) {
                ptr[(x * 3) + y * stride] = 0;
                ptr[(x * 3) + y * stride + 1] = 0;
                ptr[(x * 3) + y * stride + 2] = 0;
            }
        }
    }
}
