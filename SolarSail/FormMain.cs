using SolarSail.SourceCode;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using MetaheuristicHelper;
using Visualization;

namespace SolarSail
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            InitilizeTableValues();
            comboBoxSelectAlg.SelectedIndex = 0;
        }

        private void InitilizeTableValues() 
        {
            dataGridViewMainParams.RowCount = 9;
            dataGridViewMainParams.Rows[0].SetValues("Нижняя грань отрезка", 0);
            dataGridViewMainParams.Rows[1].SetValues("Верхняя грань отрезка", 18000);
            dataGridViewMainParams.Rows[2].SetValues("Параметр сплайна", 3);
            dataGridViewMainParams.Rows[3].SetValues("λ1", 10000000000000000);
            dataGridViewMainParams.Rows[4].SetValues("λ2", 25);
            dataGridViewMainParams.Rows[5].SetValues("λ3", 4000000000000);
            dataGridViewMainParams.Rows[6].SetValues("λ4", 5500000000000);
            dataGridViewMainParams.Rows[7].SetValues("Нижняя грань коэффициентов", -1.56);
            dataGridViewMainParams.Rows[8].SetValues("Верхняя грань коэффициентов", 0);

            dataGridViewResult.RowCount = 2;
            dataGridViewResult.Rows[0].Cells[0].Value = "Время окончания движения: ";
            dataGridViewResult.Rows[1].Cells[0].Value = "Значение качества управления решения: ";
        }
        private async void buttonResult_Click(object sender, EventArgs e)
        {
            IMetaAlgorithm alg;
            Result res = Result.getInstance();
            res.Clear();
            richTextBox1.Clear();
            int partsCount;
            object[] param;
            long lambda4;
            long lambda3;
            long lambda2;
            long lambda1;
            int p;

            int topBorderSection;
            int bottomBorderSection;

            double topBorderFunc;
            double bottomBorderFunc;

            int populationCount;
            try
            {
                int maxIterationCount        = Convert.ToInt32(dataGridViewParam.Rows[0].Cells[1].Value);
                populationCount              = Convert.ToInt32(dataGridViewParam.Rows[1].Cells[1].Value);

                bottomBorderSection          = Convert.ToInt32(dataGridViewMainParams.Rows[0].Cells[1].Value);
                topBorderSection             = Convert.ToInt32(dataGridViewMainParams.Rows[1].Cells[1].Value);
                p                            = Convert.ToInt32(dataGridViewMainParams.Rows[2].Cells[1].Value);

                lambda1                      = Convert.ToInt64(dataGridViewMainParams.Rows[3].Cells[1].Value);
                lambda2                      = Convert.ToInt64(dataGridViewMainParams.Rows[4].Cells[1].Value);
                lambda3                      = Convert.ToInt64(dataGridViewMainParams.Rows[5].Cells[1].Value);
                lambda4                      = Convert.ToInt64(dataGridViewMainParams.Rows[6].Cells[1].Value);

                bottomBorderFunc             = Convert.ToDouble(dataGridViewMainParams.Rows[7].Cells[1].Value);
                topBorderFunc                = Convert.ToDouble(dataGridViewMainParams.Rows[8].Cells[1].Value);

                partsCount = Convert.ToInt32(dataGridViewParam.Rows[2].Cells[1].Value);

                switch (comboBoxSelectAlg.SelectedIndex)
                {
                    case 0:
                        alg = new GWO();
                        object[] paramGWO = { maxIterationCount };
                        param = paramGWO;
                        break;
                    case 1:
                        alg = new WOA();
                        int b = Convert.ToInt32(dataGridViewParam.Rows[3].Cells[1].Value);
                        object[] paramWOA = { maxIterationCount, b };
                        param = paramWOA;
                        break;
                    default:
                        alg = new GWO();
                        object[] paramDefault = { maxIterationCount };
                        param = paramDefault;
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Были введены некорретные параметры", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            buttonResult.Enabled = false;
            buttonVisual.Enabled = false;
            buttonSaveResult.Enabled = false;
            await Task.Run(() => alg.CalculateResult(populationCount, bottomBorderSection, topBorderSection, bottomBorderFunc, topBorderFunc, lambda1, lambda2, lambda3, lambda4, p, partsCount, param));
            FillResultTable();
            richTextBox1.Text += res.PrintResult();
            LoadInFile(alg);
            buttonResult.Enabled = true;
            buttonVisual.Enabled = true;
            buttonSaveResult.Enabled = true;
        }


        private void LoadInFile(IMetaAlgorithm alg)
        {
            FileStream fs = new FileStream("log.txt", FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(alg.PrintParams());
            sw.Write(Result.getInstance().PrintResult());
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
            Result res = Result.getInstance();
            List<double> r      = res.GetR();
            List<double> theta  = res.GetTheta();
            List<double> u      = res.GetU();
            List<double> v      = res.GetV();
            List<double> t      = res.GetT();
            List<double> alpha  = res.GetAlpha();
            ClearAllGraphs();

            dataGridViewResult.Rows[0].Cells[1].Value = Result.ConvertFromSecToDays(res.tf);
            dataGridViewResult.Rows[1].Cells[1].Value = res.fitness;

            for (int i = 0; i < res.GetR().Count - 1; i++)
            {
                chartAlfat.Series[0].Points.AddXY(Result.ConvertFromSecToDays(t[i]), alpha[i]);

                chartRt.Series[0].Points.AddXY(Result.ConvertFromSecToDays(t[i]), r[i]);
                chartTt.Series[0].Points.AddXY(Result.ConvertFromSecToDays(t[i]), theta[i]);
                chartUt.Series[0].Points.AddXY(Result.ConvertFromSecToDays(t[i]), u[i]);
                chartVt.Series[0].Points.AddXY(Result.ConvertFromSecToDays(t[i]), v[i]);
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

        private void FormMain_Load(object sender, EventArgs e)
        {

        }

        private void buttonSaveResult_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                string file = saveFileDialog1.FileName;
                FileHandler.Write(file);
            }
        }
    }
}
