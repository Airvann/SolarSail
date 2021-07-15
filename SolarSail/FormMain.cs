using System;
using System.Windows.Forms;
using System.Collections.Generic;
using SolarSail.SourceCode;
using System.Diagnostics;

namespace SolarSail
{
    public partial class FormMain : Form
    {
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

        private void FormMain_Load(object sender, EventArgs e)
        {
            RungeKutta a = new RungeKutta();
            a.RungeKuttaCaculate(F);
        }

       public double F(double t, double x) 
       {
            return x;
       }

        private void buttonResult_Click(object sender, EventArgs e)
        {
            RungeKutta rungeKutta = new RungeKutta();
            List<double> h = new List<double>();
            h.Add(1);
            h.Add(1);

            Dictionary<double, double> res1 =  rungeKutta.RungeKuttaCaculate(h, F);          //!!!

            foreach(var item in res1) 
            {
                chartXt.Series[0].Points.AddXY(item.Key, item.Value);
            }

            Dictionary<double, double> res2 = rungeKutta.RungeKuttaCaculate(F);
            
            chartXt.Series.Add("exact");
            chartXt.Series[1].BorderWidth = 3;
            chartXt.Series[1].Color = System.Drawing.Color.Red;
            chartXt.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            foreach (var item in res2)
            {
                chartXt.Series[1].Points.AddXY(item.Key, item.Value);
            }

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

        private void FillResultTable(Dictionary<string, object> list)
        {



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
