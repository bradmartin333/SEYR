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
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPatternFollowInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.numPatternFollowInterval, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnRunAll, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenGIF, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenComposer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.numFrame, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
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
            this.numPatternFollowInterval.Location = new System.Drawing.Point(342, 32);
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
            this.numPatternFollowInterval.Size = new System.Drawing.Size(109, 20);
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
            this.btnRunAll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRunAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRunAll.Location = new System.Drawing.Point(342, 3);
            this.btnRunAll.Name = "btnRunAll";
            this.btnRunAll.Size = new System.Drawing.Size(109, 23);
            this.btnRunAll.TabIndex = 4;
            this.btnRunAll.Text = "Run All";
            this.btnRunAll.UseVisualStyleBackColor = true;
            this.btnRunAll.Click += new System.EventHandler(this.btnRunAll_Click);
            // 
            // btnOpenGIF
            // 
            this.btnOpenGIF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenGIF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenGIF.Location = new System.Drawing.Point(3, 3);
            this.btnOpenGIF.Name = "btnOpenGIF";
            this.btnOpenGIF.Size = new System.Drawing.Size(107, 23);
            this.btnOpenGIF.TabIndex = 0;
            this.btnOpenGIF.Text = "Open GIF";
            this.btnOpenGIF.UseVisualStyleBackColor = true;
            this.btnOpenGIF.Click += new System.EventHandler(this.btnOpenGIF_Click);
            // 
            // btnOpenComposer
            // 
            this.btnOpenComposer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenComposer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenComposer.Location = new System.Drawing.Point(116, 3);
            this.btnOpenComposer.Name = "btnOpenComposer";
            this.btnOpenComposer.Size = new System.Drawing.Size(107, 23);
            this.btnOpenComposer.TabIndex = 1;
            this.btnOpenComposer.Text = "Open Composer";
            this.btnOpenComposer.UseVisualStyleBackColor = true;
            this.btnOpenComposer.Click += new System.EventHandler(this.btnOpenComposer_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox, 4);
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
            this.numFrame.Location = new System.Drawing.Point(229, 4);
            this.numFrame.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numFrame.Name = "numFrame";
            this.numFrame.Size = new System.Drawing.Size(107, 20);
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
            this.label1.Location = new System.Drawing.Point(116, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 26);
            this.label1.TabIndex = 6;
            this.label1.Text = "Pattern Follow Interval";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
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
    }
}

