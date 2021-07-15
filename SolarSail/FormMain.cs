using System;
using System.Windows.Forms;
using System.Collections.Generic;
using SolarSail.SourceCode;

namespace SolarSail
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            comboBoxSelectAlg.SelectedIndex = 0;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            RungeKutta.RungeKuttaCaculate(F);
        }

       public double F(double x,double y) 
       {
            return x + y;
       }

        private void buttonResult_Click(object sender, EventArgs e)
        {

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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
