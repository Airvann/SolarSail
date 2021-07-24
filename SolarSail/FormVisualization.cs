using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace SolarSail
{
    public partial class FormVisualization : Form
    {
        float centerX = 0;
        float centerY = 0;

        public FormVisualization()
        {
            InitializeComponent();
            SourceCode.Result res = SourceCode.Result.getInstance();
#if !DEBUG
            if (res.resultTable.Count == 0)
                throw new MemberAccessException();
#endif
            float width = Size.Width;
            float height = Size.Height;

            centerX = width / 2f;
            centerY = height / 2f;
            Refresh();
        }

        private void FormVisualization_Paint(object sender, PaintEventArgs e)
        {
            float r_mercury = 5.8344f * 50;
            float r_earth = 14.96f * 50;

            Pen p1 = new Pen(Color.Orange, 4);
            Pen p2 = new Pen(Color.Blue, 4);
            Pen p3 = new Pen(Color.Orange, 3);
            p1.DashStyle = DashStyle.Dash;
            p2.DashStyle = DashStyle.Dash;
            Brush b1 = Brushes.Yellow;
            Brush b2 = Brushes.Blue;
            Brush b3 = Brushes.Orange;
            e.Graphics.FillEllipse(b1, centerX - 25, centerY - 25, 50, 50);
            e.Graphics.DrawEllipse(p3, centerX - 25, centerY - 25, 50, 50);
            e.Graphics.DrawEllipse(p1, centerX - r_mercury/2, centerY - r_mercury/2, r_mercury, r_mercury);
            e.Graphics.DrawEllipse(p2, centerX - r_earth/2, centerY - r_earth/2, r_earth, r_earth);

#if DEBUG
            e.Graphics.DrawLine(Pens.Red, 0, centerY, Width, centerY);
            e.Graphics.DrawLine(Pens.Red, centerX, 0, centerX, Height);
#endif
            SourceCode.Result res = SourceCode.Result.getInstance();
            List<double> t = res.resultTable["t"];
            List<double> r = res.resultTable["r"];
            List<double> theta = res.resultTable["thetta"];

            double x1 = 0;
            double y1 = 0;
            double x2 = 0;
            double y2 = 0;
            for (int i = 5; i < t.Count; i+=5)
            {
                x1 = (r[i] / Math.Pow(10, 11)) * 500 * Math.Cos(theta[i]);
                y1 = (r[i] / Math.Pow(10, 11)) * 500 * Math.Sin(theta[i]);

                x2 = (r[i-5] / Math.Pow(10, 11)) * 500 * Math.Cos(theta[i-5]);
                y2 = (r[i-5] / Math.Pow(10, 11)) * 500 * Math.Sin(theta[i-5]);

                if (i == 5) 
                {
                    e.Graphics.FillEllipse(b2, centerX - (float)x1/2 - 12.5f, centerY - (float)y1/2 - 12.5f, 25,25);
                }

                e.Graphics.DrawLine(Pens.Gray, centerX - (float)x1 / 2, centerY - (float)y1 / 2, centerX - (float)x2 / 2, centerY - (float)y2 / 2);
            }
            e.Graphics.FillEllipse(b3, centerX - (float)x1 / 2 - 12.5f, centerY - (float)y1 / 2 - 12.5f, 25, 25);
        }
    }
}
