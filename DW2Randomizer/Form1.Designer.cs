namespace DW2Randomizer
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
            this.chkSpeedWaitMusic = new System.Windows.Forms.CheckBox();
            this.tabAll.SuspendLayout();
            this.adjustments.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(183, 35);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(478, 26);
            this.txtFileName.TabIndex = 0;
            this.txtFileName.Leave += new System.EventHandler(this.txtFileName_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "DW2 ROM Image";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(672, 32);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(112, 35);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 120);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "SHA1 Checksum";
            // 
            // lblSHAChecksum
            // 
            this.lblSHAChecksum.AutoSize = true;
            this.lblSHAChecksum.Location = new System.Drawing.Point(178, 120);
            this.lblSHAChecksum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSHAChecksum.Name = "lblSHAChecksum";
            this.lblSHAChecksum.Size = new System.Drawing.Size(369, 20);
            this.lblSHAChecksum.TabIndex = 4;
            this.lblSHAChecksum.Text = "????????????????????????????????????????";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 157);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Required";
            // 
            // lblReqChecksum
            // 
            this.lblReqChecksum.AutoSize = true;
            this.lblReqChecksum.Location = new System.Drawing.Point(178, 157);
            this.lblReqChecksum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblReqChecksum.Name = "lblReqChecksum";
            this.lblReqChecksum.Size = new System.Drawing.Size(355, 20);
            this.lblReqChecksum.TabIndex = 6;
            this.lblReqChecksum.Text = "f464c7045a489a248686e92164fde2903cfd013e";
            // 
            // radSlightIntensity
            // 
            this.radSlightIntensity.AutoSize = true;
            this.radSlightIntensity.Location = new System.Drawing.Point(224, 342);
            this.radSlightIntensity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radSlightIntensity.Name = "radSlightIntensity";
            this.radSlightIntensity.Size = new System.Drawing.Size(69, 24);
            this.radSlightIntensity.TabIndex = 11;
            this.radSlightIntensity.Text = "Light";
            this.radSlightIntensity.UseVisualStyleBackColor = true;
            this.radSlightIntensity.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // radModerateIntensity
            // 
            this.radModerateIntensity.AutoSize = true;
            this.radModerateIntensity.Location = new System.Drawing.Point(306, 342);
            this.radModerateIntensity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radModerateIntensity.Name = "radModerateIntensity";
            this.radModerateIntensity.Size = new System.Drawing.Size(61, 24);
            this.radModerateIntensity.TabIndex = 12;
            this.radModerateIntensity.Text = "Silly";
            this.radModerateIntensity.UseVisualStyleBackColor = true;
            this.radModerateIntensity.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // radHeavyIntensity
            // 
            this.radHeavyIntensity.AutoSize = true;
            this.radHeavyIntensity.Location = new System.Drawing.Point(381, 342);
            this.radHeavyIntensity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radHeavyIntensity.Name = "radHeavyIntensity";
            this.radHeavyIntensity.Size = new System.Drawing.Size(107, 24);
            this.radHeavyIntensity.TabIndex = 13;
            this.radHeavyIntensity.Text = "Ridiculous";
            this.radHeavyIntensity.UseVisualStyleBackColor = true;
            this.radHeavyIntensity.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // radInsaneIntensity
            // 
            this.radInsaneIntensity.AutoSize = true;
            this.radInsaneIntensity.Checked = true;
            this.radInsaneIntensity.Location = new System.Drawing.Point(502, 342);
            this.radInsaneIntensity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.radInsaneIntensity.Name = "radInsaneIntensity";
            this.radInsaneIntensity.Size = new System.Drawing.Size(134, 24);
            this.radInsaneIntensity.TabIndex = 14;
            this.radInsaneIntensity.TabStop = true;
            this.radInsaneIntensity.Text = "LUDICROUS!";
            this.radInsaneIntensity.UseVisualStyleBackColor = true;
            this.radInsaneIntensity.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // btnRandomize
            // 
            this.btnRandomize.Location = new System.Drawing.Point(666, 340);
            this.btnRandomize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRandomize.Name = "btnRandomize";
            this.btnRandomize.Size = new System.Drawing.Size(112, 35);
            this.btnRandomize.TabIndex = 15;
            this.btnRandomize.Text = "Randomize!";
            this.btnRandomize.UseVisualStyleBackColor = true;
            this.btnRandomize.Click += new System.EventHandler(this.btnRandomize_Click);
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(672, 117);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(112, 35);
            this.btnCompare.TabIndex = 4;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // txtSeed
            // 
            this.txtSeed.Location = new System.Drawing.Point(105, 243);
            this.txtSeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(148, 26);
            this.txtSeed.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 246);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 20);
            this.label3.TabIndex = 20;
            this.label3.Text = "Seed";
            // 
            // btnCompareBrowse
            // 
            this.btnCompareBrowse.Location = new System.Drawing.Point(672, 72);
            this.btnCompareBrowse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCompareBrowse.Name = "btnCompareBrowse";
            this.btnCompareBrowse.Size = new System.Drawing.Size(112, 35);
            this.btnCompareBrowse.TabIndex = 3;
            this.btnCompareBrowse.Text = "Browse";
            this.btnCompareBrowse.UseVisualStyleBackColor = true;
            this.btnCompareBrowse.Click += new System.EventHandler(this.btnCompareBrowse_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 75);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 20);
            this.label5.TabIndex = 24;
            this.label5.Text = "Comparison Image";
            // 
            // txtCompare
            // 
            this.txtCompare.Location = new System.Drawing.Point(183, 75);
            this.txtCompare.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtCompare.Name = "txtCompare";
            this.txtCompare.Size = new System.Drawing.Size(478, 26);
            this.txtCompare.TabIndex = 2;
            // 
            // btnNewSeed
            // 
            this.btnNewSeed.Location = new System.Drawing.Point(280, 240);
            this.btnNewSeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnNewSeed.Name = "btnNewSeed";
            this.btnNewSeed.Size = new System.Drawing.Size(112, 35);
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
            this.tabAll.Location = new System.Drawing.Point(18, 377);
            this.tabAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabAll.Name = "tabAll";
            this.tabAll.SelectedIndex = 0;
            this.tabAll.Size = new System.Drawing.Size(760, 303);
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
            this.adjustments.Location = new System.Drawing.Point(4, 29);
            this.adjustments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.adjustments.Name = "adjustments";
            this.adjustments.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.adjustments.Size = new System.Drawing.Size(752, 270);
            this.adjustments.TabIndex = 0;
            this.adjustments.Text = "Adjustments";
            this.adjustments.UseVisualStyleBackColor = true;
            // 
            // chkAllDogs
            // 
            this.chkAllDogs.AutoSize = true;
            this.chkAllDogs.Location = new System.Drawing.Point(370, 132);
            this.chkAllDogs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkAllDogs.Name = "chkAllDogs";
            this.chkAllDogs.Size = new System.Drawing.Size(187, 24);
            this.chkAllDogs.TabIndex = 25;
            this.chkAllDogs.Text = "Turn all NPCs to dogs";
            this.chkAllDogs.UseVisualStyleBackColor = true;
            // 
            // chkExperimental
            // 
            this.chkExperimental.AutoSize = true;
            this.chkExperimental.Location = new System.Drawing.Point(370, 217);
            this.chkExperimental.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkExperimental.Name = "chkExperimental";
            this.chkExperimental.Size = new System.Drawing.Size(174, 24);
            this.chkExperimental.TabIndex = 24;
            this.chkExperimental.Text = "Text speed hack (a)";
            this.chkExperimental.UseVisualStyleBackColor = true;
            this.chkExperimental.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkSpeedHacks
            // 
            this.chkSpeedHacks.AutoSize = true;
            this.chkSpeedHacks.Location = new System.Drawing.Point(370, 177);
            this.chkSpeedHacks.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSpeedHacks.Name = "chkSpeedHacks";
            this.chkSpeedHacks.Size = new System.Drawing.Size(181, 24);
            this.chkSpeedHacks.TabIndex = 23;
            this.chkSpeedHacks.Text = "Speed up battles (A)";
            this.chkSpeedHacks.UseVisualStyleBackColor = true;
            this.chkSpeedHacks.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(9, 222);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(115, 20);
            this.label12.TabIndex = 22;
            this.label12.Text = "Princess Name";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 180);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(99, 20);
            this.label11.TabIndex = 21;
            this.label11.Text = "Prince Name";
            // 
            // txtPrincessName
            // 
            this.txtPrincessName.Location = new System.Drawing.Point(172, 217);
            this.txtPrincessName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPrincessName.Name = "txtPrincessName";
            this.txtPrincessName.Size = new System.Drawing.Size(148, 26);
            this.txtPrincessName.TabIndex = 20;
            // 
            // txtPrinceName
            // 
            this.txtPrinceName.Location = new System.Drawing.Point(172, 177);
            this.txtPrinceName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPrinceName.Name = "txtPrinceName";
            this.txtPrinceName.Size = new System.Drawing.Size(148, 26);
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
            this.cboEncounterRate.Location = new System.Drawing.Point(172, 135);
            this.cboEncounterRate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboEncounterRate.Name = "cboEncounterRate";
            this.cboEncounterRate.Size = new System.Drawing.Size(180, 28);
            this.cboEncounterRate.TabIndex = 18;
            this.cboEncounterRate.SelectedIndexChanged += new System.EventHandler(this.determineFlags);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 140);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(122, 20);
            this.label10.TabIndex = 17;
            this.label10.Text = "Encounter Rate";
            // 
            // chkGPRandomize
            // 
            this.chkGPRandomize.AutoSize = true;
            this.chkGPRandomize.Location = new System.Drawing.Point(370, 91);
            this.chkGPRandomize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkGPRandomize.Name = "chkGPRandomize";
            this.chkGPRandomize.Size = new System.Drawing.Size(232, 24);
            this.chkGPRandomize.TabIndex = 16;
            this.chkGPRandomize.Text = "Randomize Monster GP (G)";
            this.chkGPRandomize.UseVisualStyleBackColor = true;
            this.chkGPRandomize.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkXPRandomize
            // 
            this.chkXPRandomize.AutoSize = true;
            this.chkXPRandomize.Location = new System.Drawing.Point(370, 52);
            this.chkXPRandomize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkXPRandomize.Name = "chkXPRandomize";
            this.chkXPRandomize.Size = new System.Drawing.Size(228, 24);
            this.chkXPRandomize.TabIndex = 15;
            this.chkXPRandomize.Text = "Randomize Monster XP (X)";
            this.chkXPRandomize.UseVisualStyleBackColor = true;
            this.chkXPRandomize.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 98);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(147, 20);
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
            this.cboGPReq.Location = new System.Drawing.Point(172, 94);
            this.cboGPReq.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboGPReq.Name = "cboGPReq";
            this.cboGPReq.Size = new System.Drawing.Size(180, 28);
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
            this.cboXPReq.Location = new System.Drawing.Point(172, 52);
            this.cboXPReq.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboXPReq.Name = "cboXPReq";
            this.cboXPReq.Size = new System.Drawing.Size(180, 28);
            this.cboXPReq.TabIndex = 12;
            this.cboXPReq.SelectedIndexChanged += new System.EventHandler(this.determineFlags);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 57);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 20);
            this.label8.TabIndex = 11;
            this.label8.Text = "XP Requirements";
            // 
            // chkChangeStatsToRemix
            // 
            this.chkChangeStatsToRemix.AutoSize = true;
            this.chkChangeStatsToRemix.Location = new System.Drawing.Point(12, 9);
            this.chkChangeStatsToRemix.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkChangeStatsToRemix.Name = "chkChangeStatsToRemix";
            this.chkChangeStatsToRemix.Size = new System.Drawing.Size(485, 24);
            this.chkChangeStatsToRemix.TabIndex = 8;
            this.chkChangeStatsToRemix.Text = "Change all monster stats and weapon values to remix values (R)";
            this.chkChangeStatsToRemix.UseVisualStyleBackColor = true;
            this.chkChangeStatsToRemix.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // tabPage2
            // 
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
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(752, 270);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Randomization";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkSmallMap
            // 
            this.chkSmallMap.AutoSize = true;
            this.chkSmallMap.Location = new System.Drawing.Point(9, 43);
            this.chkSmallMap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSmallMap.Name = "chkSmallMap";
            this.chkSmallMap.Size = new System.Drawing.Size(154, 24);
            this.chkSmallMap.TabIndex = 11;
            this.chkSmallMap.Text = "128x128 Map (u)";
            this.chkSmallMap.UseVisualStyleBackColor = true;
            this.chkSmallMap.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkTreasures
            // 
            this.chkTreasures.AutoSize = true;
            this.chkTreasures.Location = new System.Drawing.Point(354, 148);
            this.chkTreasures.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkTreasures.Name = "chkTreasures";
            this.chkTreasures.Size = new System.Drawing.Size(214, 24);
            this.chkTreasures.TabIndex = 10;
            this.chkTreasures.Text = "Randomize Treasures (T)";
            this.chkTreasures.UseVisualStyleBackColor = true;
            this.chkTreasures.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkSpellStrengths
            // 
            this.chkSpellStrengths.AutoSize = true;
            this.chkSpellStrengths.Location = new System.Drawing.Point(354, 46);
            this.chkSpellStrengths.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSpellStrengths.Name = "chkSpellStrengths";
            this.chkSpellStrengths.Size = new System.Drawing.Size(254, 24);
            this.chkSpellStrengths.TabIndex = 9;
            this.chkSpellStrengths.Text = "Randomize Spell Strengths (S)";
            this.chkSpellStrengths.UseVisualStyleBackColor = true;
            this.chkSpellStrengths.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkHeroStats
            // 
            this.chkHeroStats.AutoSize = true;
            this.chkHeroStats.Location = new System.Drawing.Point(354, 80);
            this.chkHeroStats.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkHeroStats.Name = "chkHeroStats";
            this.chkHeroStats.Size = new System.Drawing.Size(223, 24);
            this.chkHeroStats.TabIndex = 8;
            this.chkHeroStats.Text = "Randomize Hero Stats (H)";
            this.chkHeroStats.UseVisualStyleBackColor = true;
            this.chkHeroStats.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkHeroStores
            // 
            this.chkHeroStores.AutoSize = true;
            this.chkHeroStores.Location = new System.Drawing.Point(354, 114);
            this.chkHeroStores.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkHeroStores.Name = "chkHeroStores";
            this.chkHeroStores.Size = new System.Drawing.Size(192, 24);
            this.chkHeroStores.TabIndex = 7;
            this.chkHeroStores.Text = "Randomize Stores (C)";
            this.chkHeroStores.UseVisualStyleBackColor = true;
            this.chkHeroStores.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkSpellLearning
            // 
            this.chkSpellLearning.AutoSize = true;
            this.chkSpellLearning.Location = new System.Drawing.Point(354, 11);
            this.chkSpellLearning.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSpellLearning.Name = "chkSpellLearning";
            this.chkSpellLearning.Size = new System.Drawing.Size(244, 24);
            this.chkSpellLearning.TabIndex = 6;
            this.chkSpellLearning.Text = "Randomize Spell Learning (L)";
            this.chkSpellLearning.UseVisualStyleBackColor = true;
            this.chkSpellLearning.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkMonsterStats
            // 
            this.chkMonsterStats.AutoSize = true;
            this.chkMonsterStats.Location = new System.Drawing.Point(9, 178);
            this.chkMonsterStats.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkMonsterStats.Name = "chkMonsterStats";
            this.chkMonsterStats.Size = new System.Drawing.Size(247, 24);
            this.chkMonsterStats.TabIndex = 5;
            this.chkMonsterStats.Text = "Randomize Monster Stats (M)";
            this.chkMonsterStats.UseVisualStyleBackColor = true;
            this.chkMonsterStats.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkMonsterZones
            // 
            this.chkMonsterZones.AutoSize = true;
            this.chkMonsterZones.Location = new System.Drawing.Point(9, 212);
            this.chkMonsterZones.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkMonsterZones.Name = "chkMonsterZones";
            this.chkMonsterZones.Size = new System.Drawing.Size(251, 24);
            this.chkMonsterZones.TabIndex = 4;
            this.chkMonsterZones.Text = "Randomize Monster Zones (Z)";
            this.chkMonsterZones.UseVisualStyleBackColor = true;
            this.chkMonsterZones.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkEquipEffects
            // 
            this.chkEquipEffects.AutoSize = true;
            this.chkEquipEffects.Location = new System.Drawing.Point(9, 145);
            this.chkEquipEffects.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkEquipEffects.Name = "chkEquipEffects";
            this.chkEquipEffects.Size = new System.Drawing.Size(277, 24);
            this.chkEquipEffects.TabIndex = 3;
            this.chkEquipEffects.Text = "Randomize Equipment Effects (E)";
            this.chkEquipEffects.UseVisualStyleBackColor = true;
            this.chkEquipEffects.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkEquipment
            // 
            this.chkEquipment.AutoSize = true;
            this.chkEquipment.Location = new System.Drawing.Point(9, 77);
            this.chkEquipment.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkEquipment.Name = "chkEquipment";
            this.chkEquipment.Size = new System.Drawing.Size(223, 24);
            this.chkEquipment.TabIndex = 2;
            this.chkEquipment.Text = "Randomize Equipment (Q)";
            this.chkEquipment.UseVisualStyleBackColor = true;
            this.chkEquipment.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkWhoCanEquip
            // 
            this.chkWhoCanEquip.AutoSize = true;
            this.chkWhoCanEquip.Location = new System.Drawing.Point(9, 111);
            this.chkWhoCanEquip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkWhoCanEquip.Name = "chkWhoCanEquip";
            this.chkWhoCanEquip.Size = new System.Drawing.Size(260, 24);
            this.chkWhoCanEquip.TabIndex = 1;
            this.chkWhoCanEquip.Text = "Randomize Who Can Equip (W)";
            this.chkWhoCanEquip.UseVisualStyleBackColor = true;
            this.chkWhoCanEquip.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkMap
            // 
            this.chkMap.AutoSize = true;
            this.chkMap.Location = new System.Drawing.Point(9, 9);
            this.chkMap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkMap.Name = "chkMap";
            this.chkMap.Size = new System.Drawing.Size(177, 24);
            this.chkMap.TabIndex = 0;
            this.chkMap.Text = "Randomize Map (U)";
            this.chkMap.UseVisualStyleBackColor = true;
            this.chkMap.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnLudicrousRando);
            this.tabPage1.Controls.Add(this.btnUltraRando);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(752, 270);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Shortcuts";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnLudicrousRando
            // 
            this.btnLudicrousRando.Location = new System.Drawing.Point(20, 65);
            this.btnLudicrousRando.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLudicrousRando.Name = "btnLudicrousRando";
            this.btnLudicrousRando.Size = new System.Drawing.Size(250, 35);
            this.btnLudicrousRando.TabIndex = 1;
            this.btnLudicrousRando.Text = "Ludicrous Randomization";
            this.btnLudicrousRando.UseVisualStyleBackColor = true;
            // 
            // btnUltraRando
            // 
            this.btnUltraRando.Location = new System.Drawing.Point(20, 20);
            this.btnUltraRando.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUltraRando.Name = "btnUltraRando";
            this.btnUltraRando.Size = new System.Drawing.Size(250, 35);
            this.btnUltraRando.TabIndex = 0;
            this.btnUltraRando.Text = "Ultra Randomization";
            this.btnUltraRando.UseVisualStyleBackColor = true;
            this.btnUltraRando.Click += new System.EventHandler(this.btnUltraRando_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(20, 348);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(182, 20);
            this.label6.TabIndex = 27;
            this.label6.Text = "Amount of Randomness";
            // 
            // lblIntensityDesc
            // 
            this.lblIntensityDesc.AutoSize = true;
            this.lblIntensityDesc.Location = new System.Drawing.Point(22, 715);
            this.lblIntensityDesc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIntensityDesc.Name = "lblIntensityDesc";
            this.lblIntensityDesc.Size = new System.Drawing.Size(0, 20);
            this.lblIntensityDesc.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 294);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 20);
            this.label7.TabIndex = 29;
            this.label7.Text = "Flags";
            // 
            // txtFlags
            // 
            this.txtFlags.Location = new System.Drawing.Point(105, 289);
            this.txtFlags.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFlags.Name = "txtFlags";
            this.txtFlags.Size = new System.Drawing.Size(286, 26);
            this.txtFlags.TabIndex = 30;
            this.txtFlags.TextChanged += new System.EventHandler(this.txtFlags_TextChanged);
            // 
            // lblNewChecksum
            // 
            this.lblNewChecksum.AutoSize = true;
            this.lblNewChecksum.Location = new System.Drawing.Point(178, 195);
            this.lblNewChecksum.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNewChecksum.Name = "lblNewChecksum";
            this.lblNewChecksum.Size = new System.Drawing.Size(369, 20);
            this.lblNewChecksum.TabIndex = 32;
            this.lblNewChecksum.Text = "????????????????????????????????????????";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 195);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(119, 20);
            this.label14.TabIndex = 31;
            this.label14.Text = "New Checksum";
            // 
            // btnCopyChecksum
            // 
            this.btnCopyChecksum.Location = new System.Drawing.Point(603, 188);
            this.btnCopyChecksum.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCopyChecksum.Name = "btnCopyChecksum";
            this.btnCopyChecksum.Size = new System.Drawing.Size(182, 35);
            this.btnCopyChecksum.TabIndex = 33;
            this.btnCopyChecksum.Text = "Copy New Checksum";
            this.btnCopyChecksum.UseVisualStyleBackColor = true;
            this.btnCopyChecksum.Click += new System.EventHandler(this.btnCopyChecksum_Click);
            // 
            // chkSpeedWaitMusic
            // 
            this.chkSpeedWaitMusic.AutoSize = true;
            this.chkSpeedWaitMusic.Location = new System.Drawing.Point(544, 218);
            this.chkSpeedWaitMusic.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSpeedWaitMusic.Name = "chkSpeedWaitMusic";
            this.chkSpeedWaitMusic.Size = new System.Drawing.Size(197, 24);
            this.chkSpeedWaitMusic.TabIndex = 26;
            this.chkSpeedWaitMusic.Text = "Speedy Wait Music (m)";
            this.chkSpeedWaitMusic.UseVisualStyleBackColor = true;
            this.chkSpeedWaitMusic.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 766);
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
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Dragon Warrior 2 Randomizer";
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
    }
}

