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
            this.btnOpenGIF = new System.Windows.Forms.Button();
            this.btnOpenComposer = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.numFrame = new System.Windows.Forms.NumericUpDown();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFrame)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btnOpenGIF, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenComposer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.numFrame, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 433);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnOpenGIF
            // 
            this.btnOpenGIF.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenGIF.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenGIF.Location = new System.Drawing.Point(3, 3);
            this.btnOpenGIF.Name = "btnOpenGIF";
            this.btnOpenGIF.Size = new System.Drawing.Size(145, 23);
            this.btnOpenGIF.TabIndex = 0;
            this.btnOpenGIF.Text = "Open GIF";
            this.btnOpenGIF.UseVisualStyleBackColor = true;
            this.btnOpenGIF.Click += new System.EventHandler(this.btnOpenGIF_Click);
            // 
            // btnOpenComposer
            // 
            this.btnOpenComposer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenComposer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenComposer.Location = new System.Drawing.Point(154, 3);
            this.btnOpenComposer.Name = "btnOpenComposer";
            this.btnOpenComposer.Size = new System.Drawing.Size(145, 23);
            this.btnOpenComposer.TabIndex = 1;
            this.btnOpenComposer.Text = "Open Composer";
            this.btnOpenComposer.UseVisualStyleBackColor = true;
            this.btnOpenComposer.Click += new System.EventHandler(this.btnOpenComposer_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanel1.SetColumnSpan(this.pictureBox, 3);
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(3, 32);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(448, 398);
            this.pictureBox.TabIndex = 2;
            this.pictureBox.TabStop = false;
            // 
            // numFrame
            // 
            this.numFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numFrame.Location = new System.Drawing.Point(305, 4);
            this.numFrame.Name = "numFrame";
            this.numFrame.Size = new System.Drawing.Size(146, 20);
            this.numFrame.TabIndex = 3;
            this.numFrame.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numFrame.ValueChanged += new System.EventHandler(this.numFrame_ValueChanged);
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
    }
}

