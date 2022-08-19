using System;
using System.IO;
using System.Windows.Forms;

namespace PrinterStocks
{
    public partial class Form2 : Form
    {
        private Form1 form;

        public Form2(Form1 form)
        {
            this.form = form;
            InitializeComponent();
            InitializeSettings();
        }

        public void SaveSettings()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string fileName = "/config.bin";
            string tmp = path + fileName;
            bool fileExists = File.Exists(tmp);
            if (!fileExists)
            {
                using (File.Create(tmp))
                    form.log("Configuration file not found. Creating file.");
            }
            string[] defaults = {
                            "Send Email=",
                            "Play Sound=",
                            "Auto Refresh=",
                            "Log Essentials=",
                            "CC Emails=",
                            "Recipient Emails=",
                            "Printer Websites=",
                            "Paper Tray Condition=",
                            "Black Cartridge Condition=",
                            "Refresh Rate=",
                            "Notification Frequency=" };

            for (int idx = 0; idx < defaults.Length; idx++)
            {
                switch (idx)
                {
                    case 0:
                        Globals.emailNotify = this.sendEmail.Checked;
                        defaults[idx] += this.sendEmail.Checked.ToString();
                        break;
                    case 1:
                        Globals.soundNotify = this.playSound.Checked;
                        defaults[idx] += this.playSound.Checked.ToString();
                        break;
                    case 2:
                        Globals.autoRefresh = this.autoRefresh.Checked;
                        defaults[idx] += this.autoRefresh.Checked.ToString();
                        break;
                    case 3:
                        Globals.logEssentials = this.logEssentials.Checked;
                        defaults[idx] += this.logEssentials.Checked.ToString();
                        break;
                    case 4:
                        Globals.ccEmails = this.ccEmails.Text;
                        defaults[idx] += this.ccEmails.Text.ToString();
                        break;
                    case 5:
                        Globals.recipientEmails = this.recipientEmails.Text;
                        defaults[idx] += this.recipientEmails.Text.ToString();
                        break;
                    case 6:
                        Globals.websites = this.websites.Text.Split(',');
                        defaults[idx] += this.websites.Text;
                        break;
                    case 7:
                        Globals.paperTrayCondition = form.StringToStatus(this.paperTrayCondition.Text);
                        defaults[idx] += this.paperTrayCondition.Text.ToString();
                        break;
                    case 8:
                        Globals.blackCartridgeCondition = Int32.Parse(this.blackCartridgeCondition.Value.ToString());
                        defaults[idx] += this.blackCartridgeCondition.Value.ToString();
                        break;
                    case 9:
                        Globals.refreshRate = Int32.Parse(this.refreshRate.Value.ToString());
                        defaults[idx] += this.refreshRate.Value.ToString();
                        break;
                    case 10:
                        Globals.notificationFrequency = Int32.Parse(this.notificationFrequency.Value.ToString());
                        defaults[idx] += this.notificationFrequency.Value.ToString();
                        break;
                }
            }
            File.WriteAllLines(tmp, defaults);
            form.log("Your configuration settings have been saved.");
        }

        public void InitializeSettings()
        {
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string fileName = "/config.bin";
            string tmp = path + fileName;
            bool fileExists = File.Exists(tmp);
            if (!fileExists)
            {
                using (File.Create(tmp))
                    form.log("Configuration file not found. Creating file.");
                string[] defaults = {
                            "Send Email=" + Globals.emailNotify,
                            "Play Sound=" + Globals.soundNotify,
                            "Auto Refresh=" + Globals.autoRefresh,
                            "Log Essentials=" + Globals.logEssentials,
                            "CC Emails=" + Globals.ccEmails,
                            "Recipient Emails=" + Globals.recipientEmails,
                            "Printer Websites=" + Globals.websitesString,
                            "Paper Tray Condition=" + Globals.paperTrayCondition,
                            "Black Cartridge Condition=" + Globals.blackCartridgeCondition,
                            "Refresh Rate=" + Globals.refreshRate,
                            "Notification Frequency=" + Globals.notificationFrequency };
                File.WriteAllLines(tmp, defaults);
            }
            string info;
            string[] fileInfo = File.ReadAllLines(tmp);
            for (int idx = 0; idx < fileInfo.Length; idx++)
            {
                info = fileInfo[idx];
                try
                {
                    var setting = info.Split('=')[0];
                    info = info.Split('=')[1];
                    if (info == null)
                        continue;
                    switch (setting)
                    {
                        case "Send Email":
                            this.sendEmail.Checked = Boolean.Parse(info);
                            break;
                        case "Play Sound":
                            this.playSound.Checked = Boolean.Parse(info);
                            break;
                        case "Auto Refresh":
                            this.autoRefresh.Checked = Boolean.Parse(info);
                            break;
                        case "Log Essentials":
                            this.logEssentials.Checked = Boolean.Parse(info);
                            break;
                        case "CC Emails":
                            this.ccEmails.Text = info.ToString();
                            break;
                        case "Recipient Emails":
                            this.recipientEmails.Text = info.ToString();
                            break;
                        case "Printer Websites":
                            this.websites.Text = info.ToString();
                            break;
                        case "Paper Tray Condition":
                            this.paperTrayCondition.Text = info.ToString();
                            break;
                        case "Black Cartridge Condition":
                            this.blackCartridgeCondition.Value = Int32.Parse(info);
                            break;
                        case "Refresh Rate":
                            this.refreshRate.Value = Int32.Parse(info);
                            break;
                        case "Notification Frequency":
                            this.notificationFrequency.Value = Int32.Parse(info);
                            break;
                        default:
                            form.log(setting + " is an invalid preference.");
                            break;
                    }
                }
                catch (IndexOutOfRangeException e) { }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "ERROR");
                }
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void save_Click(object sender, EventArgs e)
        {
            bool prev = Globals.autoRefresh;
            this.SaveSettings();
            if (Globals.isRunning)
            {
                if (prev != Globals.autoRefresh)
                {
                    if (this.autoRefresh.Checked)
                        form.StartTimer();
                    else
                        form.StopTimer();
                }
            }
            this.Close();
        }
    }
}
