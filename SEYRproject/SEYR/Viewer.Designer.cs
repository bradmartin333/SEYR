namespace SEYR
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pbxMain = new System.Windows.Forms.PictureBox();
            this.pbxE = new System.Windows.Forms.PictureBox();
            this.pbxD = new System.Windows.Forms.PictureBox();
            this.pbxC = new System.Windows.Forms.PictureBox();
            this.pbxB = new System.Windows.Forms.PictureBox();
            this.pbxA = new System.Windows.Forms.PictureBox();
            this.btnClearViewer = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxA)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 5;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.Controls.Add(this.pbxMain, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.pbxE, 4, 0);
            this.tableLayoutPanel.Controls.Add(this.pbxD, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.pbxC, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.pbxB, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.pbxA, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.btnClearViewer, 1, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(501, 544);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // pbxMain
            // 
            this.pbxMain.BackColor = System.Drawing.Color.DimGray;
            this.tableLayoutPanel.SetColumnSpan(this.pbxMain, 5);
            this.pbxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxMain.Location = new System.Drawing.Point(3, 106);
            this.pbxMain.Name = "pbxMain";
            this.pbxMain.Size = new System.Drawing.Size(495, 406);
            this.pbxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxMain.TabIndex = 5;
            this.pbxMain.TabStop = false;
            this.pbxMain.Tag = "0";
            // 
            // pbxE
            // 
            this.pbxE.BackColor = System.Drawing.Color.DimGray;
            this.pbxE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxE.Location = new System.Drawing.Point(403, 3);
            this.pbxE.Name = "pbxE";
            this.pbxE.Size = new System.Drawing.Size(95, 97);
            this.pbxE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxE.TabIndex = 4;
            this.pbxE.TabStop = false;
            this.pbxE.Tag = "5";
            // 
            // pbxD
            // 
            this.pbxD.BackColor = System.Drawing.Color.DimGray;
            this.pbxD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxD.Location = new System.Drawing.Point(303, 3);
            this.pbxD.Name = "pbxD";
            this.pbxD.Size = new System.Drawing.Size(94, 97);
            this.pbxD.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxD.TabIndex = 3;
            this.pbxD.TabStop = false;
            this.pbxD.Tag = "4";
            // 
            // pbxC
            // 
            this.pbxC.BackColor = System.Drawing.Color.DimGray;
            this.pbxC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxC.Location = new System.Drawing.Point(203, 3);
            this.pbxC.Name = "pbxC";
            this.pbxC.Size = new System.Drawing.Size(94, 97);
            this.pbxC.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxC.TabIndex = 2;
            this.pbxC.TabStop = false;
            this.pbxC.Tag = "3";
            // 
            // pbxB
            // 
            this.pbxB.BackColor = System.Drawing.Color.DimGray;
            this.pbxB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxB.Location = new System.Drawing.Point(103, 3);
            this.pbxB.Name = "pbxB";
            this.pbxB.Size = new System.Drawing.Size(94, 97);
            this.pbxB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxB.TabIndex = 1;
            this.pbxB.TabStop = false;
            this.pbxB.Tag = "2";
            // 
            // pbxA
            // 
            this.pbxA.BackColor = System.Drawing.Color.DimGray;
            this.pbxA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxA.Location = new System.Drawing.Point(3, 3);
            this.pbxA.Name = "pbxA";
            this.pbxA.Size = new System.Drawing.Size(94, 97);
            this.pbxA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxA.TabIndex = 0;
            this.pbxA.TabStop = false;
            this.pbxA.Tag = "1";
            // 
            // btnClearViewer
            // 
            this.btnClearViewer.BackColor = System.Drawing.Color.Black;
            this.tableLayoutPanel.SetColumnSpan(this.btnClearViewer, 3);
            this.btnClearViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClearViewer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.btnClearViewer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
            this.btnClearViewer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearViewer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearViewer.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnClearViewer.Location = new System.Drawing.Point(103, 518);
            this.btnClearViewer.Name = "btnClearViewer";
            this.btnClearViewer.Size = new System.Drawing.Size(294, 23);
            this.btnClearViewer.TabIndex = 6;
            this.btnClearViewer.Text = "Clear All Images";
            this.btnClearViewer.UseVisualStyleBackColor = false;
            this.btnClearViewer.Click += new System.EventHandler(this.btnClearViewer_Click);
            // 
            // Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(501, 544);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Viewer";
            this.Text = "SEYR Viewer";
            this.tableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox pbxMain;
        private System.Windows.Forms.PictureBox pbxE;
        private System.Windows.Forms.PictureBox pbxD;
        private System.Windows.Forms.PictureBox pbxC;
        private System.Windows.Forms.PictureBox pbxB;
        private System.Windows.Forms.PictureBox pbxA;
        private System.Windows.Forms.Button btnClearViewer;
    }
}