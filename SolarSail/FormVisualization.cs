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
            float r_earth = 1.496f * 500;

            Pen p1 = new Pen(Color.Orange, 4);
            Pen p2 = new Pen(Color.Blue, 4);
            Pen p3 = new Pen(Color.Orange, 2);
            p1.DashStyle = DashStyle.Dash;
            p2.DashStyle = DashStyle.Dash;
            Brush b1 = Brushes.Yellow;
            e.Graphics.FillEllipse(b1, centerX-25, centerY-25, 50, 50);
            e.Graphics.DrawEllipse(p3, centerX - 25, centerY - 25, 50, 50);
            e.Graphics.DrawEllipse(p1, centerX - r_mercury/2, centerY -r_mercury/2, r_mercury, r_mercury);
            e.Graphics.DrawEllipse(p2, centerX - r_earth / 2, centerY - r_earth / 2, r_earth, r_earth);
#if DEBUG
            e.Graphics.DrawLine(Pens.Red, 0, centerY, Width, centerY);
            e.Graphics.DrawLine(Pens.Red, centerX, 0, centerX, Height);
#endif
            SourceCode.Result res = SourceCode.Result.getInstance();
            List<double> t = res.resultTable["t"];
            List<double> r = res.resultTable["r"];
            List<double> theta = res.resultTable["thetta"];
            double x = 0;
            double y = 0;
            for (int i = 0; i < t.Count; i+=8)
            {
                x = (r[i] / Math.Pow(10, 11)) * 500 * Math.Cos(theta[i]);
                y = (r[i] / Math.Pow(10, 11)) * 500 * Math.Sin(theta[i]);
                e.Graphics.FillEllipse(Brushes.Black, centerX + (float)x/2, centerY + (float)y/2, 5, 5);
            }
        }
    }
}
