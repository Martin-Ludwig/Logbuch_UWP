using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Logbuch.Classes;
using System.Collections.ObjectModel;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace Logbuch
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        ObservableCollection<Log> obsLogs = new ObservableCollection<Log>();

        public MainPage()
        {
            this.InitializeComponent();

            LogbuchDataAccess.InitializeDatabase();
            List<Log> Logs = LogbuchDataAccess.GetLogs();

            foreach (Log log in Logs)
            {
                obsLogs.Add(log);
            }

            ListViewLogs.ItemsSource = obsLogs;
            

        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            this.InitializeComponent();

            Log newLog = new Log
            {
                Title = TextNewLogTitle.Text,
                Content = TextNewLogContent.Text,
                DateTime = DateTime.Now.ToString("%Y-%m-%d %H:%M:%S")
            };

            Console.WriteLine($"Datetime: {DateTime.Now.ToString("%Y-%m-%d %H:%M:%S")}");

            LogbuchDataAccess.AddLog(newLog);

            //obsLogs.Add(newLog);
            obsLogs.Insert(0, newLog);

        }

        private void Button_Click_ShowGridNewLog(object sender, RoutedEventArgs e)
        {
            GridShowLog.Visibility = Visibility.Collapsed;
            GridNewLog.Visibility = Visibility.Visible;
        }

        private void ListViewLogs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridNewLog.Visibility = Visibility.Collapsed;
            GridShowLog.Visibility = Visibility.Visible;

            var elements = e.AddedItems;
            Log clickedElement = (Log)elements[0];
                
            TextShowLogTitle.Text = $"{clickedElement.Title}";
            TextShowLogDateTime.Text = $"{clickedElement.DateTime}";
            TextShowLogContent.Text = $"{clickedElement.Content}";
        }

    }
}
