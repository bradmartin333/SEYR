namespace SEYRDesktop
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numPatternFollowInterval = new System.Windows.Forms.NumericUpDown();
            this.btnRunAll = new System.Windows.Forms.Button();
            this.btnOpenGIF = new System.Windows.Forms.Button();
            this.btnOpenComposer = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.numPatternFollowDelay = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numImageScale = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPatternFollowInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatternFollowDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numImageScale)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 8;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
            this.tableLayoutPanel1.Controls.Add(this.btnRunAll, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenGIF, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenComposer, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.numFrame, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.numPatternFollowInterval, 7, 1);
            this.tableLayoutPanel1.Controls.Add(this.numPatternFollowDelay, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.numImageScale, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 4, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 433);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // numPatternFollowInterval
            // 
            this.numPatternFollowInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numPatternFollowInterval.Location = new System.Drawing.Point(395, 32);
            this.numPatternFollowInterval.Maximum = new decimal(new int[] {
            276447232,
            23283,
            0,
            0});
            this.numPatternFollowInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPatternFollowInterval.Name = "numPatternFollowInterval";
            this.numPatternFollowInterval.Size = new System.Drawing.Size(56, 20);
            this.numPatternFollowInterval.TabIndex = 5;
            this.numPatternFollowInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numPatternFollowInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPatternFollowInterval.ValueChanged += new System.EventHandler(this.numPatternFollowInterval_ValueChanged);
            // 
            // btnRunAll
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnRunAll, 2);
            this.btnRunAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRunAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunAll.Location = new System.Drawing.Point(339, 3);
            this.btnRunAll.Name = "btnRunAll";
            this.btnRunAll.Size = new System.Drawing.Size(112, 23);
            this.btnRunAll.TabIndex = 4;
            this.btnRunAll.Text = "Run All";
            this.btnRunAll.UseVisualStyleBackColor = true;
            this.btnRunAll.Click += new System.EventHandler(this.btnRunAll_Click);
            // 
            // btnOpenGIF
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnOpenGIF, 2);
            this.btnOpenGIF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenGIF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenGIF.Location = new System.Drawing.Point(3, 3);
            this.btnOpenGIF.Name = "btnOpenGIF";
            this.btnOpenGIF.Size = new System.Drawing.Size(106, 23);
            this.btnOpenGIF.TabIndex = 0;
            this.btnOpenGIF.Text = "Open GIF";
            this.btnOpenGIF.UseVisualStyleBackColor = true;
            this.btnOpenGIF.Click += new System.EventHandler(this.btnOpenGIF_Click);
            // 
            // btnOpenComposer
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.btnOpenComposer, 2);
            this.btnOpenComposer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenComposer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenComposer.Location = new System.Drawing.Point(115, 3);
            this.btnOpenComposer.Name = "btnOpenComposer";
            this.btnOpenComposer.Size = new System.Drawing.Size(106, 23);
            this.btnOpenComposer.TabIndex = 1;
            this.btnOpenComposer.Text = "Open Composer";
            this.btnOpenComposer.UseVisualStyleBackColor = true;
            this.btnOpenComposer.Click += new System.EventHandler(this.btnOpenComposer_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox, 8);
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 58);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(448, 372);
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // numFrame
            // 
            this.numFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numFrame.Location = new System.Drawing.Point(283, 4);
            this.numFrame.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrame.Name = "numFrame";
            this.numFrame.Size = new System.Drawing.Size(50, 20);
            this.numFrame.TabIndex = 3;
            this.numFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numFrame.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrame.ValueChanged += new System.EventHandler(this.numFrame_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(283, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 26);
            this.label1.TabIndex = 6;
            this.label1.Text = "Follow Interval";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label2, 2);
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(115, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 26);
            this.label2.TabIndex = 7;
            this.label2.Text = "Follow Delay";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numPatternFollowDelay
            // 
            this.numPatternFollowDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numPatternFollowDelay.Location = new System.Drawing.Point(227, 32);
            this.numPatternFollowDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPatternFollowDelay.Name = "numPatternFollowDelay";
            this.numPatternFollowDelay.Size = new System.Drawing.Size(50, 20);
            this.numPatternFollowDelay.TabIndex = 8;
            this.numPatternFollowDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numPatternFollowDelay.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numPatternFollowDelay.ValueChanged += new System.EventHandler(this.numPatternFollowDelay_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 26);
            this.label3.TabIndex = 9;
            this.label3.Text = "Scale";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // numImageScale
            // 
            this.numImageScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numImageScale.DecimalPlaces = 2;
            this.numImageScale.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.numImageScale.Location = new System.Drawing.Point(59, 32);
            this.numImageScale.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.numImageScale.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numImageScale.Name = "numImageScale";
            this.numImageScale.Size = new System.Drawing.Size(50, 20);
            this.numImageScale.TabIndex = 10;
            this.numImageScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numImageScale.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.numImageScale.ValueChanged += new System.EventHandler(this.numImageScale_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(227, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 29);
            this.label4.TabIndex = 11;
            this.label4.Text = "Frame #";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 433);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "SEYR Desktop";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPatternFollowInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPatternFollowDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numImageScale)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnOpenGIF;
        private System.Windows.Forms.Button btnOpenComposer;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.NumericUpDown numFrame;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnRunAll;
        private System.Windows.Forms.NumericUpDown numPatternFollowInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numPatternFollowDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numImageScale;
        private System.Windows.Forms.Label label4;
    }
}

