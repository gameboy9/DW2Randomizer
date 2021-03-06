﻿namespace DW2Randomizer
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
			this.components = new System.ComponentModel.Container();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.lblSHAChecksum = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lblReqChecksum = new System.Windows.Forms.Label();
			this.radSlightIntensity = new System.Windows.Forms.RadioButton();
			this.radModerateIntensity = new System.Windows.Forms.RadioButton();
			this.radHeavyIntensity = new System.Windows.Forms.RadioButton();
			this.radInsaneIntensity = new System.Windows.Forms.RadioButton();
			this.btnRandomize = new System.Windows.Forms.Button();
			this.btnCompare = new System.Windows.Forms.Button();
			this.txtSeed = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnCompareBrowse = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.txtCompare = new System.Windows.Forms.TextBox();
			this.btnNewSeed = new System.Windows.Forms.Button();
			this.tabAll = new System.Windows.Forms.TabControl();
			this.adjustments = new System.Windows.Forms.TabPage();
			this.chkSpeedWaitMusic = new System.Windows.Forms.CheckBox();
			this.chkAllDogs = new System.Windows.Forms.CheckBox();
			this.chkExperimental = new System.Windows.Forms.CheckBox();
			this.chkSpeedHacks = new System.Windows.Forms.CheckBox();
			this.label12 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.txtPrincessName = new System.Windows.Forms.TextBox();
			this.txtPrinceName = new System.Windows.Forms.TextBox();
			this.cboEncounterRate = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.chkGPRandomize = new System.Windows.Forms.CheckBox();
			this.chkXPRandomize = new System.Windows.Forms.CheckBox();
			this.label9 = new System.Windows.Forms.Label();
			this.cboGPReq = new System.Windows.Forms.ComboBox();
			this.cboXPReq = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.chkChangeStatsToRemix = new System.Windows.Forms.CheckBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.chkMonsterResists = new System.Windows.Forms.CheckBox();
			this.chkMonsterPatterns = new System.Windows.Forms.CheckBox();
			this.chkSmallMap = new System.Windows.Forms.CheckBox();
			this.chkTreasures = new System.Windows.Forms.CheckBox();
			this.chkSpellStrengths = new System.Windows.Forms.CheckBox();
			this.chkHeroStats = new System.Windows.Forms.CheckBox();
			this.chkHeroStores = new System.Windows.Forms.CheckBox();
			this.chkSpellLearning = new System.Windows.Forms.CheckBox();
			this.chkMonsterStats = new System.Windows.Forms.CheckBox();
			this.chkMonsterZones = new System.Windows.Forms.CheckBox();
			this.chkEquipEffects = new System.Windows.Forms.CheckBox();
			this.chkEquipment = new System.Windows.Forms.CheckBox();
			this.chkWhoCanEquip = new System.Windows.Forms.CheckBox();
			this.chkMap = new System.Windows.Forms.CheckBox();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.btnLudicrousRando = new System.Windows.Forms.Button();
			this.btnUltraRando = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.lblIntensityDesc = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.txtFlags = new System.Windows.Forms.TextBox();
			this.ttUltra = new System.Windows.Forms.ToolTip(this.components);
			this.ttLudicrous = new System.Windows.Forms.ToolTip(this.components);
			this.lblNewChecksum = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.btnCopyChecksum = new System.Windows.Forms.Button();
			this.tabAll.SuspendLayout();
			this.adjustments.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.SuspendLayout();
			// 
			// txtFileName
			// 
			this.txtFileName.Location = new System.Drawing.Point(122, 23);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(320, 20);
			this.txtFileName.TabIndex = 0;
			this.txtFileName.Leave += new System.EventHandler(this.txtFileName_Leave);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 23);
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
			this.btnBrowse.TabIndex = 1;
			this.btnBrowse.Text = "Browse";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(13, 78);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "SHA1 Checksum";
			// 
			// lblSHAChecksum
			// 
			this.lblSHAChecksum.AutoSize = true;
			this.lblSHAChecksum.Location = new System.Drawing.Point(119, 78);
			this.lblSHAChecksum.Name = "lblSHAChecksum";
			this.lblSHAChecksum.Size = new System.Drawing.Size(247, 13);
			this.lblSHAChecksum.TabIndex = 4;
			this.lblSHAChecksum.Text = "????????????????????????????????????????";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(13, 102);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(50, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Required";
			// 
			// lblReqChecksum
			// 
			this.lblReqChecksum.AutoSize = true;
			this.lblReqChecksum.Location = new System.Drawing.Point(119, 102);
			this.lblReqChecksum.Name = "lblReqChecksum";
			this.lblReqChecksum.Size = new System.Drawing.Size(238, 13);
			this.lblReqChecksum.TabIndex = 6;
			this.lblReqChecksum.Text = "f464c7045a489a248686e92164fde2903cfd013e";
			// 
			// radSlightIntensity
			// 
			this.radSlightIntensity.AutoSize = true;
			this.radSlightIntensity.Location = new System.Drawing.Point(149, 222);
			this.radSlightIntensity.Name = "radSlightIntensity";
			this.radSlightIntensity.Size = new System.Drawing.Size(48, 17);
			this.radSlightIntensity.TabIndex = 11;
			this.radSlightIntensity.Text = "Light";
			this.radSlightIntensity.UseVisualStyleBackColor = true;
			this.radSlightIntensity.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// radModerateIntensity
			// 
			this.radModerateIntensity.AutoSize = true;
			this.radModerateIntensity.Location = new System.Drawing.Point(204, 222);
			this.radModerateIntensity.Name = "radModerateIntensity";
			this.radModerateIntensity.Size = new System.Drawing.Size(43, 17);
			this.radModerateIntensity.TabIndex = 12;
			this.radModerateIntensity.Text = "Silly";
			this.radModerateIntensity.UseVisualStyleBackColor = true;
			this.radModerateIntensity.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// radHeavyIntensity
			// 
			this.radHeavyIntensity.AutoSize = true;
			this.radHeavyIntensity.Location = new System.Drawing.Point(254, 222);
			this.radHeavyIntensity.Name = "radHeavyIntensity";
			this.radHeavyIntensity.Size = new System.Drawing.Size(74, 17);
			this.radHeavyIntensity.TabIndex = 13;
			this.radHeavyIntensity.Text = "Ridiculous";
			this.radHeavyIntensity.UseVisualStyleBackColor = true;
			this.radHeavyIntensity.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// radInsaneIntensity
			// 
			this.radInsaneIntensity.AutoSize = true;
			this.radInsaneIntensity.Checked = true;
			this.radInsaneIntensity.Location = new System.Drawing.Point(335, 222);
			this.radInsaneIntensity.Name = "radInsaneIntensity";
			this.radInsaneIntensity.Size = new System.Drawing.Size(91, 17);
			this.radInsaneIntensity.TabIndex = 14;
			this.radInsaneIntensity.TabStop = true;
			this.radInsaneIntensity.Text = "LUDICROUS!";
			this.radInsaneIntensity.UseVisualStyleBackColor = true;
			this.radInsaneIntensity.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// btnRandomize
			// 
			this.btnRandomize.Location = new System.Drawing.Point(444, 221);
			this.btnRandomize.Name = "btnRandomize";
			this.btnRandomize.Size = new System.Drawing.Size(75, 23);
			this.btnRandomize.TabIndex = 15;
			this.btnRandomize.Text = "Randomize!";
			this.btnRandomize.UseVisualStyleBackColor = true;
			this.btnRandomize.Click += new System.EventHandler(this.btnRandomize_Click);
			// 
			// btnCompare
			// 
			this.btnCompare.Location = new System.Drawing.Point(448, 76);
			this.btnCompare.Name = "btnCompare";
			this.btnCompare.Size = new System.Drawing.Size(75, 23);
			this.btnCompare.TabIndex = 4;
			this.btnCompare.Text = "Compare";
			this.btnCompare.UseVisualStyleBackColor = true;
			this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
			// 
			// txtSeed
			// 
			this.txtSeed.Location = new System.Drawing.Point(70, 158);
			this.txtSeed.Name = "txtSeed";
			this.txtSeed.Size = new System.Drawing.Size(100, 20);
			this.txtSeed.TabIndex = 8;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(13, 160);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 13);
			this.label3.TabIndex = 20;
			this.label3.Text = "Seed";
			// 
			// btnCompareBrowse
			// 
			this.btnCompareBrowse.Location = new System.Drawing.Point(448, 47);
			this.btnCompareBrowse.Name = "btnCompareBrowse";
			this.btnCompareBrowse.Size = new System.Drawing.Size(75, 23);
			this.btnCompareBrowse.TabIndex = 3;
			this.btnCompareBrowse.Text = "Browse";
			this.btnCompareBrowse.UseVisualStyleBackColor = true;
			this.btnCompareBrowse.Click += new System.EventHandler(this.btnCompareBrowse_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(13, 49);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(94, 13);
			this.label5.TabIndex = 24;
			this.label5.Text = "Comparison Image";
			// 
			// txtCompare
			// 
			this.txtCompare.Location = new System.Drawing.Point(122, 49);
			this.txtCompare.Name = "txtCompare";
			this.txtCompare.Size = new System.Drawing.Size(320, 20);
			this.txtCompare.TabIndex = 2;
			// 
			// btnNewSeed
			// 
			this.btnNewSeed.Location = new System.Drawing.Point(187, 156);
			this.btnNewSeed.Name = "btnNewSeed";
			this.btnNewSeed.Size = new System.Drawing.Size(75, 23);
			this.btnNewSeed.TabIndex = 9;
			this.btnNewSeed.Text = "New Seed";
			this.btnNewSeed.UseVisualStyleBackColor = true;
			this.btnNewSeed.Click += new System.EventHandler(this.btnNewSeed_Click);
			// 
			// tabAll
			// 
			this.tabAll.Controls.Add(this.adjustments);
			this.tabAll.Controls.Add(this.tabPage2);
			this.tabAll.Controls.Add(this.tabPage1);
			this.tabAll.Location = new System.Drawing.Point(12, 245);
			this.tabAll.Name = "tabAll";
			this.tabAll.SelectedIndex = 0;
			this.tabAll.Size = new System.Drawing.Size(507, 197);
			this.tabAll.TabIndex = 26;
			// 
			// adjustments
			// 
			this.adjustments.Controls.Add(this.chkSpeedWaitMusic);
			this.adjustments.Controls.Add(this.chkAllDogs);
			this.adjustments.Controls.Add(this.chkExperimental);
			this.adjustments.Controls.Add(this.chkSpeedHacks);
			this.adjustments.Controls.Add(this.label12);
			this.adjustments.Controls.Add(this.label11);
			this.adjustments.Controls.Add(this.txtPrincessName);
			this.adjustments.Controls.Add(this.txtPrinceName);
			this.adjustments.Controls.Add(this.cboEncounterRate);
			this.adjustments.Controls.Add(this.label10);
			this.adjustments.Controls.Add(this.chkGPRandomize);
			this.adjustments.Controls.Add(this.chkXPRandomize);
			this.adjustments.Controls.Add(this.label9);
			this.adjustments.Controls.Add(this.cboGPReq);
			this.adjustments.Controls.Add(this.cboXPReq);
			this.adjustments.Controls.Add(this.label8);
			this.adjustments.Controls.Add(this.chkChangeStatsToRemix);
			this.adjustments.Location = new System.Drawing.Point(4, 22);
			this.adjustments.Name = "adjustments";
			this.adjustments.Padding = new System.Windows.Forms.Padding(3);
			this.adjustments.Size = new System.Drawing.Size(499, 171);
			this.adjustments.TabIndex = 0;
			this.adjustments.Text = "Adjustments";
			this.adjustments.UseVisualStyleBackColor = true;
			// 
			// chkSpeedWaitMusic
			// 
			this.chkSpeedWaitMusic.AutoSize = true;
			this.chkSpeedWaitMusic.Location = new System.Drawing.Point(363, 142);
			this.chkSpeedWaitMusic.Name = "chkSpeedWaitMusic";
			this.chkSpeedWaitMusic.Size = new System.Drawing.Size(135, 17);
			this.chkSpeedWaitMusic.TabIndex = 26;
			this.chkSpeedWaitMusic.Text = "Speedy Wait Music (m)";
			this.chkSpeedWaitMusic.UseVisualStyleBackColor = true;
			this.chkSpeedWaitMusic.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkAllDogs
			// 
			this.chkAllDogs.AutoSize = true;
			this.chkAllDogs.Location = new System.Drawing.Point(247, 86);
			this.chkAllDogs.Name = "chkAllDogs";
			this.chkAllDogs.Size = new System.Drawing.Size(129, 17);
			this.chkAllDogs.TabIndex = 25;
			this.chkAllDogs.Text = "Turn all NPCs to dogs";
			this.chkAllDogs.UseVisualStyleBackColor = true;
			// 
			// chkExperimental
			// 
			this.chkExperimental.AutoSize = true;
			this.chkExperimental.Location = new System.Drawing.Point(247, 142);
			this.chkExperimental.Name = "chkExperimental";
			this.chkExperimental.Size = new System.Drawing.Size(121, 17);
			this.chkExperimental.TabIndex = 24;
			this.chkExperimental.Text = "Text speed hack (a)";
			this.chkExperimental.UseVisualStyleBackColor = true;
			this.chkExperimental.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkSpeedHacks
			// 
			this.chkSpeedHacks.AutoSize = true;
			this.chkSpeedHacks.Location = new System.Drawing.Point(247, 115);
			this.chkSpeedHacks.Name = "chkSpeedHacks";
			this.chkSpeedHacks.Size = new System.Drawing.Size(122, 17);
			this.chkSpeedHacks.TabIndex = 23;
			this.chkSpeedHacks.Text = "Speed up battles (A)";
			this.chkSpeedHacks.UseVisualStyleBackColor = true;
			this.chkSpeedHacks.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(6, 144);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(78, 13);
			this.label12.TabIndex = 22;
			this.label12.Text = "Princess Name";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(6, 117);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(68, 13);
			this.label11.TabIndex = 21;
			this.label11.Text = "Prince Name";
			// 
			// txtPrincessName
			// 
			this.txtPrincessName.Location = new System.Drawing.Point(115, 140);
			this.txtPrincessName.Name = "txtPrincessName";
			this.txtPrincessName.Size = new System.Drawing.Size(100, 20);
			this.txtPrincessName.TabIndex = 20;
			// 
			// txtPrinceName
			// 
			this.txtPrinceName.Location = new System.Drawing.Point(115, 113);
			this.txtPrinceName.Name = "txtPrinceName";
			this.txtPrinceName.Size = new System.Drawing.Size(100, 20);
			this.txtPrinceName.TabIndex = 19;
			// 
			// cboEncounterRate
			// 
			this.cboEncounterRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboEncounterRate.FormattingEnabled = true;
			this.cboEncounterRate.Items.AddRange(new object[] {
            "300%",
            "200%",
            "150%",
            "100%",
            "75%",
            "50%",
            "33%",
            "25%"});
			this.cboEncounterRate.Location = new System.Drawing.Point(115, 84);
			this.cboEncounterRate.Name = "cboEncounterRate";
			this.cboEncounterRate.Size = new System.Drawing.Size(121, 21);
			this.cboEncounterRate.TabIndex = 18;
			this.cboEncounterRate.SelectedIndexChanged += new System.EventHandler(this.determineFlags);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(6, 88);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(82, 13);
			this.label10.TabIndex = 17;
			this.label10.Text = "Encounter Rate";
			// 
			// chkGPRandomize
			// 
			this.chkGPRandomize.AutoSize = true;
			this.chkGPRandomize.Location = new System.Drawing.Point(247, 59);
			this.chkGPRandomize.Name = "chkGPRandomize";
			this.chkGPRandomize.Size = new System.Drawing.Size(155, 17);
			this.chkGPRandomize.TabIndex = 16;
			this.chkGPRandomize.Text = "Randomize Monster GP (G)";
			this.chkGPRandomize.UseVisualStyleBackColor = true;
			this.chkGPRandomize.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkXPRandomize
			// 
			this.chkXPRandomize.AutoSize = true;
			this.chkXPRandomize.Location = new System.Drawing.Point(247, 34);
			this.chkXPRandomize.Name = "chkXPRandomize";
			this.chkXPRandomize.Size = new System.Drawing.Size(153, 17);
			this.chkXPRandomize.TabIndex = 15;
			this.chkXPRandomize.Text = "Randomize Monster XP (X)";
			this.chkXPRandomize.UseVisualStyleBackColor = true;
			this.chkXPRandomize.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 61);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(97, 13);
			this.label9.TabIndex = 14;
			this.label9.Text = "Gold Requirements";
			// 
			// cboGPReq
			// 
			this.cboGPReq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboGPReq.FormattingEnabled = true;
			this.cboGPReq.Items.AddRange(new object[] {
            "100%",
            "75%",
            "50%",
            "33%"});
			this.cboGPReq.Location = new System.Drawing.Point(115, 57);
			this.cboGPReq.Name = "cboGPReq";
			this.cboGPReq.Size = new System.Drawing.Size(121, 21);
			this.cboGPReq.TabIndex = 13;
			this.cboGPReq.SelectedIndexChanged += new System.EventHandler(this.determineFlags);
			// 
			// cboXPReq
			// 
			this.cboXPReq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboXPReq.FormattingEnabled = true;
			this.cboXPReq.Items.AddRange(new object[] {
            "100%",
            "75%",
            "50%",
            "33%"});
			this.cboXPReq.Location = new System.Drawing.Point(115, 32);
			this.cboXPReq.Name = "cboXPReq";
			this.cboXPReq.Size = new System.Drawing.Size(121, 21);
			this.cboXPReq.TabIndex = 12;
			this.cboXPReq.SelectedIndexChanged += new System.EventHandler(this.determineFlags);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 36);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(89, 13);
			this.label8.TabIndex = 11;
			this.label8.Text = "XP Requirements";
			// 
			// chkChangeStatsToRemix
			// 
			this.chkChangeStatsToRemix.AutoSize = true;
			this.chkChangeStatsToRemix.Location = new System.Drawing.Point(8, 6);
			this.chkChangeStatsToRemix.Name = "chkChangeStatsToRemix";
			this.chkChangeStatsToRemix.Size = new System.Drawing.Size(327, 17);
			this.chkChangeStatsToRemix.TabIndex = 8;
			this.chkChangeStatsToRemix.Text = "Change all monster stats and weapon values to remix values (R)";
			this.chkChangeStatsToRemix.UseVisualStyleBackColor = true;
			this.chkChangeStatsToRemix.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.chkMonsterResists);
			this.tabPage2.Controls.Add(this.chkMonsterPatterns);
			this.tabPage2.Controls.Add(this.chkSmallMap);
			this.tabPage2.Controls.Add(this.chkTreasures);
			this.tabPage2.Controls.Add(this.chkSpellStrengths);
			this.tabPage2.Controls.Add(this.chkHeroStats);
			this.tabPage2.Controls.Add(this.chkHeroStores);
			this.tabPage2.Controls.Add(this.chkSpellLearning);
			this.tabPage2.Controls.Add(this.chkMonsterStats);
			this.tabPage2.Controls.Add(this.chkMonsterZones);
			this.tabPage2.Controls.Add(this.chkEquipEffects);
			this.tabPage2.Controls.Add(this.chkEquipment);
			this.tabPage2.Controls.Add(this.chkWhoCanEquip);
			this.tabPage2.Controls.Add(this.chkMap);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(499, 171);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Randomization";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// chkMonsterResists
			// 
			this.chkMonsterResists.AutoSize = true;
			this.chkMonsterResists.Location = new System.Drawing.Point(235, 115);
			this.chkMonsterResists.Name = "chkMonsterResists";
			this.chkMonsterResists.Size = new System.Drawing.Size(197, 17);
			this.chkMonsterResists.TabIndex = 13;
			this.chkMonsterResists.Text = "Randomize Monster Resistances (V)";
			this.chkMonsterResists.UseVisualStyleBackColor = true;
			this.chkMonsterResists.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkMonsterPatterns
			// 
			this.chkMonsterPatterns.AutoSize = true;
			this.chkMonsterPatterns.Location = new System.Drawing.Point(235, 136);
			this.chkMonsterPatterns.Name = "chkMonsterPatterns";
			this.chkMonsterPatterns.Size = new System.Drawing.Size(178, 17);
			this.chkMonsterPatterns.TabIndex = 12;
			this.chkMonsterPatterns.Text = "Randomize Monster Patterns (Y)";
			this.chkMonsterPatterns.UseVisualStyleBackColor = true;
			this.chkMonsterPatterns.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkSmallMap
			// 
			this.chkSmallMap.AutoSize = true;
			this.chkSmallMap.Location = new System.Drawing.Point(6, 31);
			this.chkSmallMap.Name = "chkSmallMap";
			this.chkSmallMap.Size = new System.Drawing.Size(106, 17);
			this.chkSmallMap.TabIndex = 11;
			this.chkSmallMap.Text = "128x128 Map (u)";
			this.chkSmallMap.UseVisualStyleBackColor = true;
			this.chkSmallMap.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkTreasures
			// 
			this.chkTreasures.AutoSize = true;
			this.chkTreasures.Location = new System.Drawing.Point(235, 94);
			this.chkTreasures.Name = "chkTreasures";
			this.chkTreasures.Size = new System.Drawing.Size(145, 17);
			this.chkTreasures.TabIndex = 10;
			this.chkTreasures.Text = "Randomize Treasures (T)";
			this.chkTreasures.UseVisualStyleBackColor = true;
			this.chkTreasures.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkSpellStrengths
			// 
			this.chkSpellStrengths.AutoSize = true;
			this.chkSpellStrengths.Location = new System.Drawing.Point(235, 31);
			this.chkSpellStrengths.Name = "chkSpellStrengths";
			this.chkSpellStrengths.Size = new System.Drawing.Size(169, 17);
			this.chkSpellStrengths.TabIndex = 9;
			this.chkSpellStrengths.Text = "Randomize Spell Strengths (S)";
			this.chkSpellStrengths.UseVisualStyleBackColor = true;
			this.chkSpellStrengths.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkHeroStats
			// 
			this.chkHeroStats.AutoSize = true;
			this.chkHeroStats.Location = new System.Drawing.Point(235, 52);
			this.chkHeroStats.Name = "chkHeroStats";
			this.chkHeroStats.Size = new System.Drawing.Size(149, 17);
			this.chkHeroStats.TabIndex = 8;
			this.chkHeroStats.Text = "Randomize Hero Stats (H)";
			this.chkHeroStats.UseVisualStyleBackColor = true;
			this.chkHeroStats.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkHeroStores
			// 
			this.chkHeroStores.AutoSize = true;
			this.chkHeroStores.Location = new System.Drawing.Point(235, 73);
			this.chkHeroStores.Name = "chkHeroStores";
			this.chkHeroStores.Size = new System.Drawing.Size(128, 17);
			this.chkHeroStores.TabIndex = 7;
			this.chkHeroStores.Text = "Randomize Stores (C)";
			this.chkHeroStores.UseVisualStyleBackColor = true;
			this.chkHeroStores.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkSpellLearning
			// 
			this.chkSpellLearning.AutoSize = true;
			this.chkSpellLearning.Location = new System.Drawing.Point(235, 10);
			this.chkSpellLearning.Name = "chkSpellLearning";
			this.chkSpellLearning.Size = new System.Drawing.Size(164, 17);
			this.chkSpellLearning.TabIndex = 6;
			this.chkSpellLearning.Text = "Randomize Spell Learning (L)";
			this.chkSpellLearning.UseVisualStyleBackColor = true;
			this.chkSpellLearning.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkMonsterStats
			// 
			this.chkMonsterStats.AutoSize = true;
			this.chkMonsterStats.Location = new System.Drawing.Point(6, 115);
			this.chkMonsterStats.Name = "chkMonsterStats";
			this.chkMonsterStats.Size = new System.Drawing.Size(165, 17);
			this.chkMonsterStats.TabIndex = 5;
			this.chkMonsterStats.Text = "Randomize Monster Stats (M)";
			this.chkMonsterStats.UseVisualStyleBackColor = true;
			this.chkMonsterStats.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkMonsterZones
			// 
			this.chkMonsterZones.AutoSize = true;
			this.chkMonsterZones.Location = new System.Drawing.Point(6, 136);
			this.chkMonsterZones.Name = "chkMonsterZones";
			this.chkMonsterZones.Size = new System.Drawing.Size(169, 17);
			this.chkMonsterZones.TabIndex = 4;
			this.chkMonsterZones.Text = "Randomize Monster Zones (Z)";
			this.chkMonsterZones.UseVisualStyleBackColor = true;
			this.chkMonsterZones.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkEquipEffects
			// 
			this.chkEquipEffects.AutoSize = true;
			this.chkEquipEffects.Location = new System.Drawing.Point(6, 94);
			this.chkEquipEffects.Name = "chkEquipEffects";
			this.chkEquipEffects.Size = new System.Drawing.Size(184, 17);
			this.chkEquipEffects.TabIndex = 3;
			this.chkEquipEffects.Text = "Randomize Equipment Effects (E)";
			this.chkEquipEffects.UseVisualStyleBackColor = true;
			this.chkEquipEffects.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkEquipment
			// 
			this.chkEquipment.AutoSize = true;
			this.chkEquipment.Location = new System.Drawing.Point(6, 52);
			this.chkEquipment.Name = "chkEquipment";
			this.chkEquipment.Size = new System.Drawing.Size(149, 17);
			this.chkEquipment.TabIndex = 2;
			this.chkEquipment.Text = "Randomize Equipment (Q)";
			this.chkEquipment.UseVisualStyleBackColor = true;
			this.chkEquipment.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkWhoCanEquip
			// 
			this.chkWhoCanEquip.AutoSize = true;
			this.chkWhoCanEquip.Location = new System.Drawing.Point(6, 73);
			this.chkWhoCanEquip.Name = "chkWhoCanEquip";
			this.chkWhoCanEquip.Size = new System.Drawing.Size(177, 17);
			this.chkWhoCanEquip.TabIndex = 1;
			this.chkWhoCanEquip.Text = "Randomize Who Can Equip (W)";
			this.chkWhoCanEquip.UseVisualStyleBackColor = true;
			this.chkWhoCanEquip.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// chkMap
			// 
			this.chkMap.AutoSize = true;
			this.chkMap.Location = new System.Drawing.Point(6, 10);
			this.chkMap.Name = "chkMap";
			this.chkMap.Size = new System.Drawing.Size(120, 17);
			this.chkMap.TabIndex = 0;
			this.chkMap.Text = "Randomize Map (U)";
			this.chkMap.UseVisualStyleBackColor = true;
			this.chkMap.CheckedChanged += new System.EventHandler(this.determineFlags);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.btnLudicrousRando);
			this.tabPage1.Controls.Add(this.btnUltraRando);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(499, 171);
			this.tabPage1.TabIndex = 2;
			this.tabPage1.Text = "Shortcuts";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// btnLudicrousRando
			// 
			this.btnLudicrousRando.Location = new System.Drawing.Point(13, 42);
			this.btnLudicrousRando.Name = "btnLudicrousRando";
			this.btnLudicrousRando.Size = new System.Drawing.Size(167, 23);
			this.btnLudicrousRando.TabIndex = 1;
			this.btnLudicrousRando.Text = "Ludicrous Randomization";
			this.btnLudicrousRando.UseVisualStyleBackColor = true;
			// 
			// btnUltraRando
			// 
			this.btnUltraRando.Location = new System.Drawing.Point(13, 13);
			this.btnUltraRando.Name = "btnUltraRando";
			this.btnUltraRando.Size = new System.Drawing.Size(167, 23);
			this.btnUltraRando.TabIndex = 0;
			this.btnUltraRando.Text = "Ultra Randomization";
			this.btnUltraRando.UseVisualStyleBackColor = true;
			this.btnUltraRando.Click += new System.EventHandler(this.btnUltraRando_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(13, 226);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(120, 13);
			this.label6.TabIndex = 27;
			this.label6.Text = "Amount of Randomness";
			// 
			// lblIntensityDesc
			// 
			this.lblIntensityDesc.AutoSize = true;
			this.lblIntensityDesc.Location = new System.Drawing.Point(15, 465);
			this.lblIntensityDesc.Name = "lblIntensityDesc";
			this.lblIntensityDesc.Size = new System.Drawing.Size(0, 13);
			this.lblIntensityDesc.TabIndex = 28;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(13, 191);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(32, 13);
			this.label7.TabIndex = 29;
			this.label7.Text = "Flags";
			// 
			// txtFlags
			// 
			this.txtFlags.Location = new System.Drawing.Point(70, 188);
			this.txtFlags.Name = "txtFlags";
			this.txtFlags.Size = new System.Drawing.Size(192, 20);
			this.txtFlags.TabIndex = 30;
			this.txtFlags.TextChanged += new System.EventHandler(this.txtFlags_TextChanged);
			// 
			// lblNewChecksum
			// 
			this.lblNewChecksum.AutoSize = true;
			this.lblNewChecksum.Location = new System.Drawing.Point(119, 127);
			this.lblNewChecksum.Name = "lblNewChecksum";
			this.lblNewChecksum.Size = new System.Drawing.Size(247, 13);
			this.lblNewChecksum.TabIndex = 32;
			this.lblNewChecksum.Text = "????????????????????????????????????????";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(13, 127);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(82, 13);
			this.label14.TabIndex = 31;
			this.label14.Text = "New Checksum";
			// 
			// btnCopyChecksum
			// 
			this.btnCopyChecksum.Location = new System.Drawing.Point(402, 122);
			this.btnCopyChecksum.Name = "btnCopyChecksum";
			this.btnCopyChecksum.Size = new System.Drawing.Size(121, 23);
			this.btnCopyChecksum.TabIndex = 33;
			this.btnCopyChecksum.Text = "Copy New Checksum";
			this.btnCopyChecksum.UseVisualStyleBackColor = true;
			this.btnCopyChecksum.Click += new System.EventHandler(this.btnCopyChecksum_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(546, 498);
			this.Controls.Add(this.btnCopyChecksum);
			this.Controls.Add(this.lblNewChecksum);
			this.Controls.Add(this.label14);
			this.Controls.Add(this.txtFlags);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.lblIntensityDesc);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.tabAll);
			this.Controls.Add(this.btnNewSeed);
			this.Controls.Add(this.btnCompareBrowse);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.txtCompare);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.txtSeed);
			this.Controls.Add(this.btnCompare);
			this.Controls.Add(this.btnRandomize);
			this.Controls.Add(this.radInsaneIntensity);
			this.Controls.Add(this.radHeavyIntensity);
			this.Controls.Add(this.radModerateIntensity);
			this.Controls.Add(this.radSlightIntensity);
			this.Controls.Add(this.lblReqChecksum);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lblSHAChecksum);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.btnBrowse);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtFileName);
			this.Name = "Form1";
			this.Text = "Dragon Warrior 2 Randomizer 2.10.6";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tabAll.ResumeLayout(false);
			this.adjustments.ResumeLayout(false);
			this.adjustments.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.tabPage1.ResumeLayout(false);
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
        private System.Windows.Forms.RadioButton radSlightIntensity;
        private System.Windows.Forms.RadioButton radModerateIntensity;
        private System.Windows.Forms.RadioButton radHeavyIntensity;
        private System.Windows.Forms.RadioButton radInsaneIntensity;
        private System.Windows.Forms.Button btnRandomize;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.TextBox txtSeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnNewSeed;
        private System.Windows.Forms.TextBox txtCompare;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCompareBrowse;
        private System.Windows.Forms.TabControl tabAll;
        private System.Windows.Forms.TabPage adjustments;
        private System.Windows.Forms.CheckBox chkChangeStatsToRemix;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkMap;
        private System.Windows.Forms.CheckBox chkSpellStrengths;
        private System.Windows.Forms.CheckBox chkHeroStats;
        private System.Windows.Forms.CheckBox chkHeroStores;
        private System.Windows.Forms.CheckBox chkSpellLearning;
        private System.Windows.Forms.CheckBox chkMonsterStats;
        private System.Windows.Forms.CheckBox chkMonsterZones;
        private System.Windows.Forms.CheckBox chkEquipEffects;
        private System.Windows.Forms.CheckBox chkEquipment;
        private System.Windows.Forms.CheckBox chkWhoCanEquip;
        private System.Windows.Forms.CheckBox chkTreasures;
        private System.Windows.Forms.Label lblIntensityDesc;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtFlags;
        private System.Windows.Forms.ComboBox cboXPReq;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cboGPReq;
        private System.Windows.Forms.CheckBox chkGPRandomize;
        private System.Windows.Forms.CheckBox chkXPRandomize;
        private System.Windows.Forms.ComboBox cboEncounterRate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtPrincessName;
        private System.Windows.Forms.TextBox txtPrinceName;
        private System.Windows.Forms.CheckBox chkSmallMap;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnLudicrousRando;
        private System.Windows.Forms.Button btnUltraRando;
        private System.Windows.Forms.ToolTip ttUltra;
        private System.Windows.Forms.ToolTip ttLudicrous;
        private System.Windows.Forms.CheckBox chkExperimental;
        private System.Windows.Forms.CheckBox chkSpeedHacks;
        private System.Windows.Forms.Label lblNewChecksum;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnCopyChecksum;
        private System.Windows.Forms.CheckBox chkAllDogs;
        private System.Windows.Forms.CheckBox chkSpeedWaitMusic;
        private System.Windows.Forms.CheckBox chkMonsterResists;
        private System.Windows.Forms.CheckBox chkMonsterPatterns;
    }
}

