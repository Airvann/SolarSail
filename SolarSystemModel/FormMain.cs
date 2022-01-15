using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace SolarSystemModel
{
    public partial class FormMain : Form
    {
        private static readonly Pen p1 = new Pen(Color.Orange, 4) { DashStyle = DashStyle.Dash };
        private static readonly Pen p2 = new Pen(Color.Blue, 4) { DashStyle = DashStyle.Dash };
        private static readonly Pen p3 = new Pen(Color.Orange, 3);

        private static readonly Brush b1 = Brushes.Yellow;
        private static readonly Brush b2 = Brushes.Blue;
        private static readonly Brush b3 = Brushes.Orange;

        public FormMain()
        {
            InitializeComponent();
        }

        public static void DrawSun(float centerX, float centerY, System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.FillEllipse(b1, centerX - 25, centerY - 25, 50, 50);
            e.Graphics.DrawEllipse(p3, centerX - 25, centerY - 25, 50, 50);
        }

        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            float centerX = Width / 2f;
            float centerY = Height / 2f;

            DrawSun(centerX, centerY, e);
        }
    }
}
