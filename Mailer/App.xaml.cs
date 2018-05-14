using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using Mailer.Controls;
using Mailer.Domain;
using Mailer.Model;
using Mailer.Services;
using Mailer.Services.Mailer.Database;
using Mailer.View.Flyouts;
using Application = System.Windows.Application;

namespace Mailer
{
    /// <summary>
    ///     Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string Root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private NotifyIcon _trayIcon;
        

        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            DispatcherHelper.Initialize();
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(Settings.Instance.Language);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            switch (Settings.Instance.AccentColor)
            {
                case "Grey":
                case "Blue":
                case "Red":
                case "Emerald":
                case "Magenta":
                case "Mango":
                case "Sea":
                case "Sky":
                case "Purple":
                case "Pink":
                case "Green":
                    Resources.MergedDictionaries[0].Source = new Uri(
                        $"/Resources/Themes/Accents/{Settings.Instance.AccentColor}.xaml", UriKind.Relative);
                    break;

                default:
                    Resources.MergedDictionaries[0].Source =
                        new Uri("/Resources/Themes/Accents/Grey.xaml", UriKind.Relative);
                    break;
            }

            switch (Settings.Instance.Theme)
            {
                case "Light":
                case "Dark":
                    Resources.MergedDictionaries[1].Source = new Uri(
                        $"/Resources/Themes/{Settings.Instance.Theme}.xaml", UriKind.Relative);
                    break;

                default:
                    Resources.MergedDictionaries[1].Source = new Uri("/Resources/Themes/Dark.xaml", UriKind.Relative);
                    break;
            }

            if (!Directory.Exists("Cache"))
                Directory.CreateDirectory("Cache");

            var dataBase = new DataBase();
            /*DataBase dataBase = new DataBase();
#pragma warning disable CS4014
            dataBase.AddUser("callofduty926@mail.ru", new Contact("Sanya", "sanya@mail.ru", "Sanya"));
            dataBase.AddUser("callofduty926@mail.ru", new Contact("Vanya", "vanya@mail.ru", "Vanya"));
            dataBase.AddUser("callofduty926@mail.ru", new Contact("Anya", "anya@mail.ru", "Anya"));
            dataBase.AddUser("callofduty927@mail.ru", new Contact("Danya", "danya@mail.ru", "Danya"));

            //var contacts = dataBase.LoadUsers("callofduty926@mail.ru");

#pragma warning restore CS4014*/

        }

        private void App_OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LoggingService.Log(e.Exception);
            Dispatcher.Invoke(async () =>
            {
                e.Handled = true;

                var flyout = new FlyoutControl {FlyoutContent = new CommonErrorView()};
                var restart = (bool) await flyout.ShowAsync();
                if (restart) Process.Start(ResourceAssembly.Location);

                Shutdown();
            });
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            //TODO Tray Icon
        }

        /*public void AddTrayIcon()
        {
            if (_trayIcon != null)
            {
                return;
            }

            _trayIcon = new NotifyIcon
            {
                Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location),
                Text = "Meridian " + Assembly.GetExecutingAssembly().GetName().Version.ToString(2)
            };
            _trayIcon.MouseClick += TrayIconOnMouseClick;
            _trayIcon.Visible = true;

            _trayIcon.ContextMenu = new ContextMenu();
            var closeItem = new System.Windows.Forms.MenuItem();
            closeItem.Text = MainResources.Close;
            closeItem.Click += (s, e) =>
            {
                foreach (Window window in Windows)
                {
                    window.Close();
                }
            };
            _trayIcon.ContextMenu.MenuItems.Add(closeItem);
        }*/

        private void TrayIconOnMouseClick(object sender, MouseEventArgs mouseEventArgs)
        {
            foreach (Window window in Windows)
            {
                if (window.Visibility == Visibility.Collapsed)
                {
                    window.Visibility = Visibility.Visible;
                    window.Show();
                }

                window.Activate();

                if (window.WindowState == WindowState.Minimized)
                    window.WindowState = WindowState.Normal;
            }
        }

        public void RemoveTrayIcon()
        {
            if (_trayIcon != null)
            {
                _trayIcon.MouseClick -= TrayIconOnMouseClick;
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
                _trayIcon = null;
            }
        }
    }
}