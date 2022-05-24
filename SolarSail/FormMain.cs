using SolarSail.SourceCode;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.IO;
using MetaheuristicHelper;
using Visualization;
using SolarSystemOrbitChooser;

namespace SolarSail
{
    public partial class FormMain : Form
    {
        public Orbit orbit = MetaheuristicHelper.Orbits.Mercury.Get();
        private System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.FFXV_Victory_Fanfare);

        public FormMain()
        {
            InitializeComponent();
            InitilizeTableValues();
            comboBoxSelectAlg.SelectedIndex = 0;
            comboBoxODESolverChooser.SelectedIndex = 0;
        }
        
        private void InitilizeTableValues()
        {
            dataGridViewMainParams.RowCount = 9;
            dataGridViewMainParams.Rows[0].SetValues("Нижняя грань отрезка",           0);
            dataGridViewMainParams.Rows[1].SetValues("Верхняя грань отрезка",          2);
            dataGridViewMainParams.Rows[2].SetValues("Параметр сплайна",               2);
            dataGridViewMainParams.Rows[3].SetValues("λ₁",                             1);
            dataGridViewMainParams.Rows[4].SetValues("λ₂",                             10000);
            dataGridViewMainParams.Rows[5].SetValues("λ₃",                             10000);
            dataGridViewMainParams.Rows[6].SetValues("λ₄",                             10000);
            dataGridViewMainParams.Rows[7].SetValues("Нижняя грань коэффициентов", -1.56);
            dataGridViewMainParams.Rows[8].SetValues("Верхняя грань коэффициентов", 1.56);
        }
        private async void buttonResult_Click(object sender, EventArgs e)
        {
            IMetaAlgorithm alg;
            Result res = Result.Get();
            Settings set = Settings.Get();

            res.Clear();
            set.Clear();
            richTextBoxInfo.Clear();

            object[] param;
            try
            {
                int maxIterationCount            = Convert.ToInt32(dataGridViewParam.Rows[0].Cells[1].Value);
                int populationCount              = Convert.ToInt32(dataGridViewParam.Rows[1].Cells[1].Value);

                set.bottomBorderSection          = Convert.ToDouble(dataGridViewMainParams.Rows[0].Cells[1].Value);
                set.topBorderSection             = Convert.ToDouble(dataGridViewMainParams.Rows[1].Cells[1].Value);
                set.splineCoeff                  = Convert.ToInt32(dataGridViewMainParams.Rows[2].Cells[1].Value);

                set.lambda1                      = Convert.ToInt64(dataGridViewMainParams.Rows[3].Cells[1].Value);
                set.lambda2                      = Convert.ToInt64(dataGridViewMainParams.Rows[4].Cells[1].Value);
                set.lambda3                      = Convert.ToInt64(dataGridViewMainParams.Rows[5].Cells[1].Value);
                set.lambda4                      = Convert.ToInt64(dataGridViewMainParams.Rows[6].Cells[1].Value);

                set.bottomBorderFunc             = Convert.ToDouble(dataGridViewMainParams.Rows[7].Cells[1].Value);
                set.topBorderFunc                = Convert.ToDouble(dataGridViewMainParams.Rows[8].Cells[1].Value);
                set.sectionsCount                = Convert.ToInt32(dataGridViewParam.Rows[2].Cells[1].Value);

                set.orbit                        = orbit;
                set.odeSolverStep                = Convert.ToDouble(textBoxODESolvingStep.Text);
                set.brightness                   = Convert.ToDouble(textBoxBrightnessSolarSail.Text);

                switch (comboBoxODESolverChooser.SelectedIndex)
                {
                    case 0:
                        set.odeSolver = new OdeSolver.EulerMethod(set.splineCoeff, set.sectionsCount, set.brightness, set.odeSolverStep);
                        break;
                    case 1:
                        set.odeSolver = new OdeSolver.RungeKutta4Method(set.splineCoeff, set.sectionsCount, set.brightness, set.odeSolverStep);
                        break;
                    default:
                        set.odeSolver = new OdeSolver.EulerMethod(set.splineCoeff, set.sectionsCount, set.brightness, set.odeSolverStep);
                        break;
                }

                switch (comboBoxSelectAlg.SelectedIndex)
                {
                    case 0:
                        alg = new GWO();
                        object[] paramGWO = { maxIterationCount, populationCount };
                        param = paramGWO;
                        break;
                    case 1:
                        alg = new WOA();
                        double b = Convert.ToDouble(dataGridViewParam.Rows[3].Cells[1].Value);
                        object[] paramWOA = { maxIterationCount, populationCount, b };
                        param = paramWOA;
                        break;
                    case 2:
                        alg = new DA();
                        object[] paramDA = { maxIterationCount, populationCount };
                        param = paramDA;
                        break;
                    default:
                        alg = new GWO();
                        object[] paramDefault = { maxIterationCount, populationCount };
                        param = paramDefault;
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Были введены некорретные параметры", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            buttonResult.Enabled        = false;
            buttonVisual.Enabled        = false;
            buttonSaveResult.Enabled    = false;
            buttonChooseTarget.Enabled  = false;
            await Task.Run(() => alg.CalculateResult(param));
            FillResultTable();

            richTextBoxInfo.Text += res.PrintResult();

            LoadInFile();
            buttonResult.Enabled        = true;
            buttonVisual.Enabled        = true;
            buttonSaveResult.Enabled    = true;
            buttonChooseTarget.Enabled  = true;

            player.Play();
        }

        private void LoadInFile()
        {
            FileStream fs = new FileStream("log.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            sw.Write(Settings.Get().PrintSettings());
            sw.Write(Result.Get().PrintResult());
            sw.Close();
            fs.Close();
        }

        private void FillParamTable(Dictionary<string, object> list)
        {
            if (dataGridViewParam.Rows.Count != 0)
                dataGridViewParam.Rows.Clear();
            foreach (var row in list)
            {
                object[] value = { row.Key, row.Value };
                dataGridViewParam.Rows.Add(value);
            }
        }

        private void ClearAllGraphs() 
        {
            chartRt.Series[0].Points.Clear();
            chartTt.Series[0].Points.Clear();
            chartUt.Series[0].Points.Clear();
            chartVt.Series[0].Points.Clear();
            chartAlfat.Series[0].Points.Clear();
        }

        private void FillResultTable()
        {
            Result res = Result.Get();
            List<double> r      = res.GetR();
            List<double> theta  = res.GetTheta();
            List<double> u      = res.GetU();
            List<double> v      = res.GetV();
            List<double> t      = res.GetT();
            List<double> alpha  = res.GetAlpha();
            ClearAllGraphs();

            for (int i = 0; i < res.GetR().Count - 1; i++)
            {
                chartAlfat.Series[0].Points.AddXY(t[i], alpha[i]);

                chartRt.Series[0].Points.AddXY(t[i], r[i]);
                chartTt.Series[0].Points.AddXY(t[i], theta[i]);
                chartUt.Series[0].Points.AddXY(t[i], u[i]);
                chartVt.Series[0].Points.AddXY(t[i], v[i]);
            }
        }

        private void comboBoxSelectAlg_SelectedIndexChanged(object sender, EventArgs e)
        {
            try 
            {
                switch (comboBoxSelectAlg.SelectedIndex)
                {
                    case 0:
                        FillParamTable(GWO.AlgParams());
                        break;
                    case 1:
                        FillParamTable(WOA.AlgParams());
                        break;
                    case 2:
                        FillParamTable(DA.AlgParams());
                        break;
                    default:
                        throw new Exception();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Выберите корректные варианты из предлеженных","Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("HelpFile.pdf");
        }

        private void buttonVisual_Click(object sender, EventArgs e)
        {
            Visualization.FormMain visualization = new Visualization.FormMain();
            visualization.DataVisualization();
            visualization.Show();
        }

        private void buttonSaveResult_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string file = saveFileDialog1.FileName;
                FileHandler.Write(file + ".txt");
            }
        }

        private void buttonChooseTarget_Click(object sender, EventArgs e)
        {
            MainWindow selectionWindow = new MainWindow();
            ElementHost.EnableModelessKeyboardInterop(selectionWindow);
            selectionWindow.ShowDialog();
            orbit = selectionWindow.targetOrbit;
            UpdateTargetOrbit();
        }

        private void UpdateTargetOrbit()
        {
            labelOrbit.Text = orbit.GetName();
        }
    }
}