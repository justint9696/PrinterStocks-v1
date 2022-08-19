using System;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using EASendMail;
using System.Media;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PrinterStocks
{
    public partial class Form1 : Form
    {
        private long startTime, timePassed;
        private string emailBody;
        private Section[] sections = new Section[Globals.MAX_WEBSITES];

        public Form1()
        {
            InitializeComponent();
            InitializeSettings();
            InitializeSections();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        private static extern bool FlashWindow(IntPtr hwnd, bool FlashStatus);

        private bool IsActive(IntPtr handle)
        {
            // Is the form window focused
            IntPtr activeHandle = GetForegroundWindow();
            return (activeHandle == handle);
        }

        private void InitializeSettings()
        {
            // Apply settings from 'config.bin'
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string fileName = "/config.bin";
            string tmp = path + fileName;
            bool fileExists = File.Exists(tmp);
            if (!fileExists)
            {
                // If config file is not found, prompt user for first time setup
                DialogResult res = MessageBox.Show("Configure Settings?", "First Time Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                switch (res)
                {
                    // Open Form2; preferences page
                    case DialogResult.Yes:
                        var form = new Form2(this);
                        form.Owner = this;
                        form.Show();
                        break;
                    default:
                        // Apply default settings
                        using (File.Create(tmp))
                            log("Configuration file not found. Creating file.");
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
                        break;
                }
            }

            // Read through each line of config file and apply settings accordingly
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
                            Globals.emailNotify = Boolean.Parse(info);
                            break;
                        case "Play Sound":
                            Globals.soundNotify = Boolean.Parse(info);
                            break;
                        case "Auto Refresh":
                            Globals.autoRefresh = Boolean.Parse(info);
                            break;
                        case "Log Essentials":
                            Globals.logEssentials = Boolean.Parse(info);
                            break;
                        case "CC Emails":
                            Globals.ccEmails = info.ToString();
                            break;
                        case "Recipient Emails":
                            Globals.recipientEmails = info.ToString();
                            break;
                        case "Printer Websites":
                            Globals.websites = info.ToString().Split(',');
                            break;
                        case "Paper Tray Condition":
                            Globals.paperTrayCondition = StringToStatus(info.ToString());
                            break;
                        case "Black Cartridge Condition":
                            Globals.blackCartridgeCondition = Int32.Parse(info);
                            break;
                        case "Refresh Rate":
                            Globals.refreshRate = Int32.Parse(info);
                            break;
                        case "Notification Frequency":
                            Globals.notificationFrequency = Int32.Parse(info);
                            break;
                        default:
                            log(setting + " is an invalid preference.");
                            break;
                    }
                }

                catch (IndexOutOfRangeException e) { }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            log("Settings applied from configuration file.");
        }

        private void InitializeSections()
        {
            // Create n Sections where n is the maximum number of websites allowed to monitor
            for (int i = 0; i < Globals.websites.Length; i++)
                sections[i] = new Section();
        }

        private void doFetch(bool init)
        {
            if (Globals.isRunning && init)
            {
                this.fetch.Text = "Fetch Printer Stocks";
                this.timer1.Stop();
                this.listBox1.Items.Clear();
                this.stocks.Nodes.Clear();
                Globals.isRunning = false;
                this.refreshToolStripMenuItem.Enabled = false;
                return;
            }
            bool wasRunning = Globals.isRunning;
            Globals.isRunning = this.FetchPrinterStocks(init, Globals.emailNotify, Globals.soundNotify);
            this.fetch.Text = Globals.isRunning ? "Stop Monitoring Stocks" : "Fetch Printer Stocks";
            this.refreshToolStripMenuItem.Enabled = Globals.isRunning;
            if (Globals.isRunning && !wasRunning)
            {
                Globals.startTime = this.GetCurrentTick();
                if (Globals.autoRefresh)
                    this.StartTimer();
                this.timer1.Start();
            }
            else if (!Globals.isRunning)
            {
                this.timer1.Stop();
                this.timer2.Stop();
                this.timer3.Stop();
            }
        }

        private void fetch_Click(object sender, EventArgs e)
        {
            if (Globals.logEssentials)
                log("Fetching printer stocks.");
            this.doFetch(true);
        }

        private string GrabHTML(string website)
        {
            // Grabs HTML code from a given website
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(website);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream receiveStream = response.GetResponseStream();
                    StreamReader readStream = null;
                    if (response.CharacterSet == null)
                        readStream = new StreamReader(receiveStream);
                    else
                        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    string data = readStream.ReadToEnd();
                    response.Close();
                    readStream.Close();

                    return data;
                }
            }
            catch (WebException e)
            {
                log("Could not reach website: " + website);
            }
            return null;
        }

        private string StripTags(string s)
        {
            return Regex.Replace(s, "<.*?>", String.Empty);
        }

        private bool FetchPrinterStocks(bool show, bool alertEmail, bool alertSound)
        {
            emailBody = "";
            int failCount = 0;
            int notifyCount = 0;
            bool failed = false;
            bool paperJam = false;
            bool shouldAlert = false;
            string website = "utep.edu";
            string sectionPaperJam = "System detected a Paper Jam at: ";
            string[] notifyMessages = new string[Globals.MAX_WEBSITES * 8]; // Max number of sites times total data from each site
            TrayStatus condition = Globals.paperTrayCondition;

            // Grab number of nodes
            int numNodes = this.stocks.Nodes.Count;

            // Clear all nodes from tree view if website count is not equal to node count
            bool isEqual = numNodes == Globals.websites.Length;
            if (!isEqual)
                this.stocks.Nodes.Clear();

            if (show && !Globals.logEssentials)
                log("Fetching printer stocks.");

            for (int section = 0; section < Globals.websites.Length; section++)
            {
                // node -> current child node
                int node = 0;

                if (show && !Globals.logEssentials)
                    log("Waiting for response from: " + Globals.websites[section] + "...");

                try
                {
                    TreeNode newNode = new TreeNode();

                    website = Globals.websites[section] + "/cgi-bin/dynamic/topbar.html";
                    var data = this.GrabHTML(website);

                    // Grab Location as listed on website; if not found, the node heading will have a generic name
                    var match = new Regex("Location: [a-zA-Z{0-9}?]*").Matches(data);
                    var title = match[0].ToString().Split(' ')[1];
                    newNode.Text = title.Length  > 0 ? title : "Section " + (section + 1);

                    website = Globals.websites[section] + "/cgi-bin/dynamic/printer/PrinterStatus.html";
                    data = this.GrabHTML(website);

                    var strip = Regex.Replace(this.StripTags(data), "&#032;", " ");

                    // Check for paper jam
                    match = new Regex("Paper jam").Matches(strip);
                    this.sections[section].paperJam = match.Count > 0;
                    paperJam = paperJam ? true : this.sections[section].paperJam;

                    // Update paper jam notify string
                    if (this.sections[section].paperJam)
                        sectionPaperJam += "\n - " + (title.Length > 0 ? title : "Section " + (section + 1));
                    
                    // Grab black cartridge percentage
                    match = new Regex("Black Cartridge .*[0-9]*%").Matches(data);
                    newNode.Nodes.Add(match[0].ToString());
                    if (isEqual)
                        this.stocks.Nodes[section].Nodes[node].Text = newNode.Nodes[node++].Text;

                    var percentage = new Regex("\\d+").Matches(match[0].ToString());
                    this.sections[section].cartridge = Int32.Parse(percentage[0].ToString());

                    // Grab statuses of all paper trays
                    match = new Regex("<b>(OK|Low|Empty)</b>").Matches(data);
                    for (int idx = 0; idx < match.Count; idx++)
                    {
                        string obj = "";
                        string stat = this.StripTags(match[idx].ToString());
                        switch (idx)
                        {
                            case 0:
                                obj = "Tray 1: ";
                                this.sections[section].topTray = this.StringToStatus(stat);
                                break;
                            case 1:
                                obj = "Tray 2: ";
                                this.sections[section].bottomTray = this.StringToStatus(stat);
                                break;
                            case 2:
                                obj = "Multi-Purpose Feeder: ";
                                this.sections[section].multiPurpose = this.StringToStatus(stat);
                                break;
                            case 3:
                                obj = "Standard Bin: ";
                                break;
                        }
                        newNode.Nodes.Add(obj + stat);
                        if (isEqual)
                            this.stocks.Nodes[section].Nodes[node].Text = newNode.Nodes[node++].Text;
                    }

                    // Grab maintenance kit life percentage
                    match = new Regex("Maintenance Kit Life Remaining:[0-9]*%").Matches(strip);
                    newNode.Nodes.Add(match[0].ToString().Replace(":", ": "));
                    if (isEqual)
                        this.stocks.Nodes[section].Nodes[node].Text = newNode.Nodes[node++].Text;

                    percentage = new Regex("\\d+").Matches(match[0].ToString());
                    this.sections[section].maintenanceKit = Int32.Parse(percentage[0].ToString());

                    // Grab roller kit life percentage
                    match = new Regex("Roller Kit Life Remaining:[0-9]*%").Matches(strip);
                    newNode.Nodes.Add(match[0].ToString().Replace(":", ": "));
                    if (isEqual)
                        this.stocks.Nodes[section].Nodes[node].Text = newNode.Nodes[node++].Text;

                    percentage = new Regex("\\d+").Matches(match[0].ToString());
                    this.sections[section].rollerKit = Int32.Parse(percentage[0].ToString());

                    // Grab imaging unit life percentage
                    match = new Regex("Imaging Unit Life Remaining:[0-9]*%").Matches(strip);
                    newNode.Nodes.Add(match[0].ToString().Replace(":", ": "));
                    if (isEqual)
                        this.stocks.Nodes[section].Nodes[node].Text = newNode.Nodes[node++].Text;

                    percentage = new Regex("\\d+").Matches(match[0].ToString());
                    this.sections[section].imagingUnit = Int32.Parse(percentage[0].ToString());

                    // Add node to tree
                    if (!isEqual)
                        this.stocks.Nodes.Add(newNode);

                    // Grabs messages from sections and checks if a condition is met
                    bool found = true;
                    string[] messages = this.sections[section].getMessages(condition, Globals.blackCartridgeCondition);
                    for (int idx = 0; idx < messages.Length; idx++)
                    {
                        var message = messages[idx];
                        if (message == null)
                            continue;

                        if (found)
                        {
                            found = false;
                            shouldAlert = true;

                            emailBody += "<h3>";
                            emailBody += title.Length > 0 ? title : "Section " + (section + 1);
                            emailBody += "</h3>";
                        }
                        emailBody += " - " + message + "<br>";
                        message = (title.Length > 0 ? title : "Section " + (section + 1)) + ": " + message;
                        notifyMessages[notifyCount] = message;
                        notifyCount += 1;
                    }
                }
                catch (Exception e)
                {
                    failCount++;
                    failed = true;
                    if (show)
                        log("Could not reach website: " + website);
                    Console.WriteLine(e.ToString());
                }
            }

            this.stocks.ExpandAll();

            if (!Globals.logEssentials)
            {
                if (!failed)
                    log("Process successfully fetched stocks from all websites.");
            }
            if (failed)
                log("Process failed to connect to " + failCount + " website(s).");
            if (shouldAlert)
            {
                if (alertEmail)
                    this.SendEmail("Printer Stocks Require Attention", emailBody);
                if (alertSound && !paperJam)
                {
                    this.Alert();
                    if (!this.IsActive(this.Handle))
                    {
                        this.timer2.Start();
                        FlashWindow(this.Handle, true);
                    }
                }
                else if (paperJam)
                {
                    FlashWindow(this.Handle, true);
                    log("A Paper Jam was detected!");
                    this.timer3.Start();
                    DialogResult res = MessageBox.Show(sectionPaperJam, "Attention Required", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    switch (res)
                    {
                        case DialogResult.Retry:
                            this.timer3.Stop();
                            if (!Globals.logEssentials)
                                log("Refreshing stocks.");
                            return this.FetchPrinterStocks(false, false, alertSound);
                        default:
                            this.timer3.Stop();
                            break;
                    }
                }
            }

            for (int idx = 0; idx < notifyCount; idx++)
            {
                var message = notifyMessages[idx] + "\n";
                log(message);
            }

            Globals.notify = Globals.notify ? false : Globals.notify;

            // Ensure the data from at least one website was retrieved
            return failCount < Globals.websites.Length;
        }

        public void log(string message)
        {
            // Displays a message with timestamp to user
            message = "[" + this.GetCurrentTime() + "] " + message;;
            this.listBox1.Items.Add(message);
            this.listBox1.SelectedIndex = this.listBox1.Items.Count - 1;
            this.listBox1.SelectedIndex = -1;
        }

        private void SendEmail(string subject, string body)
        {
            // Composes email with a customized subject, body, and recipient(s)
            string[] emails = Globals.recipientEmails.Split(',');
            foreach (var email in emails)
            {
                if (!IsValid(email))
                {
                    log("Please enter a valid email.");
                    MessageBox.Show("Invalid Email", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }
                try
                {
                    var mail = new SmtpMail("TryIt");
                    mail.From = new MailAddress("UTEP Library Alerts", Globals.email.username);
                    mail.To = email;
                    mail.Cc = new AddressCollection();
                    foreach (var address in Globals.ccEmails.Split(','))
                        mail.Cc.Add(address);
                    mail.Subject = subject;
                    mail.HtmlBody = body;

                    var server = new SmtpServer("smtp.office365.com");
                    server.User = Globals.email.username;
                    server.Password = Globals.email.password;

                    server.ConnectType = SmtpConnectType.ConnectTryTLS;
                    server.Port = 587;

                    var client = new SmtpClient();
                    client.SendMail(server, mail);
                    client.Quit();

                    log("Email sent to: " + email);
                }
                catch (Exception e)
                {
                    log("An error occurred.");
                    MessageBox.Show(e.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void notifyEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SendEmail("Test Email", "This is a test of the UTEP Library Alerts email system.");
        }

        private bool IsValid(string email)
        {
            // Attempts to find invalid emails
            if (email.Split('@').Length != 2)
                return false;
            email = email.Split('@')[1];
            if (email.Length > 5) {
                switch (email)
                {
                    case "yopmail.com":
                    case "utep.edu":
                    case "miners.utep.edu":
                    case "gmail.com":
                    case "outlook.com":
                    case "hotmail.com":
                    case "msn.com":
                    case "yahoo.com":
                    case "inbox.com":
                    case "icloud.com":
                    case "mail.com":
                        return new Regex("#|\"|\\..|!").Matches(email).Count > 0;
                }
            }
            return false;
        }

        private void notifySoundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Alert();
        }

        private void Alert()
        {
            try
            {
                using (var sound = new SoundPlayer("alert0.wav"))
                {
                    sound.Play();
                }
            } 
            catch (Exception e)
            {
                SystemSounds.Exclamation.Play();
            }
        }

        private string GetCurrentTime()
        {
            return DateTime.Now.ToString("hh:mm:ss");
        }

        private long GetCurrentTick()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public void StartTimer()
        {
            this.startTime = this.GetCurrentTick();
            log("Auto refresh timer started.");
        }

        public void StopTimer()
        {
            log("Auto refresh timer stopped.");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Timer for auto refresh
            this.timer1.Interval = 1000;
            long curretTick = this.GetCurrentTick();
            timePassed = (curretTick - this.startTime) / 1000;
            Globals.runningTime = (curretTick - Globals.startTime) / 1000;
            if (Globals.notificationFrequency > 0 && ((Globals.runningTime / 60) >= Globals.notificationFrequency))
            {
                Globals.notify = true;
                Globals.startTime = curretTick;
            }
            if (Globals.autoRefresh && ((timePassed / 60) >= Globals.refreshRate))
            {
                this.startTime = curretTick;
                if (!Globals.logEssentials)
                    log("Auto refreshing printer stocks.");
                this.doFetch(false);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // Timer for sound notify when window is not focused
            this.timer2.Interval = 3000;
            if (this.IsActive(this.Handle))
                this.timer2.Stop();
            else
                this.Alert();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Form2(this);
            form.Owner = this;
            form.Show();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log("Refreshing printer stocks.");
            this.doFetch(false);
            log("Printer stocks are up to date.");
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            // Timer for paper jam notification
            this.timer3.Interval = 3000;
            this.Alert();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
        }

        public TrayStatus StringToStatus(string status)
        {
            switch (status)
            {
                case "Empty":
                    return TrayStatus.Empty;
                case "Low":
                    return TrayStatus.Low;
                case "OK":
                    return TrayStatus.OK;
                default:
                    return TrayStatus.OK;
            }
        }
    }
}
