using SolarSail.SourceCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace SolarSail
{
    public partial class FormMain : Form
    {
        Agent best;
        public FormMain()
        {
            InitializeComponent();
            comboBoxSelectAlg.SelectedIndex = 0;

            dataGridViewResult.RowCount = 2;
            dataGridViewResult.Rows[0].Cells[0].Value = "Время окончания движения: ";
            dataGridViewResult.Rows[1].Cells[0].Value = "Попадание в эпсилон-окрестность: ";
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            Result res = Result.getInstance();
            res.Clear();

            int maxIterationCount = Convert.ToInt32(dataGridViewParam.Rows[0].Cells[1].Value);
            int populationCount = Convert.ToInt32(dataGridViewParam.Rows[1].Cells[1].Value);
            int partsCount = 0;
            int b = 0;

            IMetaAlgorithm alg;
            object[] param;
            switch (comboBoxSelectAlg.SelectedIndex)
            {
                case 0:
                    alg = new GWO();
                    partsCount = Convert.ToInt32(dataGridViewParam.Rows[2].Cells[1].Value);
                    object[] paramGWO = { maxIterationCount, Params.Linear, partsCount };
                    param = paramGWO;
                    break;
                case 1:
                    alg = new WOA();
                    b = Convert.ToInt32(dataGridViewParam.Rows[2].Cells[1].Value);
                    partsCount = Convert.ToInt32(dataGridViewParam.Rows[3].Cells[1].Value);
                    object[] paramWOA = { maxIterationCount, Params.Linear, partsCount, b };
                    param = paramWOA;
                    break;
                default:
                    alg = new GWO();
                    object[] paramDefault = { maxIterationCount, Params.Linear, partsCount };
                    param = paramDefault;
                    break;
            }

            best = alg.CalculateResult(populationCount, 40000, 50000, -Math.PI / 2f, Math.PI / 2f, param);
            FillResultTable(best);
            buttonVisual.Enabled = true;
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

        private void FillResultTable(Agent best)
        {
            Result res = Result.getInstance();
            chartRt.Series[0].Points.Clear();
            chartTt.Series[0].Points.Clear();
            chartUt.Series[0].Points.Clear();
            chartVt.Series[0].Points.Clear();
            chartAlfat.Series[0].Points.Clear();

            dataGridViewResult.Rows[0].Cells[1].Value = best.tf/60f/60f/24f;
            dataGridViewResult.Rows[1].Cells[1].Value = best.Fitness;

            for (int i = 0; i < res.resultTable["r"].Count - 1; i++)
            {
                chartAlfat.Series[0].Points.AddXY(res.resultTable["t"][i], res.resultTable["alpha"][i]);

                chartRt.Series[0].Points.AddXY(res.resultTable["t"][i], res.resultTable["r"][i]);
                chartTt.Series[0].Points.AddXY(res.resultTable["t"][i], res.resultTable["thetta"][i]);
                chartUt.Series[0].Points.AddXY(res.resultTable["t"][i], res.resultTable["u"][i]);
                chartVt.Series[0].Points.AddXY(res.resultTable["t"][i], res.resultTable["v"][i]);
            }
        }

        private void comboBoxSelectAlg_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxSelectAlg.SelectedIndex)
            {
                case 0:
                    FillParamTable(GWO.PAR());
                    break;
                case 1:
                    FillParamTable(WOA.PAR());
                    break;
                default:
                    break;
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
            try
            {
                FormVisualization visualization = new FormVisualization();
                visualization.Show();
            }
            catch (MemberAccessException)
            {
                MessageBox.Show("Отсутствуют данные для визуализации", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
