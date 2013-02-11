using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Scheduler;
namespace dynamicBackground
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            try
            {
                background.downloadBackground();
            }
            catch
            {
            }

            updateView();
            updateMyBg();

            double timeout = 60 * 15;
            
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.TryGetValue<double>("timespan_raw", out timeout);
            if (timeout == 0)
                timeout = 60 * 2;

            slider1.Value = timeout;
        }
        void updateView()
        {
            bool slideshow = false;
            IsolatedStorageSettings.ApplicationSettings.TryGetValue<bool>("slideshow", out slideshow);

            if (slideshow)
            {
                radioButton2.IsChecked = true;
                slideshowPanel.Visibility = System.Windows.Visibility.Visible;
                updateList();
            }
            else
            {
                slideshowPanel.Visibility = System.Windows.Visibility.Collapsed;
                radioButton1.IsChecked = true;
            }
        }

        
        void updateList()
        {
            slideshowImages.Children.Clear();

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                int x = 0;
                StackPanel panel = new StackPanel();
                panel.Orientation = System.Windows.Controls.Orientation.Horizontal;
                slideshowImages.Children.Add(panel);

                foreach (string file in store.GetFileNames("slideshow*"))
                {
                    Image img = new Image();
                    img.Name = file.Replace(".", "");
                    img.Stretch = Stretch.Uniform;
                    img.Width = 100;
                    img.Height = 180;
                    img.Margin = new Thickness(2, 2, 2, 0);
                    img.Tap += new EventHandler<GestureEventArgs>(img_Tap);
                    BitmapImage bmp = new BitmapImage();
                    using (var stream = store.OpenFile(file, FileMode.Open))
                    {
                        bmp.SetSource(stream);
                    }
                    img.Source = bmp;

                    panel.Children.Add(img);
                    System.Diagnostics.Debug.WriteLine(file);
                    x++;
                    if (x > 3)
                    {
                        x = 0;

                        panel = new StackPanel();
                        panel.Orientation = System.Windows.Controls.Orientation.Horizontal;
                        slideshowImages.Children.Add(panel);
                    }

                    bmp = null;
                }
            }
        }

        void img_Tap(object sender, GestureEventArgs e)
        {
            if (MessageBox.Show("Remove?", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    store.DeleteFile(((Image)sender).Name.Replace("jpg", ".jpg"));
                }
                updateList();
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            var oldTask = ScheduledActionService.Find("task") as PeriodicTask;
            if (oldTask != null)
            {
                ScheduledActionService.Remove("task");
            }

            Microsoft.Phone.Scheduler.PeriodicTask task = new Microsoft.Phone.Scheduler.PeriodicTask("task");
            task.Description = "Changes the lock screen background";

            ScheduledActionService.Add(task);
            
        }

        void updateMyBg()
        {
            try
            {
                using (var store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var str = store.OpenFile("bing.jpg", FileMode.Open))
                    {
                        System.Windows.Media.Imaging.WriteableBitmap wb = new System.Windows.Media.Imaging.WriteableBitmap(480, 800);
                        BitmapImage bimg = new BitmapImage();
                        bimg.SetSource(str);
                        image1.Source = bimg;
                    }
                }
            }
            catch
            {
                System.Windows.Threading.DispatcherTimer tim = new System.Windows.Threading.DispatcherTimer();
                tim.Interval = TimeSpan.FromSeconds(2);
                tim.Tick += new EventHandler((o, e) =>
                {
                    updateMyBg();
                    tim.Stop();
                });
                tim.Start();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Phone.Tasks.PhotoChooserTask t = new Microsoft.Phone.Tasks.PhotoChooserTask();
            t.Show();
            t.Completed += new EventHandler<Microsoft.Phone.Tasks.PhotoResult>(t_Completed);
        }

        void t_Completed(object sender, Microsoft.Phone.Tasks.PhotoResult e)
        {
            if (e.TaskResult == Microsoft.Phone.Tasks.TaskResult.OK)
            {
                System.Windows.Media.Imaging.WriteableBitmap wb = new System.Windows.Media.Imaging.WriteableBitmap(480, 800);
                BitmapImage bimg = new BitmapImage();
                bimg.SetSource(e.ChosenPhoto);
                Image img = new Image();
                img.Width = 480;
                img.Height = 800;
                img.Source = bimg;
                img.Stretch = Stretch.UniformToFill;

                wb.Render(img, null);

                wb.Invalidate();
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var st = new IsolatedStorageFileStream("slideshow_" + (DateTime.Now.Day.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + DateTime.Now.Hour.ToString()) + ".jpg", FileMode.Create, FileAccess.Write, store))
                    {
                        System.Windows.Media.Imaging.Extensions.SaveJpeg(wb, st, 480, 800, 0, 100);
                    }
                }
                updateList();
            }
            else
            {
                MessageBox.Show("It looks like your phone is syncing; couldn't open the chooser.");
            }
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            setSlideshow(false);
            updateView();
        }
        private void setSlideshow(bool val)
        {
            try
            {
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Remove("slideshow");
            }
            catch
            {
            }
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Add("slideshow", val);
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Save();
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            setSlideshow(true);
            updateView();
        }
        
        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                
                int val = Convert.ToInt32(Math.Round(slider1.Value));

                if (val < 400)
                    val = (int)((60) * ((double)val / 400));
                else
                    val -= 340;

                string str = "";
                if (val < 60)
                {
                    str = val + " minutes";
                }
                else
                {
                    str = (val / 60) + " hours";
                }
                txtMinutes.Text = str;
                
            }
            catch
            {
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            int val = Convert.ToInt32(Math.Round(slider1.Value));

            if (val < 400)
                val = (int)((60) * ((double)val / 400));
            else
                val -= 340;

            try
            {
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Remove("timespan");
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Remove("timespan_raw");
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Remove("lastrun");
            }
            catch
            {
            }
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Add("timespan", val);
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Add("timespan_raw", slider1.Value);
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Save();


            ScheduledActionService.LaunchForTest("task", TimeSpan.FromMilliseconds(120));
        }

        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("===========\n" +
               "dynamic background v2\n" +
               "===========\n" +
               "by windowsphonehacker\n\n" +
               "developed by jaxbot\n\n" +
               "thanks to fiinix for DllImport\n\nDedicated to Ivey");
        }

        private void ApplicationBarMenuItem_Click_1(object sender, EventArgs e)
        {
            Microsoft.Phone.Tasks.WebBrowserTask t = new Microsoft.Phone.Tasks.WebBrowserTask();
            t.Uri = new Uri("http://forum.xda-developers.com/donatetome.php?u=1639678");
            t.Show();
        }
    }
}