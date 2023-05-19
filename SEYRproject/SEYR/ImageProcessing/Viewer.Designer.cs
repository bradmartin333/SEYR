namespace SEYR.ImageProcessing
{
    partial class Viewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Viewer));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.TLP = new System.Windows.Forms.TableLayoutPanel();
            this.PBX = new System.Windows.Forms.PictureBox();
            this.InfoLabel = new System.Windows.Forms.Label();
            this.BtnShowData = new System.Windows.Forms.Button();
            this.TLPFeatureData = new System.Windows.Forms.TableLayoutPanel();
            this.ComboFeatureSelector = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LblFailingNullCount = new System.Windows.Forms.Label();
            this.PanelChart = new System.Windows.Forms.Panel();
            this.ChartFeatureData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.TLP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBX)).BeginInit();
            this.TLPFeatureData.SuspendLayout();
            this.PanelChart.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChartFeatureData)).BeginInit();
            this.SuspendLayout();
            // 
            // TLP
            // 
            this.TLP.ColumnCount = 2;
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TLP.Controls.Add(this.PBX, 0, 0);
            this.TLP.Controls.Add(this.InfoLabel, 0, 1);
            this.TLP.Controls.Add(this.BtnShowData, 1, 1);
            this.TLP.Controls.Add(this.TLPFeatureData, 0, 2);
            this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP.Location = new System.Drawing.Point(0, 0);
            this.TLP.Name = "TLP";
            this.TLP.RowCount = 3;
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 225F));
            this.TLP.Size = new System.Drawing.Size(361, 336);
            this.TLP.TabIndex = 1;
            // 
            // PBX
            // 
            this.PBX.BackColor = System.Drawing.Color.Black;
            this.PBX.BackgroundImage = global::SEYR.Properties.Resources.SEYR;
            this.PBX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.TLP.SetColumnSpan(this.PBX, 2);
            this.PBX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PBX.Location = new System.Drawing.Point(3, 3);
            this.PBX.Name = "PBX";
            this.PBX.Size = new System.Drawing.Size(355, 85);
            this.PBX.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBX.TabIndex = 0;
            this.PBX.TabStop = false;
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.BackColor = System.Drawing.Color.Silver;
            this.InfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InfoLabel.Location = new System.Drawing.Point(3, 91);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Size = new System.Drawing.Size(335, 20);
            this.InfoLabel.TabIndex = 1;
            this.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // BtnShowData
            // 
            this.BtnShowData.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnShowData.BackgroundImage")));
            this.BtnShowData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnShowData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnShowData.FlatAppearance.BorderSize = 0;
            this.BtnShowData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnShowData.Location = new System.Drawing.Point(341, 91);
            this.BtnShowData.Margin = new System.Windows.Forms.Padding(0);
            this.BtnShowData.Name = "BtnShowData";
            this.BtnShowData.Size = new System.Drawing.Size(20, 20);
            this.BtnShowData.TabIndex = 2;
            this.BtnShowData.UseVisualStyleBackColor = true;
            this.BtnShowData.Click += new System.EventHandler(this.BtnShowData_Click);
            // 
            // TLPFeatureData
            // 
            this.TLPFeatureData.ColumnCount = 4;
            this.TLP.SetColumnSpan(this.TLPFeatureData, 2);
            this.TLPFeatureData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TLPFeatureData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLPFeatureData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TLPFeatureData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.TLPFeatureData.Controls.Add(this.ComboFeatureSelector, 1, 1);
            this.TLPFeatureData.Controls.Add(this.label1, 0, 1);
            this.TLPFeatureData.Controls.Add(this.label2, 2, 1);
            this.TLPFeatureData.Controls.Add(this.LblFailingNullCount, 3, 1);
            this.TLPFeatureData.Controls.Add(this.PanelChart, 0, 0);
            this.TLPFeatureData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLPFeatureData.Location = new System.Drawing.Point(3, 114);
            this.TLPFeatureData.Name = "TLPFeatureData";
            this.TLPFeatureData.RowCount = 2;
            this.TLPFeatureData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLPFeatureData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLPFeatureData.Size = new System.Drawing.Size(355, 219);
            this.TLPFeatureData.TabIndex = 3;
            // 
            // ComboFeatureSelector
            // 
            this.ComboFeatureSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComboFeatureSelector.FormattingEnabled = true;
            this.ComboFeatureSelector.Location = new System.Drawing.Point(97, 195);
            this.ComboFeatureSelector.Name = "ComboFeatureSelector";
            this.ComboFeatureSelector.Size = new System.Drawing.Size(60, 21);
            this.ComboFeatureSelector.TabIndex = 0;
            this.ComboFeatureSelector.SelectedIndexChanged += new System.EventHandler(this.ComboFeatureSelector_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "Selected Feature";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(163, 192);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 27);
            this.label2.TabIndex = 3;
            this.label2.Text = "Failing Null Count";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LblFailingNullCount
            // 
            this.LblFailingNullCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LblFailingNullCount.AutoSize = true;
            this.LblFailingNullCount.Location = new System.Drawing.Point(258, 192);
            this.LblFailingNullCount.Name = "LblFailingNullCount";
            this.LblFailingNullCount.Size = new System.Drawing.Size(13, 27);
            this.LblFailingNullCount.TabIndex = 4;
            this.LblFailingNullCount.Text = "0";
            this.LblFailingNullCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PanelChart
            // 
            this.PanelChart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.TLPFeatureData.SetColumnSpan(this.PanelChart, 4);
            this.PanelChart.Controls.Add(this.ChartFeatureData);
            this.PanelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelChart.Location = new System.Drawing.Point(3, 3);
            this.PanelChart.Name = "PanelChart";
            this.PanelChart.Size = new System.Drawing.Size(349, 186);
            this.PanelChart.TabIndex = 5;
            // 
            // ChartFeatureData
            // 
            this.ChartFeatureData.BackColor = System.Drawing.Color.Transparent;
            chartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea1.AxisX.LabelStyle.Interval = 0D;
            chartArea1.AxisX.LabelStyle.IntervalOffset = 0D;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Number;
            chartArea1.AxisX.MajorGrid.Enabled = false;
            chartArea1.AxisX.Title = "Score";
            chartArea1.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
            chartArea1.AxisY.MajorGrid.Enabled = false;
            chartArea1.AxisY.MajorTickMark.Enabled = false;
            chartArea1.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea1.BackColor = System.Drawing.Color.Transparent;
            chartArea1.Name = "ChartArea1";
            this.ChartFeatureData.ChartAreas.Add(chartArea1);
            this.ChartFeatureData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartFeatureData.Location = new System.Drawing.Point(0, 0);
            this.ChartFeatureData.Name = "ChartFeatureData";
            series1.ChartArea = "ChartArea1";
            series1.Color = System.Drawing.Color.Black;
            series1.Name = "Series1";
            this.ChartFeatureData.Series.Add(series1);
            this.ChartFeatureData.Size = new System.Drawing.Size(349, 186);
            this.ChartFeatureData.TabIndex = 1;
            this.ChartFeatureData.Text = "chart1";
            title1.Name = "ChartTitle";
            title1.Text = "Count in Last X Points";
            this.ChartFeatureData.Titles.Add(title1);
            // 
            // Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(361, 336);
            this.Controls.Add(this.TLP);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(375, 375);
            this.Name = "Viewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Viewer";
            this.Load += new System.EventHandler(this.Viewer_Load);
            this.TLP.ResumeLayout(false);
            this.TLP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBX)).EndInit();
            this.TLPFeatureData.ResumeLayout(false);
            this.TLPFeatureData.PerformLayout();
            this.PanelChart.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChartFeatureData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PBX;
        private System.Windows.Forms.TableLayoutPanel TLP;
        public System.Windows.Forms.Label InfoLabel;
        private System.Windows.Forms.Button BtnShowData;
        private System.Windows.Forms.TableLayoutPanel TLPFeatureData;
        private System.Windows.Forms.ComboBox ComboFeatureSelector;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartFeatureData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LblFailingNullCount;
        private System.Windows.Forms.Panel PanelChart;
    }
}