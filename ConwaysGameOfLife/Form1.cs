using System;
using System.Drawing;
using System.Windows.Forms;

namespace ConwaysGameOfLife
{
    public partial class Form1 : Form
    {
        private readonly Timer gfxTimer;
        private int generations = 0;
        private Rectangle resolution = Screen.PrimaryScreen.Bounds;
        public Grid grid;
        private bool startLife = false;

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
            Console.WriteLine(e.Location);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
            if (e.KeyCode == Keys.S)
            {
                startLife = true;
            }
            if (e.KeyCode == Keys.P)
            {
                grid.pause = !grid.pause;
            }
        }

        private void GfxTimer_Tick(object sender, EventArgs e)
        {
            if (startLife && !grid.pause)
            {
                generations++;
                GenerationCount.Text = "Generation: " + generations;
            }
            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (startLife)
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
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            DoubleBuffered = true;
            grid = new Grid(resolution.Width, resolution.Height);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}

