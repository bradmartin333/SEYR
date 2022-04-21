namespace SEYR.Session
{
    partial class PatternWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatternWizard));
            this.TLP = new System.Windows.Forms.TableLayoutPanel();
            this.FlowDelta = new System.Windows.Forms.FlowLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.NumPatternDeltaMax = new System.Windows.Forms.NumericUpDown();
            this.FlowInterval = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.ComboPatternInterval = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.NumPatternInterval = new System.Windows.Forms.NumericUpDown();
            this.WizardLabel = new System.Windows.Forms.Label();
            this.PBX = new System.Windows.Forms.PictureBox();
            this.BtnContinue = new System.Windows.Forms.Button();
            this.FlowScore = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.NumPatternScore = new System.Windows.Forms.NumericUpDown();
            this.BtnFindPatterns = new System.Windows.Forms.Button();
            this.RTB = new System.Windows.Forms.RichTextBox();
            this.TLP.SuspendLayout();
            this.FlowDelta.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPatternDeltaMax)).BeginInit();
            this.FlowInterval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPatternInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBX)).BeginInit();
            this.FlowScore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPatternScore)).BeginInit();
            this.SuspendLayout();
            // 
            // TLP
            // 
            this.TLP.ColumnCount = 2;
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.TLP.Controls.Add(this.FlowDelta, 0, 4);
            this.TLP.Controls.Add(this.FlowInterval, 0, 3);
            this.TLP.Controls.Add(this.WizardLabel, 0, 0);
            this.TLP.Controls.Add(this.PBX, 0, 1);
            this.TLP.Controls.Add(this.BtnContinue, 0, 5);
            this.TLP.Controls.Add(this.FlowScore, 0, 2);
            this.TLP.Controls.Add(this.RTB, 1, 1);
            this.TLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLP.Location = new System.Drawing.Point(0, 0);
            this.TLP.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.TLP.Name = "TLP";
            this.TLP.RowCount = 7;
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.TLP.Size = new System.Drawing.Size(912, 814);
            this.TLP.TabIndex = 0;
            // 
            // FlowDelta
            // 
            this.FlowDelta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.FlowDelta.AutoSize = true;
            this.FlowDelta.Controls.Add(this.label5);
            this.FlowDelta.Controls.Add(this.NumPatternDeltaMax);
            this.FlowDelta.Location = new System.Drawing.Point(182, 744);
            this.FlowDelta.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FlowDelta.Name = "FlowDelta";
            this.FlowDelta.Size = new System.Drawing.Size(274, 30);
            this.FlowDelta.TabIndex = 6;
            this.FlowDelta.Visible = false;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 7);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(141, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "Pattern Delta Max (µm)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumPatternDeltaMax
            // 
            this.NumPatternDeltaMax.Location = new System.Drawing.Point(153, 4);
            this.NumPatternDeltaMax.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NumPatternDeltaMax.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.NumPatternDeltaMax.Name = "NumPatternDeltaMax";
            this.NumPatternDeltaMax.Size = new System.Drawing.Size(117, 22);
            this.NumPatternDeltaMax.TabIndex = 3;
            this.NumPatternDeltaMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FlowInterval
            // 
            this.FlowInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.FlowInterval.AutoSize = true;
            this.FlowInterval.Controls.Add(this.label2);
            this.FlowInterval.Controls.Add(this.ComboPatternInterval);
            this.FlowInterval.Controls.Add(this.label3);
            this.FlowInterval.Controls.Add(this.NumPatternInterval);
            this.FlowInterval.Location = new System.Drawing.Point(65, 704);
            this.FlowInterval.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FlowInterval.Name = "FlowInterval";
            this.FlowInterval.Size = new System.Drawing.Size(509, 32);
            this.FlowInterval.TabIndex = 4;
            this.FlowInterval.Visible = false;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 8);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Trigger String";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ComboPatternInterval
            // 
            this.ComboPatternInterval.FormattingEnabled = true;
            this.ComboPatternInterval.Location = new System.Drawing.Point(100, 4);
            this.ComboPatternInterval.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ComboPatternInterval.Name = "ComboPatternInterval";
            this.ComboPatternInterval.Size = new System.Drawing.Size(162, 24);
            this.ComboPatternInterval.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(270, 8);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Interval ( 0 = disabled )";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumPatternInterval
            // 
            this.NumPatternInterval.Location = new System.Drawing.Point(418, 4);
            this.NumPatternInterval.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NumPatternInterval.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.NumPatternInterval.Name = "NumPatternInterval";
            this.NumPatternInterval.Size = new System.Drawing.Size(87, 22);
            this.NumPatternInterval.TabIndex = 3;
            this.NumPatternInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // WizardLabel
            // 
            this.WizardLabel.AutoSize = true;
            this.WizardLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WizardLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WizardLabel.Location = new System.Drawing.Point(4, 0);
            this.WizardLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.WizardLabel.Name = "WizardLabel";
            this.WizardLabel.Size = new System.Drawing.Size(631, 37);
            this.WizardLabel.TabIndex = 0;
            // 
            // PBX
            // 
            this.PBX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.PBX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PBX.Location = new System.Drawing.Point(4, 41);
            this.PBX.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PBX.Name = "PBX";
            this.PBX.Size = new System.Drawing.Size(631, 605);
            this.PBX.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBX.TabIndex = 1;
            this.PBX.TabStop = false;
            // 
            // BtnContinue
            // 
            this.BtnContinue.BackColor = System.Drawing.Color.White;
            this.BtnContinue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnContinue.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnContinue.Location = new System.Drawing.Point(4, 782);
            this.BtnContinue.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnContinue.Name = "BtnContinue";
            this.BtnContinue.Size = new System.Drawing.Size(631, 28);
            this.BtnContinue.TabIndex = 2;
            this.BtnContinue.Text = "Continue";
            this.BtnContinue.UseVisualStyleBackColor = false;
            this.BtnContinue.Click += new System.EventHandler(this.BtnContinue_Click);
            // 
            // FlowScore
            // 
            this.FlowScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.FlowScore.AutoSize = true;
            this.FlowScore.Controls.Add(this.label1);
            this.FlowScore.Controls.Add(this.NumPatternScore);
            this.FlowScore.Controls.Add(this.BtnFindPatterns);
            this.FlowScore.Location = new System.Drawing.Point(113, 654);
            this.FlowScore.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.FlowScore.Name = "FlowScore";
            this.FlowScore.Size = new System.Drawing.Size(412, 42);
            this.FlowScore.TabIndex = 3;
            this.FlowScore.Visible = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 13);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pattern Score Threshold";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // NumPatternScore
            // 
            this.NumPatternScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.NumPatternScore.DecimalPlaces = 2;
            this.NumPatternScore.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.NumPatternScore.Location = new System.Drawing.Point(164, 10);
            this.NumPatternScore.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.NumPatternScore.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            131072});
            this.NumPatternScore.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.NumPatternScore.Name = "NumPatternScore";
            this.NumPatternScore.Size = new System.Drawing.Size(107, 22);
            this.NumPatternScore.TabIndex = 1;
            this.NumPatternScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumPatternScore.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // BtnFindPatterns
            // 
            this.BtnFindPatterns.AutoSize = true;
            this.BtnFindPatterns.BackColor = System.Drawing.Color.White;
            this.BtnFindPatterns.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFindPatterns.Location = new System.Drawing.Point(279, 4);
            this.BtnFindPatterns.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.BtnFindPatterns.Name = "BtnFindPatterns";
            this.BtnFindPatterns.Size = new System.Drawing.Size(129, 34);
            this.BtnFindPatterns.TabIndex = 2;
            this.BtnFindPatterns.Text = "Find Patterns";
            this.BtnFindPatterns.UseVisualStyleBackColor = false;
            this.BtnFindPatterns.Click += new System.EventHandler(this.BtnFindPatterns_Click);
            // 
            // RTB
            // 
            this.RTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.RTB.Location = new System.Drawing.Point(643, 41);
            this.RTB.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.RTB.Name = "RTB";
            this.RTB.Size = new System.Drawing.Size(265, 605);
            this.RTB.TabIndex = 5;
            this.RTB.Text = "";
            this.RTB.Visible = false;
            // 
            // PatternWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 814);
            this.Controls.Add(this.TLP);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(594, 543);
            this.Name = "PatternWizard";
            this.Text = "Pattern Wizard";
            this.TLP.ResumeLayout(false);
            this.TLP.PerformLayout();
            this.FlowDelta.ResumeLayout(false);
            this.FlowDelta.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPatternDeltaMax)).EndInit();
            this.FlowInterval.ResumeLayout(false);
            this.FlowInterval.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPatternInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PBX)).EndInit();
            this.FlowScore.ResumeLayout(false);
            this.FlowScore.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPatternScore)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TLP;
        private System.Windows.Forms.Label WizardLabel;
        private System.Windows.Forms.PictureBox PBX;
        private System.Windows.Forms.Button BtnContinue;
        private System.Windows.Forms.FlowLayoutPanel FlowScore;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NumPatternScore;
        private System.Windows.Forms.Button BtnFindPatterns;
        private System.Windows.Forms.FlowLayoutPanel FlowInterval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox ComboPatternInterval;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown NumPatternInterval;
        private System.Windows.Forms.RichTextBox RTB;
        private System.Windows.Forms.FlowLayoutPanel FlowDelta;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown NumPatternDeltaMax;
    }
}