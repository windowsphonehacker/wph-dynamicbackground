﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
namespace WindowsPhoneHacker
{
    public class wph
    {

        public static Boolean czech()
        {
            bool u = false;
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.TryGetValue<bool>("u", out u);
            if (u)
                return true;



            System.Windows.Resources.StreamResourceInfo stream = Application.GetResourceStream(new Uri("Tile.png", UriKind.Relative));
            
            StreamReader reader = new StreamReader(stream.Stream);
            string line = reader.ReadLine();
            DateTime t1 = DateTime.Parse(line);
            double d = DateTime.Now.Subtract(t1).TotalHours;

            if (d > 25)
                return false;

            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Add("u", true);
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Save();

            return true;
        }

        public static Boolean updateAvailable = false;

        public static void bacon(String id)
        {
            try
            {
                System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(doit));
                th.Start(id);
            }
            catch
            {
            }
        }
        private static void doit(object id)
        {
            try
            {
                WebClient cl = new WebClient();
                cl.DownloadStringAsync(new Uri(String.Format("http://windowsphonehacker.com/omgbacon/bacon.php?id={0}&device={1}", (String)id, getDevInfo())));
                cl.DownloadStringCompleted += new DownloadStringCompletedEventHandler(cl_DownloadStringCompleted);
            }
            catch
            {
            }
        }

        static void cl_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //Check how often we nag
            if (e.Result == "UPDATE")
            {
                int nagcount = 0;
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.TryGetValue<int>("nags", out nagcount);
                if (nagcount == 0 || nagcount == 5)
                {

                    updateAvailableEvent(sender, new EventArgs());
                }
                nagcount++;
                try
                {
                    System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Remove("nags");
                }
                catch
                {
                }
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Add("nags", nagcount);
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Save();
            }


        }
        public static event EventHandler updateAvailableEvent;

        private static String getDevInfo()
        {
            String info = Microsoft.Phone.Info.DeviceStatus.DeviceName + "," + Microsoft.Phone.Info.DeviceStatus.DeviceManufacturer + "," + System.Environment.OSVersion;
            return info;
        }
    }
}
