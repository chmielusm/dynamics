namespace RoleMembershipsLoader
{
    partial class RolePluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblSourceFile = new System.Windows.Forms.Label();
            this.btnRolesBrowse = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.lstWorksheetName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtSecurityRole = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProfile = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkAllRows = new System.Windows.Forms.CheckBox();
            this.txtBu = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtLe = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtTerritory = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSite = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtOccupancy = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtManager = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRowsToImport = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.toolStripMenu.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssSeparator1});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(1025, 25);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(53, 692);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(640, 45);
            this.btnLoad.TabIndex = 7;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblSourceFile
            // 
            this.lblSourceFile.AutoSize = true;
            this.lblSourceFile.Location = new System.Drawing.Point(53, 57);
            this.lblSourceFile.Name = "lblSourceFile";
            this.lblSourceFile.Size = new System.Drawing.Size(153, 25);
            this.lblSourceFile.TabIndex = 15;
            this.lblSourceFile.Text = "Source file path:";
            // 
            // btnRolesBrowse
            // 
            this.btnRolesBrowse.Location = new System.Drawing.Point(901, 95);
            this.btnRolesBrowse.Name = "btnRolesBrowse";
            this.btnRolesBrowse.Size = new System.Drawing.Size(75, 39);
            this.btnRolesBrowse.TabIndex = 13;
            this.btnRolesBrowse.Text = "...";
            this.btnRolesBrowse.UseVisualStyleBackColor = true;
            this.btnRolesBrowse.Click += new System.EventHandler(this.btnRolesBrowse_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(53, 95);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(842, 29);
            this.txtFileName.TabIndex = 0;
            // 
            // lstWorksheetName
            // 
            this.lstWorksheetName.FormattingEnabled = true;
            this.lstWorksheetName.Location = new System.Drawing.Point(53, 175);
            this.lstWorksheetName.Name = "lstWorksheetName";
            this.lstWorksheetName.Size = new System.Drawing.Size(842, 32);
            this.lstWorksheetName.TabIndex = 1;
            this.lstWorksheetName.SelectedIndexChanged += new System.EventHandler(this.lstWorksheetName_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 25);
            this.label1.TabIndex = 17;
            this.label1.Text = "Worksheet";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 25);
            this.label3.TabIndex = 24;
            this.label3.Text = "User Name:";
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(15, 72);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(153, 29);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.Text = "A2";
            this.txtUserName.TextChanged += new System.EventHandler(this.txtUserName_TextChanged);
            // 
            // txtSecurityRole
            // 
            this.txtSecurityRole.Location = new System.Drawing.Point(705, 157);
            this.txtSecurityRole.Name = "txtSecurityRole";
            this.txtSecurityRole.Size = new System.Drawing.Size(153, 29);
            this.txtSecurityRole.TabIndex = 9;
            this.txtSecurityRole.Text = "I2:R2";
            this.txtSecurityRole.TextChanged += new System.EventHandler(this.txtSecurityRole_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(705, 121);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 25);
            this.label4.TabIndex = 26;
            this.label4.Text = "Security Role:";
            // 
            // txtProfile
            // 
            this.txtProfile.Location = new System.Drawing.Point(705, 237);
            this.txtProfile.Name = "txtProfile";
            this.txtProfile.Size = new System.Drawing.Size(153, 29);
            this.txtProfile.TabIndex = 10;
            this.txtProfile.Text = "S2:W2";
            this.txtProfile.TextChanged += new System.EventHandler(this.txtProfile_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(705, 201);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(195, 25);
            this.label6.TabIndex = 28;
            this.label6.Text = "Field Security Profile:";
            // 
            // chkAllRows
            // 
            this.chkAllRows.AutoSize = true;
            this.chkAllRows.Checked = true;
            this.chkAllRows.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllRows.Location = new System.Drawing.Point(53, 566);
            this.chkAllRows.Name = "chkAllRows";
            this.chkAllRows.Size = new System.Drawing.Size(173, 29);
            this.chkAllRows.TabIndex = 11;
            this.chkAllRows.Text = "Import all rows?";
            this.chkAllRows.UseVisualStyleBackColor = true;
            this.chkAllRows.CheckedChanged += new System.EventHandler(this.chkAllRows_CheckedChanged);
            // 
            // txtBu
            // 
            this.txtBu.Location = new System.Drawing.Point(705, 72);
            this.txtBu.Name = "txtBu";
            this.txtBu.Size = new System.Drawing.Size(153, 29);
            this.txtBu.TabIndex = 8;
            this.txtBu.Text = "H2";
            this.txtBu.TextChanged += new System.EventHandler(this.txtBu_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(705, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 25);
            this.label5.TabIndex = 32;
            this.label5.Text = "Business Unit:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtLe);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtTerritory);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtSite);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtOccupancy);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtManager);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtBu);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtSecurityRole);
            this.groupBox1.Controls.Add(this.txtProfile);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(53, 235);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(923, 292);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cell range definition";
            // 
            // txtLe
            // 
            this.txtLe.Location = new System.Drawing.Point(376, 237);
            this.txtLe.Name = "txtLe";
            this.txtLe.Size = new System.Drawing.Size(153, 29);
            this.txtLe.TabIndex = 7;
            this.txtLe.Text = "G2";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(376, 201);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(119, 25);
            this.label10.TabIndex = 42;
            this.label10.Text = "Legal Entity:";
            // 
            // txtTerritory
            // 
            this.txtTerritory.Location = new System.Drawing.Point(376, 157);
            this.txtTerritory.Name = "txtTerritory";
            this.txtTerritory.Size = new System.Drawing.Size(153, 29);
            this.txtTerritory.TabIndex = 6;
            this.txtTerritory.Text = "F2";
            this.txtTerritory.TextChanged += new System.EventHandler(this.txtTerritory_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(376, 121);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 25);
            this.label9.TabIndex = 40;
            this.label9.Text = "Teritory:";
            // 
            // txtSite
            // 
            this.txtSite.Location = new System.Drawing.Point(376, 72);
            this.txtSite.Name = "txtSite";
            this.txtSite.Size = new System.Drawing.Size(153, 29);
            this.txtSite.TabIndex = 5;
            this.txtSite.Text = "E2";
            this.txtSite.TextChanged += new System.EventHandler(this.txtSite_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(376, 36);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 25);
            this.label8.TabIndex = 38;
            this.label8.Text = "Site:";
            // 
            // txtOccupancy
            // 
            this.txtOccupancy.Location = new System.Drawing.Point(15, 237);
            this.txtOccupancy.Name = "txtOccupancy";
            this.txtOccupancy.Size = new System.Drawing.Size(153, 29);
            this.txtOccupancy.TabIndex = 4;
            this.txtOccupancy.Text = "D2";
            this.txtOccupancy.TextChanged += new System.EventHandler(this.txtOccupancy_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 25);
            this.label7.TabIndex = 36;
            this.label7.Text = "Position:";
            // 
            // txtManager
            // 
            this.txtManager.Location = new System.Drawing.Point(15, 156);
            this.txtManager.Name = "txtManager";
            this.txtManager.Size = new System.Drawing.Size(153, 29);
            this.txtManager.TabIndex = 3;
            this.txtManager.Text = "C2";
            this.txtManager.TextChanged += new System.EventHandler(this.txtManager_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 25);
            this.label2.TabIndex = 34;
            this.label2.Text = "Manager:";
            // 
            // txtRowsToImport
            // 
            this.txtRowsToImport.Location = new System.Drawing.Point(242, 566);
            this.txtRowsToImport.Name = "txtRowsToImport";
            this.txtRowsToImport.Size = new System.Drawing.Size(153, 29);
            this.txtRowsToImport.TabIndex = 12;
            this.txtRowsToImport.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(699, 692);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(277, 45);
            this.btnClose.TabIndex = 34;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // RolePluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtRowsToImport);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkAllRows);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstWorksheetName);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.lblSourceFile);
            this.Controls.Add(this.btnRolesBrowse);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.toolStripMenu);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "RolePluginControl";
            this.Size = new System.Drawing.Size(1025, 780);
            this.OnCloseTool += new System.EventHandler(this.RolePluginControl_OnCloseTool);
            this.Load += new System.EventHandler(this.RolePluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblSourceFile;
        private System.Windows.Forms.Button btnRolesBrowse;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.ComboBox lstWorksheetName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtSecurityRole;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtProfile;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkAllRows;
        private System.Windows.Forms.TextBox txtBu;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtLe;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtTerritory;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSite;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtOccupancy;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtManager;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRowsToImport;
        private System.Windows.Forms.Button btnClose;
    }
}
