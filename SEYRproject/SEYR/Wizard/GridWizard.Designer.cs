namespace SEYR.Wizard
{
    partial class GridWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridWizard));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.NumPitchX = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.NumPitchY = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.NumOriginX = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.NumOriginY = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnConfirm = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.LabelScale = new System.Windows.Forms.Label();
            this.NumRows = new System.Windows.Forms.NumericUpDown();
            this.LabelThreshold = new System.Windows.Forms.Label();
            this.NumColumns = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPitchX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumPitchY)).BeginInit();
            this.flowLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumOriginX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOriginY)).BeginInit();
            this.flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumColumns)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.PictureBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 3);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 461);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.label3);
            this.flowLayoutPanel3.Controls.Add(this.NumPitchX);
            this.flowLayoutPanel3.Controls.Add(this.label4);
            this.flowLayoutPanel3.Controls.Add(this.NumPitchY);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(90, 365);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(304, 26);
            this.flowLayoutPanel3.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(3, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Pitch X (μm)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumPitchX
            // 
            this.NumPitchX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumPitchX.Location = new System.Drawing.Point(79, 3);
            this.NumPitchX.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.NumPitchX.Name = "NumPitchX";
            this.NumPitchX.Size = new System.Drawing.Size(70, 20);
            this.NumPitchX.TabIndex = 3;
            this.NumPitchX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumPitchX.ValueChanged += new System.EventHandler(this.NumPitchX_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(155, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Pitch Y (μm)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumPitchY
            // 
            this.NumPitchY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumPitchY.Location = new System.Drawing.Point(231, 3);
            this.NumPitchY.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.NumPitchY.Name = "NumPitchY";
            this.NumPitchY.Size = new System.Drawing.Size(70, 20);
            this.NumPitchY.TabIndex = 1;
            this.NumPitchY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumPitchY.ValueChanged += new System.EventHandler(this.NumPitchY_ValueChanged);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.label1);
            this.flowLayoutPanel2.Controls.Add(this.NumOriginX);
            this.flowLayoutPanel2.Controls.Add(this.label2);
            this.flowLayoutPanel2.Controls.Add(this.NumOriginY);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(90, 333);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(304, 26);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Origin X (μm)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumOriginX
            // 
            this.NumOriginX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumOriginX.Location = new System.Drawing.Point(79, 3);
            this.NumOriginX.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.NumOriginX.Name = "NumOriginX";
            this.NumOriginX.Size = new System.Drawing.Size(70, 20);
            this.NumOriginX.TabIndex = 3;
            this.NumOriginX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumOriginX.ValueChanged += new System.EventHandler(this.NumOriginX_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Location = new System.Drawing.Point(155, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Origin Y (μm)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumOriginY
            // 
            this.NumOriginY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumOriginY.Location = new System.Drawing.Point(231, 3);
            this.NumOriginY.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.NumOriginY.Name = "NumOriginY";
            this.NumOriginY.Size = new System.Drawing.Size(70, 20);
            this.NumOriginY.TabIndex = 1;
            this.NumOriginY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumOriginY.ValueChanged += new System.EventHandler(this.NumOriginY_ValueChanged);
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Controls.Add(this.BtnConfirm);
            this.flowLayoutPanel4.Controls.Add(this.BtnCancel);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(161, 429);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(162, 29);
            this.flowLayoutPanel4.TabIndex = 4;
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.BackColor = System.Drawing.Color.LightGreen;
            this.BtnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnConfirm.Location = new System.Drawing.Point(3, 3);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(75, 23);
            this.BtnConfirm.TabIndex = 3;
            this.BtnConfirm.Text = "Confirm";
            this.BtnConfirm.UseVisualStyleBackColor = false;
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.Color.LightCoral;
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancel.Location = new System.Drawing.Point(84, 3);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // PictureBox
            // 
            this.PictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBox.Location = new System.Drawing.Point(3, 3);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(478, 324);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox.TabIndex = 0;
            this.PictureBox.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.LabelScale);
            this.flowLayoutPanel1.Controls.Add(this.NumRows);
            this.flowLayoutPanel1.Controls.Add(this.LabelThreshold);
            this.flowLayoutPanel1.Controls.Add(this.NumColumns);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(90, 397);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(304, 26);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // LabelScale
            // 
            this.LabelScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelScale.Location = new System.Drawing.Point(3, 6);
            this.LabelScale.Name = "LabelScale";
            this.LabelScale.Size = new System.Drawing.Size(70, 13);
            this.LabelScale.TabIndex = 2;
            this.LabelScale.Text = "Rows";
            this.LabelScale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumRows
            // 
            this.NumRows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumRows.Location = new System.Drawing.Point(79, 3);
            this.NumRows.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.NumRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumRows.Name = "NumRows";
            this.NumRows.Size = new System.Drawing.Size(70, 20);
            this.NumRows.TabIndex = 3;
            this.NumRows.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumRows.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumRows.ValueChanged += new System.EventHandler(this.NumRows_ValueChanged);
            // 
            // LabelThreshold
            // 
            this.LabelThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelThreshold.Location = new System.Drawing.Point(155, 6);
            this.LabelThreshold.Name = "LabelThreshold";
            this.LabelThreshold.Size = new System.Drawing.Size(70, 13);
            this.LabelThreshold.TabIndex = 0;
            this.LabelThreshold.Text = "Columns";
            this.LabelThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumColumns
            // 
            this.NumColumns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumColumns.Location = new System.Drawing.Point(231, 3);
            this.NumColumns.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.NumColumns.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumColumns.Name = "NumColumns";
            this.NumColumns.Size = new System.Drawing.Size(70, 20);
            this.NumColumns.TabIndex = 1;
            this.NumColumns.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumColumns.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumColumns.ValueChanged += new System.EventHandler(this.NumColumns_ValueChanged);
            // 
            // GridWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "GridWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Filters";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumPitchX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumPitchY)).EndInit();
            this.flowLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumOriginX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumOriginY)).EndInit();
            this.flowLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumColumns)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox PictureBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.Button BtnConfirm;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label LabelThreshold;
        private System.Windows.Forms.NumericUpDown NumColumns;
        private System.Windows.Forms.Label LabelScale;
        private System.Windows.Forms.NumericUpDown NumRows;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NumPitchX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown NumPitchY;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NumOriginX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NumOriginY;
    }
}