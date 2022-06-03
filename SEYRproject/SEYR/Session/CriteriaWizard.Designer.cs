namespace SEYR.Session
{
    partial class CriteriaWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CriteriaWizard));
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.BtnDone = new System.Windows.Forms.Button();
            this.PBX = new System.Windows.Forms.PictureBox();
            this.ComboSelector = new System.Windows.Forms.ComboBox();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.BtnCreateGroup = new System.Windows.Forms.Button();
            this.BtnFinishGroup = new System.Windows.Forms.Button();
            this.TxtGroupName = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBX)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel.Controls.Add(this.BtnDelete, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.BtnDone, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.PBX, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.ComboSelector, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.BtnAdd, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.BtnCreateGroup, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.BtnFinishGroup, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.TxtGroupName, 1, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(484, 461);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // BtnDelete
            // 
            this.BtnDelete.BackColor = System.Drawing.Color.LightCoral;
            this.BtnDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDelete.Location = new System.Drawing.Point(3, 435);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(155, 23);
            this.BtnDelete.TabIndex = 4;
            this.BtnDelete.Text = "Delete";
            this.BtnDelete.UseVisualStyleBackColor = false;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnDone
            // 
            this.BtnDone.BackColor = System.Drawing.Color.LightGreen;
            this.BtnDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnDone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDone.Location = new System.Drawing.Point(325, 435);
            this.BtnDone.Name = "BtnDone";
            this.BtnDone.Size = new System.Drawing.Size(156, 23);
            this.BtnDone.TabIndex = 3;
            this.BtnDone.Text = "Done";
            this.BtnDone.UseVisualStyleBackColor = false;
            this.BtnDone.Click += new System.EventHandler(this.BtnDone_Click);
            // 
            // PBX
            // 
            this.PBX.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tableLayoutPanel.SetColumnSpan(this.PBX, 3);
            this.PBX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PBX.Location = new System.Drawing.Point(3, 30);
            this.PBX.Name = "PBX";
            this.PBX.Size = new System.Drawing.Size(478, 370);
            this.PBX.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBX.TabIndex = 0;
            this.PBX.TabStop = false;
            // 
            // ComboSelector
            // 
            this.tableLayoutPanel.SetColumnSpan(this.ComboSelector, 3);
            this.ComboSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ComboSelector.FormattingEnabled = true;
            this.ComboSelector.Location = new System.Drawing.Point(3, 3);
            this.ComboSelector.Name = "ComboSelector";
            this.ComboSelector.Size = new System.Drawing.Size(478, 21);
            this.ComboSelector.TabIndex = 1;
            this.ComboSelector.SelectedIndexChanged += new System.EventHandler(this.ComboSelector_SelectedIndexChanged);
            // 
            // BtnAdd
            // 
            this.BtnAdd.BackColor = System.Drawing.Color.LightBlue;
            this.BtnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAdd.Location = new System.Drawing.Point(164, 435);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(155, 23);
            this.BtnAdd.TabIndex = 2;
            this.BtnAdd.Text = "Add";
            this.BtnAdd.UseVisualStyleBackColor = false;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnCreateGroup
            // 
            this.BtnCreateGroup.BackColor = System.Drawing.Color.White;
            this.BtnCreateGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnCreateGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCreateGroup.Location = new System.Drawing.Point(3, 406);
            this.BtnCreateGroup.Name = "BtnCreateGroup";
            this.BtnCreateGroup.Size = new System.Drawing.Size(155, 23);
            this.BtnCreateGroup.TabIndex = 5;
            this.BtnCreateGroup.Text = "Create Group";
            this.BtnCreateGroup.UseVisualStyleBackColor = false;
            this.BtnCreateGroup.Click += new System.EventHandler(this.BtnCreateGroup_Click);
            // 
            // BtnFinishGroup
            // 
            this.BtnFinishGroup.BackColor = System.Drawing.Color.White;
            this.BtnFinishGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnFinishGroup.Enabled = false;
            this.BtnFinishGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFinishGroup.Location = new System.Drawing.Point(325, 406);
            this.BtnFinishGroup.Name = "BtnFinishGroup";
            this.BtnFinishGroup.Size = new System.Drawing.Size(156, 23);
            this.BtnFinishGroup.TabIndex = 6;
            this.BtnFinishGroup.Text = "Finish Group";
            this.BtnFinishGroup.UseVisualStyleBackColor = false;
            this.BtnFinishGroup.Click += new System.EventHandler(this.BtnFinishGroup_Click);
            // 
            // TxtGroupName
            // 
            this.TxtGroupName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtGroupName.Enabled = false;
            this.TxtGroupName.Location = new System.Drawing.Point(164, 407);
            this.TxtGroupName.Name = "TxtGroupName";
            this.TxtGroupName.Size = new System.Drawing.Size(155, 20);
            this.TxtGroupName.TabIndex = 7;
            // 
            // CriteriaWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "CriteriaWizard";
            this.Text = "Criteria Wizard";
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.PictureBox PBX;
        private System.Windows.Forms.ComboBox ComboSelector;
        private System.Windows.Forms.Button BtnDone;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Button BtnCreateGroup;
        private System.Windows.Forms.Button BtnFinishGroup;
        private System.Windows.Forms.TextBox TxtGroupName;
    }
}