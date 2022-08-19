using System;

namespace PrinterStocks
{
    public struct email_s
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public static class Globals
    {
        // Class to easily find and manage global variables
        // Global constants
        public const int MAX_WEBSITES = 15;

        // Global static variables
        public static Form1 form;
        public static bool isReady = false;
        public static bool notify = true;
        public static bool isRunning = false;
        public static long startTime, runningTime;

        // Default values for config file
        public static bool emailNotify = false, soundNotify = true, autoRefresh = true, logEssentials = true;
        public static string ccEmails, recipientEmails = "test@utep.edu";
        public static string websitesString = "http://clcseciprn.utep.edu,http://clcseciiprn.utep.edu,http://clcseciiiprn.utep.edu,http://clcsecivprn.utep.edu,http://clcseciv2prn.utep.edu";
        public static string[] websites = websitesString.Split(',');
        public static TrayStatus paperTrayCondition = (TrayStatus)0x1;
        public static int blackCartridgeCondition = 0, refreshRate = 1;
        public static int notificationFrequency = 45;

        // Information to outlook email account
        public static email_s email = new email_s {
            username = "utep.library.clc@outlook.com",
            password = "password12345*"
        };
    }

    public enum TrayStatus
    {
        Empty = 0x0,
        Low = 0x1,
        OK = 0x2,
    };

    public class Section
    {
        // Class to contain each section of the websites provided. 
        // Used to manage section properties and compose alert messages based on a given condition.
        public int cartridge { get; set; }
        public TrayStatus topTray { get; set; }
        public TrayStatus bottomTray { get; set; }
        public TrayStatus multiPurpose { get; set; }
        public int maintenanceKit { get; set; }
        public int rollerKit { get; set; }
        public int imagingUnit { get; set; }
        public bool paperJam { get; set; }
        private string[] messages;

        public Section() {
            this.messages = new string[8];
        }

        public string[] getMessages(TrayStatus trayCondition, int cartridgeCondition, int kitCondition=2, int imageCondition=0)
        {
            // Returns a string array of notifications that will be sent within an email
            // Checks if previous check and current check are equal, and if so, an email will not be sent
            bool isEqual = true;
            string[] messages = new string[8];
            for (int idx = 0; idx < messages.Length; idx++)
            {
                switch (idx)
                {
                    case 0:
                        messages[idx] = this.cartridge <= cartridgeCondition ? "Black Cartidge is at " + this.cartridge.ToString() + "%" : null;
                        break;
                    case 1:
                        messages[idx] = this.topTray <= trayCondition ? "Top Tray is " + this.topTray : null;
                        break;
                    case 2:
                        messages[idx] = this.bottomTray <= trayCondition ? "Bottom Tray is " + this.bottomTray : null;
                        break;
                    case 3:
                        messages[idx] = this.multiPurpose <= trayCondition ? "Multi-Purpose Feeder is " + this.multiPurpose : null;
                        break;
                    case 4:
                        messages[idx] = this.maintenanceKit <= kitCondition ? "Maintenance Kit Life Remaining is " + this.maintenanceKit + "%" : null;
                        break;
                    case 5:
                        messages[idx] = this.rollerKit <= kitCondition ? "Roller Kit Life Remaining is " + this.rollerKit + "%" : null;
                        break;
                    case 6:
                        messages[idx] = this.imagingUnit <= imageCondition ? "Imaging Unit Life Remaining is " + this.imagingUnit + "%" : null;
                        break;
                    case 7:
                        messages[idx] = this.paperJam ? "Paper Jam Detected!" : null;
                        break;
                }
                if (messages[idx] == null)
                    continue;
                if (!Globals.notify && !messages[idx].Contains("Paper Jam") && (this.messages[idx] != null && this.isEqual(this.messages[idx], messages[idx])))
                    messages[idx] = null;
                else
                {
                    isEqual = false;
                    this.messages[idx] = messages[idx];
                }
            }
            
            return isEqual ? messages : this.messages;
        }

        private bool isEqual<T>(T target1, T target2)
        {
            return target1.Equals(target2);
        }
    }
}
