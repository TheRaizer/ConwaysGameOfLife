using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConwaysGameOfLife
{
    public partial class Form1 : Form
    {
        private readonly Timer gfxTimer;
        public static int generations = 0;
        private Rectangle resolution = Screen.PrimaryScreen.Bounds;
        public Grid grid;

        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
            Paint += Form1_Paint;
            KeyDown += Form1_KeyDown;
            gfxTimer = new Timer
            {
                Interval = 1
            };
            gfxTimer.Tick += GfxTimer_Tick;
            gfxTimer.Start();
            MouseClick += Form1_MouseClick;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            PaintWithMouse(e.Location);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            if (e.KeyCode == Keys.P)
            {
                grid.pause = !grid.pause;
            }
        }

        private void GfxTimer_Tick(object sender, EventArgs e)
        {
            if (!grid.pause)
            {
                generations++;
                GenerationCount.Text = "Generation: " + generations;
            }
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (generations > 1)
            {
                grid.RedrawCheckGrid(e.Graphics);
            }
            else
            {
                grid.InitializeGrid(e.Graphics);
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            grid = new Grid(resolution.Width, resolution.Height);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void PaintWithMouse(Point e)
        {
            for (int y = 0; y < grid.maxY; y++)
            {
                for(int x = 0; x < grid.maxX; x++)
                {
                    Cell cell = grid.currentGrid[x, y];
                    if ((e.X > cell.x) && (e.X < cell.x * Grid.CELL_LENGTH) && 
                        (e.Y > cell.y) && (e.Y < cell.y * Grid.CELL_LENGTH))
                    {
                        Console.WriteLine("On");
                        cell.alive = !cell.alive;
                        return;
                    }
                }
            }
        }
    }
}

