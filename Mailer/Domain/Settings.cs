using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Mailer.Controls;
using Mailer.Resources.Localization;
using Mailer.View.Flyouts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Mailer.Domain
{
    public class Settings
    {
        private const string SettingsFile = "Mailer.settings";

        private static Settings _instance = new Settings();

        public static Settings Instance => _instance;

        public bool CheckForUpdates { get; set; }

        public bool InstallDevUpdates { get; set; }

        public bool NeedClean { get; set; }

        public string AccentColor { get; set; }

        public string Theme { get; set; }

        public string Language { get; set; }

        public bool SendStats { get; set; }

        public bool BlurBackground { get; set; }

        public Settings()
        {
            CheckForUpdates = true;
            AccentColor = "Blue";
            Theme = "Light";
            Language = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            SendStats = true;
            BlurBackground = false;
        }

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
                var o = (JObject)JsonConvert.DeserializeObject(json);
                var settings = serializer.Deserialize<Settings>(o.CreateReader());
                _instance = settings;
            }
            catch (Exception ex)
            {
                //LoggingService.Log(ex);
            }
        }

        public async void Save()
        {
            try
            {
                var settings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                var json = JsonConvert.SerializeObject(this, settings);

                File.WriteAllText(SettingsFile, json);
            }
            catch (UnauthorizedAccessException ex)
            {
                var flyout = new FlyoutControl
                {
                    FlyoutContent = new CommonErrorView(ErrorResources.AccessDeniedErrorTitle, ErrorResources.AccessDeniedErrorDescription)
                };
                await flyout.ShowAsync().ContinueWith(t =>
                {
                    if ((bool)t.Result == true)
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

                    //LoggingService.Log(ex);
                });
            }
            catch (Exception ex)
            {
                //LoggingService.Log(ex);
            }
        }
    }
}
