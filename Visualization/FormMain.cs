using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using MetaheuristicHelper;
using OdeSolver;


namespace Visualization
{
    public partial class FormMain : Form
    {
        float centerX = 0;
        float centerY = 0;

        List<double> t;
        List<double> r;
        List<double> theta;

        public FormMain()
        {
            InitializeComponent();

            float width = Size.Width;
            float height = Size.Height;
            centerX = width / 2f;
            centerY = height / 2f;
        }

        public void DataVisualization() 
        {
            Result res = Result.getInstance();
            try 
            {
                t = res.GetT();
                r = res.GetR();
                theta = res.GetTheta();
            }
            catch (Exception) 
            {
                MessageBox.Show("Ошибка чтения данных", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Refresh();
        }

        private void FormVisualization_Paint(object sender, PaintEventArgs e)
        {
            if (t != null)
            {
                float width = Size.Width;
                float height = Size.Height;

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

        private void LoadData() 
        {
            AgentFrame agentFrame;
            try 
            {
                agentFrame = FileHandler.Read();
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось загрузить данные из файла", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OdeSolver.OdeSolver solver = new OdeSolver.OdeSolver(agentFrame.splineCoeff, agentFrame.sectionsCount);
            solver.EulerMethod(agentFrame.agent, Mode.SaveResults);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            Result res = Result.getInstance();
            res.Clear();
            LoadData();  
            DataVisualization();
        }
    }
}
