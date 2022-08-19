namespace PrinterStocks
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.logEssentials = new System.Windows.Forms.CheckBox();
            this.notificationFrequency = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.websites = new System.Windows.Forms.TextBox();
            this.blackCartridgeCondition = new System.Windows.Forms.NumericUpDown();
            this.paperTrayCondition = new System.Windows.Forms.ComboBox();
            this.refreshRate = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.recipientEmails = new System.Windows.Forms.TextBox();
            this.ccEmails = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.autoRefresh = new System.Windows.Forms.CheckBox();
            this.playSound = new System.Windows.Forms.CheckBox();
            this.sendEmail = new System.Windows.Forms.CheckBox();
            this.save = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.notificationFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackCartridgeCondition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.refreshRate)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.logEssentials);
            this.groupBox1.Controls.Add(this.notificationFrequency);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.websites);
            this.groupBox1.Controls.Add(this.blackCartridgeCondition);
            this.groupBox1.Controls.Add(this.paperTrayCondition);
            this.groupBox1.Controls.Add(this.refreshRate);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.recipientEmails);
            this.groupBox1.Controls.Add(this.ccEmails);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.autoRefresh);
            this.groupBox1.Controls.Add(this.playSound);
            this.groupBox1.Controls.Add(this.sendEmail);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(658, 228);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Notification Settings";
            // 
            // logEssentials
            // 
            this.logEssentials.AutoSize = true;
            this.logEssentials.Location = new System.Drawing.Point(6, 102);
            this.logEssentials.Name = "logEssentials";
            this.logEssentials.Size = new System.Drawing.Size(122, 21);
            this.logEssentials.TabIndex = 18;
            this.logEssentials.Text = "Log Essentials";
            this.toolTip1.SetToolTip(this.logEssentials, "Only log essential notifications.");
            this.logEssentials.UseVisualStyleBackColor = true;
            // 
            // notificationFrequency
            // 
            this.notificationFrequency.Increment = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.notificationFrequency.Location = new System.Drawing.Point(532, 106);
            this.notificationFrequency.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.notificationFrequency.Name = "notificationFrequency";
            this.notificationFrequency.ReadOnly = true;
            this.notificationFrequency.Size = new System.Drawing.Size(120, 22);
            this.notificationFrequency.TabIndex = 17;
            this.toolTip1.SetToolTip(this.notificationFrequency, "Rate in minutes the application will re-notify the user if no change was detected" +
        ".");
            this.notificationFrequency.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(197, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(189, 17);
            this.label7.TabIndex = 16;
            this.label7.Text = "Notification Frequency (min):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 203);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 17);
            this.label6.TabIndex = 15;
            this.label6.Text = "Websites to Monitor:";
            this.toolTip1.SetToolTip(this.label6, "Websites seperated by commas that the application will grab stocks from.");
            // 
            // websites
            // 
            this.websites.Location = new System.Drawing.Point(200, 200);
            this.websites.Name = "websites";
            this.websites.Size = new System.Drawing.Size(452, 22);
            this.websites.TabIndex = 14;
            this.toolTip1.SetToolTip(this.websites, "Websites seperated by commas that the application will grab stocks from.");
            // 
            // blackCartridgeCondition
            // 
            this.blackCartridgeCondition.Location = new System.Drawing.Point(532, 47);
            this.blackCartridgeCondition.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.blackCartridgeCondition.Name = "blackCartridgeCondition";
            this.blackCartridgeCondition.ReadOnly = true;
            this.blackCartridgeCondition.Size = new System.Drawing.Size(120, 22);
            this.blackCartridgeCondition.TabIndex = 1;
            this.toolTip1.SetToolTip(this.blackCartridgeCondition, "Threshold of Black Cartridge percentage that dictates a notification.");
            this.blackCartridgeCondition.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // paperTrayCondition
            // 
            this.paperTrayCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.paperTrayCondition.Items.AddRange(new object[] {
            "Empty",
            "Low"});
            this.paperTrayCondition.Location = new System.Drawing.Point(532, 19);
            this.paperTrayCondition.Name = "paperTrayCondition";
            this.paperTrayCondition.Size = new System.Drawing.Size(120, 24);
            this.paperTrayCondition.TabIndex = 13;
            this.toolTip1.SetToolTip(this.paperTrayCondition, "Threshold of Paper Trays that dictates a notification.");
            // 
            // refreshRate
            // 
            this.refreshRate.Location = new System.Drawing.Point(532, 78);
            this.refreshRate.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.refreshRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.refreshRate.Name = "refreshRate";
            this.refreshRate.ReadOnly = true;
            this.refreshRate.Size = new System.Drawing.Size(120, 22);
            this.refreshRate.TabIndex = 12;
            this.toolTip1.SetToolTip(this.refreshRate, "Rate in minutes to refresh printer stock.");
            this.refreshRate.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(197, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(165, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Auto Refresh Rate (min):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(197, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(211, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Black Cartridge Notify Condition:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(197, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(186, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Paper Tray Notify Condition:";
            // 
            // recipientEmails
            // 
            this.recipientEmails.Location = new System.Drawing.Point(200, 172);
            this.recipientEmails.Name = "recipientEmails";
            this.recipientEmails.Size = new System.Drawing.Size(452, 22);
            this.recipientEmails.TabIndex = 7;
            this.toolTip1.SetToolTip(this.recipientEmails, "Email addresses seperated by commas that will be notified when a condition is met" +
        ".");
            // 
            // ccEmails
            // 
            this.ccEmails.Location = new System.Drawing.Point(200, 144);
            this.ccEmails.Name = "ccEmails";
            this.ccEmails.Size = new System.Drawing.Size(452, 22);
            this.ccEmails.TabIndex = 6;
            this.toolTip1.SetToolTip(this.ccEmails, "Email addresses seperated by commas to CC when a condition is met.");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 149);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "CC Email Addresses:";
            this.toolTip1.SetToolTip(this.label2, "Email addresses seperated by commas to CC when a condition is met.");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Reciepient Email Addresses:";
            this.toolTip1.SetToolTip(this.label1, "Email addresses seperated by commas that will be notified when a condition is met" +
        ".");
            // 
            // autoRefresh
            // 
            this.autoRefresh.AutoSize = true;
            this.autoRefresh.Location = new System.Drawing.Point(6, 75);
            this.autoRefresh.Name = "autoRefresh";
            this.autoRefresh.Size = new System.Drawing.Size(113, 21);
            this.autoRefresh.TabIndex = 3;
            this.autoRefresh.Text = "Auto Refresh";
            this.toolTip1.SetToolTip(this.autoRefresh, "Auto refresh printer stocks.");
            this.autoRefresh.UseVisualStyleBackColor = true;
            // 
            // playSound
            // 
            this.playSound.AutoSize = true;
            this.playSound.Location = new System.Drawing.Point(6, 48);
            this.playSound.Name = "playSound";
            this.playSound.Size = new System.Drawing.Size(102, 21);
            this.playSound.TabIndex = 1;
            this.playSound.Text = "Play Sound";
            this.toolTip1.SetToolTip(this.playSound, "Play a looping sound when notified.");
            this.playSound.UseVisualStyleBackColor = true;
            // 
            // sendEmail
            // 
            this.sendEmail.AutoSize = true;
            this.sendEmail.Location = new System.Drawing.Point(6, 21);
            this.sendEmail.Name = "sendEmail";
            this.sendEmail.Size = new System.Drawing.Size(101, 21);
            this.sendEmail.TabIndex = 0;
            this.sendEmail.Text = "Send Email";
            this.toolTip1.SetToolTip(this.sendEmail, "Send email when notified.");
            this.sendEmail.UseVisualStyleBackColor = true;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(238, 246);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(100, 30);
            this.save.TabIndex = 1;
            this.save.Text = "Save";
            this.save.UseVisualStyleBackColor = true;
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // cancel
            // 
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(344, 246);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(100, 30);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(682, 288);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.save);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.notificationFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blackCartridgeCondition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.refreshRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button save;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.CheckBox playSound;
        private System.Windows.Forms.CheckBox sendEmail;
        private System.Windows.Forms.TextBox recipientEmails;
        private System.Windows.Forms.TextBox ccEmails;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox autoRefresh;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown refreshRate;
        private System.Windows.Forms.ComboBox paperTrayCondition;
        private System.Windows.Forms.NumericUpDown blackCartridgeCondition;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox websites;
        private System.Windows.Forms.NumericUpDown notificationFrequency;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox logEssentials;
    }
}