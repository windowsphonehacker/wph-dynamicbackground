using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Scheduler;
namespace dynamicBackground
{
    public class task : ScheduledTaskAgent
    {
        protected override void OnInvoke(ScheduledTask task)
        {
            int timeout = 0;

            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.TryGetValue<int>("timespan", out timeout);

            DateTime lastrun;

            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.TryGetValue<DateTime>("lastrun", out lastrun);

            if (DateTime.Now.Subtract(lastrun).TotalMinutes < timeout)
            {
                System.Diagnostics.Debug.WriteLine("Too soon, stopping.");
                NotifyComplete();
                return;
            }


            ScheduledActionService.LaunchForTest("task", TimeSpan.FromMinutes(timeout));

            System.Diagnostics.Debug.WriteLine("here we go...");

            try
            {
                background.render();
                try
                {
                    System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Remove("lastrun");
                }
                catch
                {
                }
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Add("lastrun", DateTime.Now);
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Save();
            }
            catch
            {
            }

            System.Threading.Thread.Sleep(9000);
            NotifyComplete();   
        }
    }
}
