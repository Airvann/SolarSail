﻿using SolarSail.SourceCode;
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
            InitilizeTableValues();
            comboBoxSelectAlg.SelectedIndex = 0;
        }

        private void InitilizeTableValues() 
        {
            dataGridViewMainParams.RowCount = 8;
            dataGridViewMainParams.Rows[0].SetValues("Нижняя грань отрезка", 2000);
            dataGridViewMainParams.Rows[1].SetValues("Верхняя грань отрезка", 3000);
            dataGridViewMainParams.Rows[2].SetValues("Параметр сплайна", 2);
            dataGridViewMainParams.Rows[3].SetValues("λ1", 100000);
            dataGridViewMainParams.Rows[4].SetValues("λ2", 100000);
            dataGridViewMainParams.Rows[5].SetValues("λ3", 100000);
            dataGridViewMainParams.Rows[6].SetValues("Нижняя грань коэффициентов", -1.56);
            dataGridViewMainParams.Rows[7].SetValues("Верхняя грань коэффициентов", 1.56);

            dataGridViewResult.RowCount = 2;
            dataGridViewResult.Rows[0].Cells[0].Value = "Время окончания движения: ";
            dataGridViewResult.Rows[1].Cells[0].Value = "Значение качества управления решения: ";
        }
        private void buttonResult_Click(object sender, EventArgs e)
        {
            IMetaAlgorithm alg;
            Result res = Result.getInstance();
            res.Clear();
            int partsCount;
            object[] param;
            int lambda3;
            int lambda2;
            int lambda1;
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
                lambda1                      = Convert.ToInt32(dataGridViewMainParams.Rows[3].Cells[1].Value);
                lambda2                      = Convert.ToInt32(dataGridViewMainParams.Rows[4].Cells[1].Value);
                lambda3                      = Convert.ToInt32(dataGridViewMainParams.Rows[5].Cells[1].Value);

                bottomBorderFunc             = Convert.ToDouble(dataGridViewMainParams.Rows[6].Cells[1].Value);
                topBorderFunc                = Convert.ToDouble(dataGridViewMainParams.Rows[7].Cells[1].Value);

                switch (comboBoxSelectAlg.SelectedIndex)
                {
                    case 0:
                        alg = new GWO();
                        partsCount = Convert.ToInt32(dataGridViewParam.Rows[2].Cells[1].Value);
                        object[] paramGWO = { maxIterationCount, partsCount };
                        param = paramGWO;
                        break;
                    case 1:
                        alg = new WOA();
                        int b = Convert.ToInt32(dataGridViewParam.Rows[2].Cells[1].Value);
                        partsCount = Convert.ToInt32(dataGridViewParam.Rows[3].Cells[1].Value);
                        object[] paramWOA = { maxIterationCount, partsCount, b };
                        param = paramWOA;
                        break;
                    default:
                        alg = new GWO();
                        object[] paramDefault = { maxIterationCount, 0 };
                        param = paramDefault;
                        break;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Были введены некорретные параметры", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            best = alg.CalculateResult(populationCount, bottomBorderSection, topBorderSection, bottomBorderFunc, topBorderFunc, lambda1, lambda2, lambda3, p, param);
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

        private void ClearAllGraphs() 
        {
            chartRt.Series[0].Points.Clear();
            chartTt.Series[0].Points.Clear();
            chartUt.Series[0].Points.Clear();
            chartVt.Series[0].Points.Clear();
            chartAlfat.Series[0].Points.Clear();
        }

        private void FillResultTable(Agent best)
        {
            Result res = Result.getInstance();
            ClearAllGraphs();

            dataGridViewResult.Rows[0].Cells[1].Value = ConvertFromSecToDays(best.tf);
            dataGridViewResult.Rows[1].Cells[1].Value = best.Fitness;

            for (int i = 0; i < res.resultTable["r"].Count - 1; i++)
            {
                chartAlfat.Series[0].Points.AddXY(ConvertFromSecToDays(res.resultTable["t"][i]), res.resultTable["alpha"][i]);

                chartRt.Series[0].Points.AddXY(ConvertFromSecToDays(res.resultTable["t"][i]), res.resultTable["r"][i]);
                chartTt.Series[0].Points.AddXY(ConvertFromSecToDays(res.resultTable["t"][i]), res.resultTable["thetta"][i]);
                chartUt.Series[0].Points.AddXY(ConvertFromSecToDays(res.resultTable["t"][i]), res.resultTable["u"][i]);
                chartVt.Series[0].Points.AddXY(ConvertFromSecToDays(res.resultTable["t"][i]), res.resultTable["v"][i]);
            }
        }

        private double ConvertFromSecToDays(double t) 
        {
            return (t / 60f / 60f / 24f);
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
