
namespace Visualization
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title4 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.openFileDialogMain = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chartAlphat = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chartRt = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.chartTt = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.chartUt = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.chartVt = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartAlphat)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRt)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartTt)).BeginInit();
            this.tabPage5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartUt)).BeginInit();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartVt)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialogMain
            // 
            this.openFileDialogMain.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Location = new System.Drawing.Point(0, 26);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(810, 811);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMain_Paint);
            // 
            // chartAlphat
            // 
            this.chartAlphat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.Crossing = -1.7976931348623157E+308D;
            chartArea1.AxisX.Minimum = 0D;
            chartArea1.AxisX.Title = "t";
            chartArea1.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea1.AxisX.TitleFont = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea1.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Stacked;
            chartArea1.AxisY.Title = "α";
            chartArea1.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea1.AxisY.TitleFont = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea1.Name = "ChartArea1";
            this.chartAlphat.ChartAreas.Add(chartArea1);
            this.chartAlphat.Location = new System.Drawing.Point(814, 28);
            this.chartAlphat.Margin = new System.Windows.Forms.Padding(2);
            this.chartAlphat.Name = "chartAlphat";
            series1.BorderWidth = 3;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Name = "Series1";
            this.chartAlphat.Series.Add(series1);
            this.chartAlphat.Size = new System.Drawing.Size(832, 402);
            this.chartAlphat.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1657, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.открытьToolStripMenuItem_Click);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabControl1.Location = new System.Drawing.Point(814, 435);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(832, 402);
            this.tabControl1.TabIndex = 23;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chartRt);
            this.tabPage3.Location = new System.Drawing.Point(4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(824, 374);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "r(t)";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chartRt
            // 
            chartArea2.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chartArea2.AxisX.Crossing = -1.7976931348623157E+308D;
            chartArea2.AxisX.Minimum = 0D;
            chartArea2.AxisX.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea2.AxisX.Title = "t";
            chartArea2.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea2.AxisX.TitleFont = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea2.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chartArea2.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea2.AxisY.Title = "r";
            chartArea2.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea2.AxisY.TitleFont = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea2.Name = "ChartArea1";
            this.chartRt.ChartAreas.Add(chartArea2);
            this.chartRt.Location = new System.Drawing.Point(0, 3);
            this.chartRt.Name = "chartRt";
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.MarkerSize = 12;
            series2.Name = "Series1";
            this.chartRt.Series.Add(series2);
            this.chartRt.Size = new System.Drawing.Size(818, 368);
            this.chartRt.TabIndex = 1;
            this.chartRt.Text = "chart2";
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            title1.Name = "Title1";
            title1.Text = "Радиальная позиция";
            this.chartRt.Titles.Add(title1);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.chartTt);
            this.tabPage4.Location = new System.Drawing.Point(4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(824, 374);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "Θ(t)";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // chartTt
            // 
            chartArea3.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chartArea3.AxisX.Crossing = -1.7976931348623157E+308D;
            chartArea3.AxisX.Minimum = 0D;
            chartArea3.AxisX.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea3.AxisX.Title = "t";
            chartArea3.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea3.AxisX.TitleFont = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea3.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chartArea3.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea3.AxisY.Title = "θ";
            chartArea3.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea3.AxisY.TitleFont = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea3.Name = "ChartArea1";
            this.chartTt.ChartAreas.Add(chartArea3);
            this.chartTt.Location = new System.Drawing.Point(0, 0);
            this.chartTt.Name = "chartTt";
            series3.BorderWidth = 3;
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.MarkerSize = 12;
            series3.Name = "Series1";
            this.chartTt.Series.Add(series3);
            this.chartTt.Size = new System.Drawing.Size(818, 368);
            this.chartTt.TabIndex = 2;
            this.chartTt.Text = "chart2";
            title2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            title2.Name = "Title1";
            title2.Text = "Угловая позиция";
            this.chartTt.Titles.Add(title2);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.chartUt);
            this.tabPage5.Location = new System.Drawing.Point(4, 4);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(824, 374);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "u(t)";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // chartUt
            // 
            chartArea4.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chartArea4.AxisX.Crossing = -1.7976931348623157E+308D;
            chartArea4.AxisX.Minimum = 0D;
            chartArea4.AxisX.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea4.AxisX.Title = "t";
            chartArea4.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea4.AxisX.TitleFont = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea4.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chartArea4.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea4.AxisY.Title = "u";
            chartArea4.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea4.AxisY.TitleFont = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea4.Name = "ChartArea1";
            this.chartUt.ChartAreas.Add(chartArea4);
            this.chartUt.Location = new System.Drawing.Point(0, 0);
            this.chartUt.Name = "chartUt";
            series4.BorderWidth = 3;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series4.MarkerSize = 12;
            series4.Name = "Series1";
            this.chartUt.Series.Add(series4);
            this.chartUt.Size = new System.Drawing.Size(818, 368);
            this.chartUt.TabIndex = 2;
            this.chartUt.Text = "chart2";
            title3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            title3.Name = "Title1";
            title3.Text = "Радиальная скорость";
            this.chartUt.Titles.Add(title3);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.chartVt);
            this.tabPage6.Location = new System.Drawing.Point(4, 4);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(824, 374);
            this.tabPage6.TabIndex = 3;
            this.tabPage6.Text = "v(t)";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // chartVt
            // 
            chartArea5.AxisX.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chartArea5.AxisX.Crossing = -1.7976931348623157E+308D;
            chartArea5.AxisX.Minimum = 0D;
            chartArea5.AxisX.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea5.AxisX.Title = "t";
            chartArea5.AxisX.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea5.AxisX.TitleFont = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea5.AxisY.ArrowStyle = System.Windows.Forms.DataVisualization.Charting.AxisArrowStyle.Triangle;
            chartArea5.AxisY.TextOrientation = System.Windows.Forms.DataVisualization.Charting.TextOrientation.Horizontal;
            chartArea5.AxisY.Title = "v";
            chartArea5.AxisY.TitleAlignment = System.Drawing.StringAlignment.Far;
            chartArea5.AxisY.TitleFont = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            chartArea5.Name = "ChartArea1";
            this.chartVt.ChartAreas.Add(chartArea5);
            this.chartVt.Location = new System.Drawing.Point(0, 0);
            this.chartVt.Name = "chartVt";
            series5.BorderWidth = 3;
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.MarkerSize = 12;
            series5.Name = "Series1";
            this.chartVt.Series.Add(series5);
            this.chartVt.Size = new System.Drawing.Size(818, 368);
            this.chartVt.TabIndex = 2;
            this.chartVt.Text = "chart2";
            title4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            title4.Name = "Title1";
            title4.Text = "Тангенциальная скорость";
            this.chartVt.Titles.Add(title4);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1657, 859);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.chartAlphat);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Визуализация движения солнечного паруса";
            this.Resize += new System.EventHandler(this.FormMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.chartAlphat)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartRt)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartTt)).EndInit();
            this.tabPage5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartUt)).EndInit();
            this.tabPage6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartVt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialogMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartAlphat;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRt;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartTt;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartUt;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartVt;
    }
}

