using Elements_Playground.Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Elements_Playground {
    public partial class GameForm : Form {
        Simulation sim;
        Renderer renderer;
        TabControl tabControl;

        Queue<long> frameTicks;
        int targetFPS;
        long targetFrameInterval;
        long lastUpdate;

        List<Command> commands;
        int currentCommand;

        List<string> brushes;

        int simHeight;
        int simWidth;
        int windowHeight;
        int windowWidth;
        int pixelSize;
        int buttonHeight;
        int buttonWidth;
        int buttonMargin;
        int randomSeed;

        string curBrush;
        int brushSize;
        int brushShape;
        UIInformation curBrushUI;

        bool paused = false;
        int playMode = CONST.PLAY_MODE_RECORD;

        Point mousePosition;
        bool isLeftButtonDown;
        bool isRightButtonDown;
        
        public GameForm() {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;

            renderer = new Renderer();
            targetFPS = 60;
            targetFrameInterval = 10000000 / targetFPS;
            lastUpdate = DateTime.Now.Ticks;
            frameTicks = new Queue<long>();

            simHeight = 110;
            simWidth = 180;
            pixelSize = 5;

            InitializeSimulation();
            GetBrushes();
            BuildUI();

            mousePosition = this.PointToClient(Cursor.Position);

            brushSize = 7;
            curBrush = "SAND";
            curBrushUI = Util.GetUIInformation(curBrush);
            
            tabControl.SelectedIndex = 2;

            gameTimer = new Timer();
            gameTimer.Interval = (int)((double)1000 / targetFPS / 1.1);
            gameTimer.Tick += Update;
            gameTimer.Start();
        }

        void Update(Object sender, EventArgs e) {
            brushShape = curBrushUI.brushShape;
            if (playMode == CONST.PLAY_MODE_RECORD) {
                if (isLeftButtonDown) {
                    commands.Add(new SetCommand(sim.step, mousePosition.X / pixelSize + 1, mousePosition.Y / pixelSize + 1, brushSize, brushShape, curBrush));
                } else if (isRightButtonDown) {
                    brushShape = CONST.BRUSH_SHAPE_CIRCLE;
                    commands.Add(new RemoveCommand(sim.step, mousePosition.X / pixelSize + 1, mousePosition.Y / pixelSize + 1, brushSize));
                }
            }

            long now = DateTime.Now.Ticks;
            if (now - lastUpdate >= 5 * targetFrameInterval) {
                lastUpdate = now - targetFrameInterval;
            }
            while (now - lastUpdate >= targetFrameInterval) {
                frameTicks.Enqueue(now);
                while (currentCommand < commands.Count && commands[currentCommand].step == sim.step) {
                    commands[currentCommand].Execute(sim);
                    ++currentCommand;
                }
                if (playMode == CONST.PLAY_MODE_REPLAY && currentCommand == commands.Count)
                    SetPlayMode(CONST.PLAY_MODE_RECORD);
                if (!paused) 
                    sim.Update();
                lastUpdate += targetFrameInterval;
            }
            while (frameTicks.Count > 0 && now - frameTicks.Peek() > 10000000)
                frameTicks.Dequeue();
            labelFPS.Text = frameTicks.Count.ToString();

            labelInfo.Text = sim.GetInfo(mousePosition.X / pixelSize + 1, mousePosition.Y / pixelSize + 1);

            this.Invalidate();
        }

        void SetPlayMode(int newPlayMode) {
            if (newPlayMode == CONST.PLAY_MODE_REPLAY) {
                if (playMode == CONST.PLAY_MODE_RECORD) {
                    playMode = CONST.PLAY_MODE_REPLAY;
                    currentCommand = 0;
                    sim = new Simulation(new Size(simWidth, simHeight), randomSeed);
                }
            } else if (newPlayMode == CONST.PLAY_MODE_RECORD) {
                if (playMode == CONST.PLAY_MODE_REPLAY) {
                    playMode = CONST.PLAY_MODE_RECORD;
                    commands.RemoveRange(currentCommand, commands.Count - currentCommand);
                }
            }
        }

        void InitializeSimulation() {
            randomSeed = (int)(DateTime.Now.Ticks % 1000000);
            sim = new Simulation(new Size(simWidth, simHeight), randomSeed);
            commands = new List<Command>();
            currentCommand = 0;
        }

        void GetBrushes() {
            brushes = new List<string>();
            string @namespace = "Elements_Playground.Elements";
            var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == @namespace
                    select t;
            q.ToList().ForEach(t => brushes.Add(t.Name));
            @namespace = "Elements_Playground.Tools";
            q = from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == @namespace
                    select t;
            q.ToList().ForEach(t => brushes.Add(t.Name));
        }

        void BuildUI() {
            buttonHeight = 27;
            buttonWidth = 60;
            buttonMargin = 10;
            windowWidth = simWidth * pixelSize;

            // Make TabControl
            tabControl = new TabControl();
            this.Controls.Add(tabControl);
            tabControl.Location = new Point(0, simHeight * pixelSize);
            tabControl.ItemSize = new Size(tabControl.ItemSize.Width, 20);
            tabControl.Size = new Size(windowWidth, tabControl.ItemSize.Height + 8 + buttonHeight + 2 * buttonMargin);

            for (int i = 0; i < CONST.categories.Length; ++i) {
                TabPage page = new TabPage();
                tabControl.TabPages.Add(page);
                page.Text = CONST.categories[i].Item2;
                BuildButtons(page, CONST.categories[i].Item1);
            }

            windowHeight = simHeight * pixelSize + tabControl.Height;
            this.ClientSize = new Size(windowWidth, windowHeight);
        }

        void BuildButtons(Control obj, int category) {
            int left = buttonMargin;

            foreach (string name in brushes) {
                UIInformation uiinformation = Util.GetUIInformation(name);
                if (uiinformation.category == category &&
                    uiinformation.visibleInMenu == true) {
                    Button button = new Button();
                    button.UseCompatibleTextRendering = true;
                    button.BackColor = uiinformation.color;
                    button.Click += new EventHandler(ElementClickEvent);
                    if (uiinformation.color.GetBrightness() <= 0.5)
                        button.ForeColor = Color.White;
                    else
                        button.ForeColor = Color.Black;
                    button.Location = new Point(left, obj.Location.Y + buttonMargin);
                    button.Size = new Size(buttonWidth, buttonHeight);
                    button.FlatAppearance.BorderSize = 0;
                    button.FlatStyle = FlatStyle.Flat;
                    button.Text = uiinformation.name;
                    button.TextAlign = ContentAlignment.MiddleCenter;
                    obj.Controls.Add(button);
                    left += buttonMargin + buttonWidth;
                }
            }
        }
    
        void ElementClickEvent(object sender, EventArgs e) {
            Button button = sender as Button;
            curBrush = button.Text;
            curBrushUI = Util.GetUIInformation(curBrush);
        }

        private void GameForm_Paint(object sender, PaintEventArgs e) {
            renderer.Render(e.Graphics, sim, pixelSize, simWidth * pixelSize, simHeight * pixelSize, mousePosition, brushSize, brushShape);
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e) {
            switch (e.KeyCode) {
                case Keys.Space:
                    paused = !paused;
                    break;
                case Keys.Q:
                    SetPlayMode(CONST.PLAY_MODE_REPLAY);
                    break;
                case Keys.R:
                    InitializeSimulation();
                    break;
            }
        }

        private void GameForm_MouseDown(object sender, MouseEventArgs e) {
            Point mouse = this.PointToClient(Cursor.Position);

            if (e.Button == MouseButtons.Left) {
                isLeftButtonDown = true;
                if (playMode == CONST.PLAY_MODE_REPLAY)
                    SetPlayMode(CONST.PLAY_MODE_RECORD);
            } else if (e.Button == MouseButtons.Right) {
                isRightButtonDown = true;
                if (playMode == CONST.PLAY_MODE_REPLAY)
                    SetPlayMode(CONST.PLAY_MODE_RECORD);
            }
        }

        private void GameForm_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                isLeftButtonDown = false;
            else if (e.Button == MouseButtons.Right)
                isRightButtonDown = false;
        }

        private void GameForm_MouseWheel(object sender, MouseEventArgs e) {
            if (e.Delta < 0)
                --brushSize;
            else
                ++brushSize;
            brushSize = Util.Clamp(brushSize, 0, 50);
        }

        private void GameForm_MouseMove(object sender, MouseEventArgs e) {
            mousePosition = this.PointToClient(Cursor.Position);
        }
    }
}
