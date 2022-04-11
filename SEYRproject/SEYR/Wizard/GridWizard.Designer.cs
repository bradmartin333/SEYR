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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FiltersWizard));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnConfirm = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.ButtonToggleGrid = new System.Windows.Forms.Button();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.LabelScale = new System.Windows.Forms.Label();
            this.NumScaling = new System.Windows.Forms.NumericUpDown();
            this.LabelThreshold = new System.Windows.Forms.Label();
            this.NumThreshold = new System.Windows.Forms.NumericUpDown();
            this.LabelAngle = new System.Windows.Forms.Label();
            this.NumAngle = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumScaling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumAngle)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.PictureBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(484, 461);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Controls.Add(this.BtnConfirm);
            this.flowLayoutPanel4.Controls.Add(this.BtnCancel);
            this.flowLayoutPanel4.Controls.Add(this.ButtonToggleGrid);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(120, 429);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(243, 29);
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
            // 
            // ButtonToggleGrid
            // 
            this.ButtonToggleGrid.BackColor = System.Drawing.Color.White;
            this.ButtonToggleGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonToggleGrid.Location = new System.Drawing.Point(165, 3);
            this.ButtonToggleGrid.Name = "ButtonToggleGrid";
            this.ButtonToggleGrid.Size = new System.Drawing.Size(75, 23);
            this.ButtonToggleGrid.TabIndex = 4;
            this.ButtonToggleGrid.Text = "Toggle Grid";
            this.ButtonToggleGrid.UseVisualStyleBackColor = false;
            // 
            // PictureBox
            // 
            this.PictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureBox.Location = new System.Drawing.Point(3, 3);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(478, 388);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox.TabIndex = 0;
            this.PictureBox.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.LabelScale);
            this.flowLayoutPanel1.Controls.Add(this.NumScaling);
            this.flowLayoutPanel1.Controls.Add(this.LabelThreshold);
            this.flowLayoutPanel1.Controls.Add(this.NumThreshold);
            this.flowLayoutPanel1.Controls.Add(this.LabelAngle);
            this.flowLayoutPanel1.Controls.Add(this.NumAngle);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(14, 397);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(456, 26);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // LabelScale
            // 
            this.LabelScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelScale.Location = new System.Drawing.Point(3, 6);
            this.LabelScale.Name = "LabelScale";
            this.LabelScale.Size = new System.Drawing.Size(70, 13);
            this.LabelScale.TabIndex = 2;
            this.LabelScale.Text = "Scaling";
            this.LabelScale.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumScaling
            // 
            this.NumScaling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumScaling.DecimalPlaces = 2;
            this.NumScaling.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumScaling.Location = new System.Drawing.Point(79, 3);
            this.NumScaling.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumScaling.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumScaling.Name = "NumScaling";
            this.NumScaling.Size = new System.Drawing.Size(70, 20);
            this.NumScaling.TabIndex = 3;
            this.NumScaling.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumScaling.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            // 
            // LabelThreshold
            // 
            this.LabelThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelThreshold.Location = new System.Drawing.Point(155, 6);
            this.LabelThreshold.Name = "LabelThreshold";
            this.LabelThreshold.Size = new System.Drawing.Size(70, 13);
            this.LabelThreshold.TabIndex = 0;
            this.LabelThreshold.Text = "Threshold";
            this.LabelThreshold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumThreshold
            // 
            this.NumThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumThreshold.Location = new System.Drawing.Point(231, 3);
            this.NumThreshold.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumThreshold.Name = "NumThreshold";
            this.NumThreshold.Size = new System.Drawing.Size(70, 20);
            this.NumThreshold.TabIndex = 1;
            this.NumThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LabelAngle
            // 
            this.LabelAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelAngle.Location = new System.Drawing.Point(307, 6);
            this.LabelAngle.Name = "LabelAngle";
            this.LabelAngle.Size = new System.Drawing.Size(70, 13);
            this.LabelAngle.TabIndex = 4;
            this.LabelAngle.Text = "Angle";
            this.LabelAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NumAngle
            // 
            this.NumAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumAngle.DecimalPlaces = 2;
            this.NumAngle.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumAngle.Location = new System.Drawing.Point(383, 3);
            this.NumAngle.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.NumAngle.Name = "NumAngle";
            this.NumAngle.Size = new System.Drawing.Size(70, 20);
            this.NumAngle.TabIndex = 5;
            this.NumAngle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FiltersWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "FiltersWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Filters";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NumScaling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumAngle)).EndInit();
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
        private System.Windows.Forms.NumericUpDown NumThreshold;
        private System.Windows.Forms.Label LabelScale;
        private System.Windows.Forms.NumericUpDown NumScaling;
        private System.Windows.Forms.Label LabelAngle;
        private System.Windows.Forms.NumericUpDown NumAngle;
        private System.Windows.Forms.Button ButtonToggleGrid;
    }
}