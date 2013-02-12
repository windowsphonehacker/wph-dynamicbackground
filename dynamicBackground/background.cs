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
using System.IO;
using System.IO.IsolatedStorage;
using CSharp___DllImport;
namespace dynamicBackground
{
    public class background
    {
        public static void updateBackground(string file)
        {
            //this deletes the file for some reason
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                store.CopyFile(file, file + "2.jpg");
            }
            DllImportCaller.lib.StringStringCall("aygshell", "SetCurrentWallpaper", @"Applications\Data\c5dd31a7-3985-43bf-b00f-2bf46d66f40b\Data\IsolatedStore\" + file + "2.jpg", "0");
             
        }
        public static void render()
        {
            bool slideshow = false;
            System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.TryGetValue<bool>("slideshow", out slideshow);

            if (slideshow)
            {
                int slide = 0;

                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.TryGetValue<int>("slide", out slide);

                slide++;

                using (var store = System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication())
                {
                    var images = store.GetFileNames("slideshow*");
                    if (slide > images.Length - 1)
                        slide = 0;
                    updateBackground(images[slide]);
                }

                try
                {
                    System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Remove("slide");
                }
                catch
                {
                }
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Add("slide", slide);
                System.IO.IsolatedStorage.IsolatedStorageSettings.ApplicationSettings.Save();
            }
            else
            {
                downloadBackground();
            }
        }
        public static void downloadBackground()
        {
            WebClient client = new System.Net.WebClient();
            client.OpenReadCompleted += new System.Net.OpenReadCompletedEventHandler(client_OpenReadCompleted);
            client.OpenReadAsync(new Uri("http://appserver.m.bing.net/BackgroundImageService/TodayImageService.svc/GetTodayImage?dateOffset=-0&urlEncodeHeaders=true&osName=wince&osVersion=7.10&orientation=480x800&mkt=en-US&deviceName=donttreadonme&AppId=homebrew&UserId=rules", UriKind.Absolute));

        }

        static void client_OpenReadCompleted(object sender, System.Net.OpenReadCompletedEventArgs e)
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = new IsolatedStorageFileStream("bing.jpg", FileMode.OpenOrCreate, store))
                {
                    e.Result.CopyTo(stream);
                }
            }
            updateBackground("bing.jpg");
        }
    }
}
