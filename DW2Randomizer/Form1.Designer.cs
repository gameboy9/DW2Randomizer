namespace DW2Rando
{
    partial class Form1
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
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSHAChecksum = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblReqChecksum = new System.Windows.Forms.Label();
            this.chkChangeStatsToRemix = new System.Windows.Forms.CheckBox();
            this.chkHalfExpGoldReq = new System.Windows.Forms.CheckBox();
            this.radSlightIntensity = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.radModerateIntensity = new System.Windows.Forms.RadioButton();
            this.radHeavyIntensity = new System.Windows.Forms.RadioButton();
            this.radInsaneIntensity = new System.Windows.Forms.RadioButton();
            this.btnRandomize = new System.Windows.Forms.Button();
            this.lblIntensityDesc = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(122, 23);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(320, 20);
            this.txtFileName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "DW2 ROM Image";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(448, 21);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 2;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SHA1 Checksum";
            // 
            // lblSHAChecksum
            // 
            this.lblSHAChecksum.AutoSize = true;
            this.lblSHAChecksum.Location = new System.Drawing.Point(119, 58);
            this.lblSHAChecksum.Name = "lblSHAChecksum";
            this.lblSHAChecksum.Size = new System.Drawing.Size(139, 13);
            this.lblSHAChecksum.TabIndex = 4;
            this.lblSHAChecksum.Text = "??????????????????????";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Required";
            // 
            // lblReqChecksum
            // 
            this.lblReqChecksum.AutoSize = true;
            this.lblReqChecksum.Location = new System.Drawing.Point(119, 82);
            this.lblReqChecksum.Name = "lblReqChecksum";
            this.lblReqChecksum.Size = new System.Drawing.Size(238, 13);
            this.lblReqChecksum.TabIndex = 6;
            this.lblReqChecksum.Text = "f464c7045a489a248686e92164fde2903cfd013e";
            // 
            // chkChangeStatsToRemix
            // 
            this.chkChangeStatsToRemix.AutoSize = true;
            this.chkChangeStatsToRemix.Checked = true;
            this.chkChangeStatsToRemix.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChangeStatsToRemix.Location = new System.Drawing.Point(12, 107);
            this.chkChangeStatsToRemix.Name = "chkChangeStatsToRemix";
            this.chkChangeStatsToRemix.Size = new System.Drawing.Size(310, 17);
            this.chkChangeStatsToRemix.TabIndex = 7;
            this.chkChangeStatsToRemix.Text = "Change all monster stats and weapon values to remix values";
            this.chkChangeStatsToRemix.UseVisualStyleBackColor = true;
            // 
            // chkHalfExpGoldReq
            // 
            this.chkHalfExpGoldReq.AutoSize = true;
            this.chkHalfExpGoldReq.Checked = true;
            this.chkHalfExpGoldReq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHalfExpGoldReq.Location = new System.Drawing.Point(12, 130);
            this.chkHalfExpGoldReq.Name = "chkHalfExpGoldReq";
            this.chkHalfExpGoldReq.Size = new System.Drawing.Size(357, 17);
            this.chkHalfExpGoldReq.TabIndex = 8;
            this.chkHalfExpGoldReq.Text = "Half experience requirements for levels and gold requirements for items";
            this.chkHalfExpGoldReq.UseVisualStyleBackColor = true;
            // 
            // radSlightIntensity
            // 
            this.radSlightIntensity.AutoSize = true;
            this.radSlightIntensity.Checked = true;
            this.radSlightIntensity.Location = new System.Drawing.Point(12, 185);
            this.radSlightIntensity.Name = "radSlightIntensity";
            this.radSlightIntensity.Size = new System.Drawing.Size(51, 17);
            this.radSlightIntensity.TabIndex = 9;
            this.radSlightIntensity.TabStop = true;
            this.radSlightIntensity.Text = "Slight";
            this.radSlightIntensity.UseVisualStyleBackColor = true;
            this.radSlightIntensity.CheckedChanged += new System.EventHandler(this.radSlightIntensity_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Random intensity:";
            // 
            // radModerateIntensity
            // 
            this.radModerateIntensity.AutoSize = true;
            this.radModerateIntensity.Location = new System.Drawing.Point(69, 185);
            this.radModerateIntensity.Name = "radModerateIntensity";
            this.radModerateIntensity.Size = new System.Drawing.Size(70, 17);
            this.radModerateIntensity.TabIndex = 11;
            this.radModerateIntensity.Text = "Moderate";
            this.radModerateIntensity.UseVisualStyleBackColor = true;
            this.radModerateIntensity.CheckedChanged += new System.EventHandler(this.radModerateIntensity_CheckedChanged);
            // 
            // radHeavyIntensity
            // 
            this.radHeavyIntensity.AutoSize = true;
            this.radHeavyIntensity.Location = new System.Drawing.Point(145, 185);
            this.radHeavyIntensity.Name = "radHeavyIntensity";
            this.radHeavyIntensity.Size = new System.Drawing.Size(56, 17);
            this.radHeavyIntensity.TabIndex = 12;
            this.radHeavyIntensity.Text = "Heavy";
            this.radHeavyIntensity.UseVisualStyleBackColor = true;
            this.radHeavyIntensity.CheckedChanged += new System.EventHandler(this.radHeavyIntensity_CheckedChanged);
            // 
            // radInsaneIntensity
            // 
            this.radInsaneIntensity.AutoSize = true;
            this.radInsaneIntensity.Location = new System.Drawing.Point(207, 185);
            this.radInsaneIntensity.Name = "radInsaneIntensity";
            this.radInsaneIntensity.Size = new System.Drawing.Size(68, 17);
            this.radInsaneIntensity.TabIndex = 13;
            this.radInsaneIntensity.Text = "INSANE!";
            this.radInsaneIntensity.UseVisualStyleBackColor = true;
            this.radInsaneIntensity.CheckedChanged += new System.EventHandler(this.radInsaneIntensity_CheckedChanged);
            // 
            // btnRandomize
            // 
            this.btnRandomize.Location = new System.Drawing.Point(446, 185);
            this.btnRandomize.Name = "btnRandomize";
            this.btnRandomize.Size = new System.Drawing.Size(75, 23);
            this.btnRandomize.TabIndex = 14;
            this.btnRandomize.Text = "Randomize!";
            this.btnRandomize.UseVisualStyleBackColor = true;
            this.btnRandomize.Click += new System.EventHandler(this.btnRandomize_Click);
            // 
            // lblIntensityDesc
            // 
            this.lblIntensityDesc.AutoSize = true;
            this.lblIntensityDesc.Location = new System.Drawing.Point(9, 216);
            this.lblIntensityDesc.MaximumSize = new System.Drawing.Size(500, 0);
            this.lblIntensityDesc.Name = "lblIntensityDesc";
            this.lblIntensityDesc.Size = new System.Drawing.Size(0, 13);
            this.lblIntensityDesc.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 313);
            this.Controls.Add(this.lblIntensityDesc);
            this.Controls.Add(this.btnRandomize);
            this.Controls.Add(this.radInsaneIntensity);
            this.Controls.Add(this.radHeavyIntensity);
            this.Controls.Add(this.radModerateIntensity);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.radSlightIntensity);
            this.Controls.Add(this.chkHalfExpGoldReq);
            this.Controls.Add(this.chkChangeStatsToRemix);
            this.Controls.Add(this.lblReqChecksum);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblSHAChecksum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFileName);
            this.Name = "Form1";
            this.Text = "Dragon Warrior 2 Randomizer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSHAChecksum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblReqChecksum;
        private System.Windows.Forms.CheckBox chkChangeStatsToRemix;
        private System.Windows.Forms.CheckBox chkHalfExpGoldReq;
        private System.Windows.Forms.RadioButton radSlightIntensity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radModerateIntensity;
        private System.Windows.Forms.RadioButton radHeavyIntensity;
        private System.Windows.Forms.RadioButton radInsaneIntensity;
        private System.Windows.Forms.Button btnRandomize;
        private System.Windows.Forms.Label lblIntensityDesc;
    }
}

