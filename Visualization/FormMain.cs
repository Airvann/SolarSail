using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetaheuristicHelper;
using OdeSolver;
using System.IO;
using System.Drawing;

namespace Visualization
{
    public partial class FormMain : Form
    {
        private float centerX = 0;
        private float centerY = 0;

        private List<double> t;
        private List<double> r;
        private List<double> theta;
        private List<double> u;
        private List<double> v;
        private List<double> alpha;
        private Orbit orbit;

        public FormMain()
        {
            InitializeComponent();
        }

        public void DataVisualization()
        {
            Result res = Result.Get();
            Settings set = Settings.Get();
            chartAlphat.Series[0].Points.Clear();
            chartRt.Series[0].Points.Clear();
            chartTt.Series[0].Points.Clear();
            chartUt.Series[0].Points.Clear();
            chartVt.Series[0].Points.Clear();

            try
            {
                t     = res.GetT();
                r     = res.GetR();
                theta = res.GetTheta();
                u     = res.GetU();
                v     = res.GetV();
                alpha = res.GetAlpha();

                for (int i = 0; i < res.GetR().Count - 1; i++) 
                {
                    chartRt.Series[0].Points.AddXY(t[i], r[i]);
                    chartTt.Series[0].Points.AddXY(t[i], theta[i]);
                    chartUt.Series[0].Points.AddXY(t[i], u[i]);
                    chartVt.Series[0].Points.AddXY(t[i], v[i]);
                    chartAlphat.Series[0].Points.AddXY(t[i], alpha[i]);
                }
                

                panel1.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка чтения данных", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData(string path)
        {
            Result res = Result.Get();
            Settings set = Settings.Get();
            try
            {
                FileHandler.Read(path);
            }
            catch (FileLoadException)
            {
                MessageBox.Show("Не удалось загрузить данные из файла", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            orbit = set.orbit;
            int dim = res.GetH().Count + res.GetControl().Count;

            Agent agent = new Agent(dim);

            for (int i = 0; i < agent.P; i++)
                agent.Coords[i] = res.GetH()[i];
            
            for (int i = agent.P; i < dim; i++)
                agent.Coords[i] = res.GetControl()[i-agent.P];

            res.Clear();
            set.odeSolver.Solve(agent, Mode.SaveResults);
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            Settings set = Settings.Get();

            if (t != null)
            {
                float width = panel1.Size.Width;
                float height = panel1.Size.Height;

                centerX = width / 2f;
                centerY = height / 2f;

                Draw.DrawSun(centerX, centerY, e);

                Draw.DrawOrbit(set.orbit, centerX, centerY, e);
                Draw.DrawOrbit(MetaheuristicHelper.Orbits.Earth.Get(), centerX, centerY, e);
#if DEBUG
                e.Graphics.DrawLine(Pens.Red, 0, centerY, Width, centerY);
                e.Graphics.DrawLine(Pens.Red, centerX, 0, centerX, Height);
#endif
                Draw.DrawPath(r, theta, t, e, centerX, centerY);
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialogMain.ShowDialog();
            if (result == DialogResult.OK)
            {
                LoadData(openFileDialogMain.FileName);
                DataVisualization();
            }
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            panel1.Refresh();
        }
    }
}
