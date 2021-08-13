using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetaheuristicHelper;
using OdeSolver;
using System.IO;
using System.Drawing.Drawing2D;
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
        private List<double> alpha;

        public FormMain()
        {
            InitializeComponent();
        }

        public void DataVisualization() 
        {
            Result res = Result.getInstance();
            chartAlphat.Series[0].Points.Clear();
            try 
            {
                t = res.GetT();
                r = res.GetR();
                theta = res.GetTheta();
                alpha = res.GetAlpha();

                for (int i = 0; i < res.GetR().Count - 1; i++)
                    chartAlphat.Series[0].Points.AddXY(Result.ConvertFromSecToDays(t[i]), alpha[i]);

                panel1.Refresh();
            }
            catch (Exception) 
            {
                MessageBox.Show("Ошибка чтения данных", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData(string path) 
        {
            AgentFrame agentFrame;
            try 
            {
                agentFrame = FileHandler.Read(path);
            }
            catch (FileLoadException)
            {
                MessageBox.Show("Не удалось загрузить данные из файла", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OdeSolver.OdeSolver solver = new OdeSolver.OdeSolver(agentFrame.splineCoeff, agentFrame.sectionsCount);
            solver.EulerMethod(agentFrame.agent, Mode.SaveResults);
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            if (t != null)
            {
                float width = panel1.Size.Width;
                float height = panel1.Size.Height;

                centerX = width / 2f;
                centerY = height / 2f;

                Draw.DrawSun(centerX, centerY, e);

                Draw.DrawOrbit(Planet.Mercury, centerX, centerY, e);
                Draw.DrawOrbit(Planet.Earth, centerX, centerY, e);
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
                Result res = Result.getInstance();
                res.Clear();
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
