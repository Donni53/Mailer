using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Mailer.Controls;
using Mailer.Model;
using Mailer.Resources.Localization;
using Mailer.Services;
using Mailer.View.Flyouts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Mailer.Domain
{
    public class Settings
    {
        private const string SettingsFile = "Mailer.settings";

        public Settings()
        {
            CheckForUpdates = true;
            AccentColor = "Blue";
            Theme = "Light";
            Language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            SendStats = true;
            BlurBackground = false;
            EnableTrayIcon = false;
            ShowBackgroundArt = false;
            Accounts = new List<Account>();
            SelectedAccount = -1;
            CustomBackgroundPath = "";
            CustomBackground = false;
            SaveMessagesToCache = false;
            LoadFullVersions = false;
        }

        public static Settings Instance { get; private set; } = new Settings();

        public bool CheckForUpdates { get; set; }

        public bool InstallDevUpdates { get; set; }

        public bool NeedClean { get; set; }

        public string AccentColor { get; set; }

        public string Theme { get; set; }

        public string Language { get; set; }

        public bool SendStats { get; set; }

        public bool BlurBackground { get; set; }

        public bool EnableTrayIcon { get; set; }
        public bool ShowBackgroundArt { get; set; }
        public List<Account> Accounts { get; set; }
        public int SelectedAccount { get; set; }
        public bool CustomBackground { get; set; }
        public string CustomBackgroundPath { get; set; }
        public bool AutoReplie { get; set; }
        public string AutoReplieText { get; set; }
        public int SelectedImage { get; set; }
        public bool SaveMessagesToCache { get; set; }
        public bool LoadFullVersions { get; set; }

        public static void Load()
        {
            if (!File.Exists(SettingsFile))
                return;

            try
            {
                var json = File.ReadAllText(SettingsFile);
                if (string.IsNullOrEmpty(json))
                    return;

                var serializer = new JsonSerializer();
                var o = (JObject) JsonConvert.DeserializeObject(json);
                var settings = serializer.Deserialize<Settings>(o.CreateReader());
                Instance = settings;
            }
            catch (Exception ex)
            {
                LoggingService.Log(ex);
            }
        }

        public async void Save()
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                };

                var json = JsonConvert.SerializeObject(this, settings);

                File.WriteAllText(SettingsFile, json);
            }
            catch (UnauthorizedAccessException ex)
            {
                var flyout = new FlyoutControl
                {
                    FlyoutContent = new CommonErrorView(ErrorResources.AccessDeniedErrorTitle,
                        ErrorResources.AccessDeniedErrorDescription)
                };
                await flyout.ShowAsync().ContinueWith(t =>
                {
                    if ((bool) t.Result)
                    {
                        var info = new ProcessStartInfo
                        {
                            UseShellExecute = true,
                            FileName = Application.ResourceAssembly.Location,
                            WorkingDirectory = Environment.CurrentDirectory,
                            Verb = "runas"
                        };

                        Process.Start(info);
                    }

                    Application.Current.Shutdown();

                    LoggingService.Log(ex);
                });
            }
            catch (Exception ex)
            {
                LoggingService.Log(ex);
            }
        }
    }
}