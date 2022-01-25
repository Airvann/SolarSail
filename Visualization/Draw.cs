using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using MetaheuristicHelper;

namespace Visualization
{
    public class Draw
    {
        private static readonly Pen p1 = new Pen(Color.Orange, 4)  { DashStyle = DashStyle.Dash };
        private static readonly Pen p2 = new Pen(Color.Blue, 4)    { DashStyle = DashStyle.Dash };
        private static readonly Pen p3 = new Pen(Color.Orange, 3);
                
        private static readonly Brush b1 = Brushes.Yellow;
        private static readonly Brush b2 = Brushes.Blue;
        private static readonly Brush b3 = Brushes.Orange;

        public static void DrawOrbit(Orbit orbit, float centerX, float centerY, System.Windows.Forms.PaintEventArgs e) 
        {
            float orbit_r = (float)orbit.GetR() * 500;
            e.Graphics.DrawEllipse(p2, centerX - orbit_r / 2, centerY - orbit_r / 2, orbit_r, orbit_r);
        }

        public static void DrawSun(float centerX, float centerY, System.Windows.Forms.PaintEventArgs e)
        {
            e.Graphics.FillEllipse(b1, centerX - 25, centerY - 25, 50, 50);
            e.Graphics.DrawEllipse(p3, centerX - 25, centerY - 25, 50, 50);
        }

        public static void DrawPath(IReadOnlyList<double> r, IReadOnlyList<double> theta, IReadOnlyList<double> T, System.Windows.Forms.PaintEventArgs e, float centerX, float centerY)
        {
            double x1 = 0;
            double y1 = 0;

            double x2 = 0;
            double y2 = 0;

            for (int i = 5; i < T.Count - 1; i += 5)
            {
                x1 = r[i] * 500 * Math.Cos(theta[i]);
                y1 = r[i] * 500 * Math.Sin(theta[i]);

                x2 = r[i - 5] * 500 * Math.Cos(theta[i - 5]);
                y2 = r[i - 5] * 500 * Math.Sin(theta[i - 5]);

                e.Graphics.DrawLine(Pens.Gray, centerX - (float)x1 / 2, centerY - (float)y1 / 2, centerX - (float)x2 / 2, centerY - (float)y2 / 2);
                if (i == 5) e.Graphics.FillEllipse(b2, centerX - (float)x1 / 2 - 12.5f, centerY - (float)y1 / 2 - 12.5f, 25, 25);
            }
            e.Graphics.FillEllipse(b3, centerX - (float)x1 / 2 - 12.5f, centerY - (float)y1 / 2 - 12.5f, 25, 25);
        }
    }
}
