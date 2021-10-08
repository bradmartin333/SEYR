namespace SEYR
{
    partial class PixelPitch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PixelPitch));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtFrameWidth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSEYRWidth = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblRatio = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.txtFrameWidth, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.label2, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.lblSEYRWidth, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.label4, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.lblRatio, 2, 3);
            this.tableLayoutPanel.Controls.Add(this.label6, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.label7, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.label3, 3, 3);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 5;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(315, 121);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "Width of Current Frame";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFrameWidth
            // 
            this.txtFrameWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFrameWidth.Location = new System.Drawing.Point(138, 18);
            this.txtFrameWidth.Name = "txtFrameWidth";
            this.txtFrameWidth.Size = new System.Drawing.Size(100, 20);
            this.txtFrameWidth.TabIndex = 1;
            this.txtFrameWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtFrameWidth.TextChanged += new System.EventHandler(this.txtFrameWidth_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(16, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "Width of SEYR Frame";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSEYRWidth
            // 
            this.lblSEYRWidth.AutoSize = true;
            this.lblSEYRWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSEYRWidth.Location = new System.Drawing.Point(138, 45);
            this.lblSEYRWidth.Name = "lblSEYRWidth";
            this.lblSEYRWidth.Size = new System.Drawing.Size(100, 30);
            this.lblSEYRWidth.TabIndex = 3;
            this.lblSEYRWidth.Text = "N/A";
            this.lblSEYRWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(16, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 30);
            this.label4.TabIndex = 4;
            this.label4.Text = "Ratio";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRatio
            // 
            this.lblRatio.AutoSize = true;
            this.lblRatio.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblRatio.Location = new System.Drawing.Point(138, 75);
            this.lblRatio.Name = "lblRatio";
            this.lblRatio.Size = new System.Drawing.Size(100, 30);
            this.lblRatio.TabIndex = 5;
            this.lblRatio.Text = "N/A";
            this.lblRatio.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(244, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 30);
            this.label6.TabIndex = 6;
            this.label6.Text = "mm";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(244, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 30);
            this.label7.TabIndex = 7;
            this.label7.Text = "pixels";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(244, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 30);
            this.label3.TabIndex = 8;
            this.label3.Text = "mm / pixel";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PixelPitch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 121);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PixelPitch";
            this.Text = "Pixel Pitch Calculator";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtFrameWidth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSEYRWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblRatio;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
    }
}