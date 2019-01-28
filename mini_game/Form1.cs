using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mini_game
{



    public partial class Form1 : Form
    {
        public bool g = false, sel = false;
        public int w = 50;
        public int h = 50;
        public int g_x = 0;
        public int g_y = 0;
        public int stat = 0;
        public int[,] p_array = new int[5, 5];

        public void Random_Gen()
        {
            for(int i = 1; i <= 3; i+=2)
            {
                for (int j = 0; j < 5; j+=2)
                {
                    p_array[i,j] = -1;
                }
            }
            //for (int i = 0; i <= 5; i += 2)
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        p_array[i, j] = 1;
            //    }
            //}

            int f = 1;
            Random rand = new Random();
            for (int i = 0; i < 15; i++)
            {
                if (i == 5)
                    f++;
                if (i == 10)
                    f++;
                int x1 = rand.Next(0, 5); int y1 = rand.Next(0, 5);
                if (p_array[x1, y1] != 0)
                {
                    if (i == 5 || i == 10)
                        f--;
                    i--;
                }
                else
                    p_array[x1, y1] = f;
            }

        }


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint_1(object sender, PaintEventArgs e)
        {
            DrawGrid(e, 5, 5, 0);
            Render(e.Graphics);
        }

        public void DrawGrid(PaintEventArgs e, int xCells, int yCells, int oth)
        {
            using (Pen pen = new Pen(Color.Black, 1))
            {
                //Горизонтальные линии
                for (int i = oth; i <= xCells; i++)
                    e.Graphics.DrawLine(pen, i * w, 0, i * w, h * yCells);
                //Вертикальные линии
                for (int i = 0; i <= yCells; i++)
                    e.Graphics.DrawLine(pen, oth * 20, i * h, w * xCells, i * h);
            }
        }
        
        public void Render(Graphics e)
        {
            if (g)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (p_array[i, j] == -1)
                        {
                            Pen pen = new Pen(Color.Black, 2);
                            e.DrawRectangle(pen, i * 50 + 10, j * 50 + 10, 30, 30);
                        }
                        if (p_array[i, j] == 1)
                        {
                            e.FillEllipse(Brushes.Green, new Rectangle(i * 50 + 2, j * 50 + 2, 46, 46));
                        }
                        if (p_array[i, j] == 2)
                        {
                            e.FillEllipse(Brushes.Red, new Rectangle(i * 50 + 2, j * 50 + 2, 46, 46));
                        }
                        if (p_array[i, j] == 3)
                        {
                            e.FillEllipse(Brushes.Blue, new Rectangle(i * 50 + 2, j * 50 + 2, 46, 46));
                        }
                    }
                }
                if (sel)
                {
                    Pen pen = new Pen(Color.Yellow, 4);
                    e.DrawEllipse(pen, g_x * 50 + 2, g_y * 50 + 2, 46, 46);
                }
            }
            else
            {
                Random_Gen();
                g = true;
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int x1 = e.X / 50, y1 = e.Y / 50;
            if (x1 < 5 && y1 < 5)
            {
                if (!sel)
                {
                    if (p_array[x1, y1] > 0)
                    {
                        g_x = x1; g_y = y1;
                        sel = true;
                    }
                }
                else
                {
                    int r1 = Math.Abs(g_x - x1), r2 = Math.Abs(g_y - y1);
                    if (((r1 < 2 && r2 == 0) || (r2 < 2 && r1 == 0)) && p_array[x1, y1] == 0)
                    {
                        p_array[x1, y1] = p_array[g_x, g_y];
                        p_array[g_x, g_y] = 0;
                        g_x = x1; g_y = y1;
                        stat++;
                    }
                    else
                        sel = false;
                }
            }
        }

        public bool Rules()
        {
            label1.Text = "vsego hodov - " + stat;
            if ((p_array[0, 0] == 1 || p_array[0, 0] == 2 || p_array[0, 0] == 3) && p_array[0,0] == p_array[0,1] && p_array[0, 0] == p_array[0,2] && p_array[0, 0] == p_array[0,3] && p_array[0, 0] == p_array[0, 4])
            {
                if (p_array[2, 0] == p_array[2, 1] && p_array[2, 0] == p_array[2, 2] && p_array[2, 0] == p_array[2, 3] && p_array[2, 0] == p_array[2, 4])
                {
                    if (p_array[4, 0] == p_array[4, 1] && p_array[4, 0] == p_array[4, 2] && p_array[4, 0] == p_array[4, 3] && p_array[4, 0] == p_array[4, 4])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // Turn on WS_EX_COMPOSITED
                cp.ExStyle |= 0x00080000;   //WS_EX_LAYERED    
                return cp;
            }
        }


        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (this.Rules())
            {
                Invalidate();
            }
            else
            {
                timer1.Enabled = false;
                MessageBox.Show("Game over");
            }
        }
    }
}
