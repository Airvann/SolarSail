using System;
using System.Windows.Forms;
using System.Collections.Generic;
using SolarSail.SourceCode;
using System.Diagnostics;

namespace SolarSail
{
    public partial class FormMain : Form
    {
        Result result = Result.getInstance();
        public FormMain()
        {
            InitializeComponent();
            comboBoxSelectAlg.SelectedIndex = 0;

            dataGridViewResult.RowCount = 2;
            dataGridViewResult.Rows[0].Cells[0].Value = "Время окончания движения: ";
            dataGridViewResult.Rows[1].Cells[0].Value = "Попадание в эпсилон-окрестность: ";

            chartXt.ChartAreas[0].AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartXt.ChartAreas[0].AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
        }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            GWO alg = new GWO();
           // alg.CalculateResult(100, 0, 100, -3.1415f / 2, 3.1415926f / 2, 100, Params.Linear, 10, 5);

            FillResultTable();
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

        private void FillResultTable()
        {
            result.resultTable = new Dictionary<string, List<double>>();
            result.Clear();
            dataGridViewTableRes.Rows.Clear();

            result.resultTable.TryGetValue("t",        out List<double> t);
            result.resultTable.TryGetValue("r",        out List<double> r);
            result.resultTable.TryGetValue("u",        out List<double> u);
            result.resultTable.TryGetValue("v",        out List<double> v);
            result.resultTable.TryGetValue("thetta",   out List<double> thetta);
            result.resultTable.TryGetValue("alpha",    out List<double> alpha);

            if (t != null && r != null && u != null && v != null && thetta != null &&  alpha != null)
                for (int i = 0; i < t.Count; i++)
                    dataGridViewTableRes.Rows.Add(t[i], r[i], thetta[i], u[i], v[i], alpha);
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
    }
}
