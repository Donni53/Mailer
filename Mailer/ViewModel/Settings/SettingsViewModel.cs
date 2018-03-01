using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Mailer.Controls;
using Mailer.Helpers;
using Mailer.Messages;
using Mailer.Model;
using Mailer.Resources.Localization;
using Mailer.Services;
using Mailer.View.Accounts;
using Mailer.View.Flyouts;

namespace Mailer.ViewModel.Settings
{
    public class SettingsViewModel : ViewModelBase
    {

        public Dictionary<string, string> MenuItems { get; } = new Dictionary<string, string>
        {
            {MainResources.SettingsAccounts, "/View/Settings/AccountsView.xaml"},
            {MainResources.SettingsPersonalization, "/View/Settings/PersonalizationView.xaml"},
            //{MainResources.SettingsSecurity, "/View/Settings/SecurityView.xaml"},
            {MainResources.SettingsNotifications, "/View/Settings/NotificationsView.xaml"},
            {MainResources.SettingsAbout, "/View/Settings/AboutView.xaml"}
        };

        public List<string> Themes { get; } = new List<string>
        {
            "Dark",
            "Light"
        };

        public List<ColorScheme> AccentColors { get; } = new List<ColorScheme>
        {
            new ColorScheme("Grey", "#363b3e"),
            new ColorScheme("Blue", "#006ac1"),
            new ColorScheme("Red", "#e51400"),
            new ColorScheme("Sky", "#1ba1e2"),
            new ColorScheme("Emerald", "#059f01"),
            new ColorScheme("Mango", "#fe6f11"),
            new ColorScheme("Magenta", "#d80073"),
            new ColorScheme("Sea", "#009f9f"),
            new ColorScheme("Purple", "#6800d3"),
            new ColorScheme("Pink", "#e671b8"),
            new ColorScheme("Green", "#00896d")
        };

        private readonly List<SettingsLanguage> _languages = new List<SettingsLanguage>()
        {
            new SettingsLanguage() {LanguageCode = "en", Title = CultureInfo.GetCultureInfo("en").NativeName},
            new SettingsLanguage() {LanguageCode = "ru", Title = CultureInfo.GetCultureInfo("ru").NativeName}
        };

        private bool _canSave;
        private bool _restartRequired;
        private ColorScheme _selectedColorScheme;
        private string _selectedTheme;
        private bool _checkForUpdates;
        private bool _enableNotifications;
        private bool _enableTrayIcon;
        private bool _showBackgroundArt;
        private bool _blurBackground;
        private string _cacheSize;
        private SettingsLanguage _selectedLanguage;
        private int _selectedAccount;

        public RelayCommand CloseSettingsCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SaveRestartCommand { get; private set; }
        public RelayCommand CheckUpdatesCommand { get; private set; }
        public RelayCommand ClearCacheCommand { get; private set; }
        public RelayCommand EditAccountCommand { get; private set; }
        public RelayCommand DeleteAccountCommand { get; private set; }
        public RelayCommand AddAccountCommand { get; private set; }
        public RelayCommand OmsSetupCommand { get; private set; }

        public SettingsViewModel()
        {
            Domain.Settings.Load();
            InitializeCommands();
            _selectedTheme = Domain.Settings.Instance.Theme;
            _selectedColorScheme = AccentColors.FirstOrDefault(c => c.Name == Domain.Settings.Instance.AccentColor);
            _checkForUpdates = Domain.Settings.Instance.CheckForUpdates;
            _enableTrayIcon = Domain.Settings.Instance.EnableTrayIcon;
            _showBackgroundArt = Domain.Settings.Instance.ShowBackgroundArt;
            _blurBackground = Domain.Settings.Instance.BlurBackground;
            _selectedAccount = Domain.Settings.Instance.SelectedAccount;

            var lang = _languages.FirstOrDefault(l => l.LanguageCode == Domain.Settings.Instance.Language);
            if (lang != null)
                _selectedLanguage = lang;
            else
                _selectedLanguage = _languages.First();
        }

        public List<Model.Account> Accounts => Domain.Settings.Instance.Accounts;

        public string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (Set(ref _selectedTheme, value))
                    CanSave = true;
            }
        }

        public ColorScheme SelectedColorScheme
        {
            get => _selectedColorScheme;
            set
            {
                if (Set(ref _selectedColorScheme, value))
                    CanSave = true;
            }
        }

        public bool RestartRequired
        {
            get => _restartRequired;
            set => Set(ref _restartRequired, value);
        }

        public bool CanSave
        {
            get => _canSave;
            set => Set(ref _canSave, value);
        }

        public bool CheckForUpdates
        {
            get => _checkForUpdates;
            set
            {
                if (Set(ref _checkForUpdates, value))
                    CanSave = true;
            }
        }

        public bool InstallDevUpdates
        {
            get => Domain.Settings.Instance.InstallDevUpdates;
            set => Domain.Settings.Instance.InstallDevUpdates = value;
        }

        public bool EnableNotifications
        {
            get => _enableNotifications;
            set
            {
                if (Set(ref _enableNotifications, value))
                    CanSave = true;
            }
        }

        public bool EnableTrayIcon
        {
            get => _enableTrayIcon;
            set
            {
                if (Set(ref _enableTrayIcon, value))
                {
                    CanSave = true;

                    if (value != Domain.Settings.Instance.EnableTrayIcon)
                        RestartRequired = true;
                }
            }
        }

        public bool ShowBackgroundArt
        {
            get => _showBackgroundArt;
            set
            {
                if (Set(ref _showBackgroundArt, value))
                    CanSave = true;
            }
        }


        public bool BlurBackground
        {
            get => _blurBackground;
            set
            {
                if (Set(ref _blurBackground, value))
                    CanSave = true;
            }
        }

        public string CacheSize
        {
            get => _cacheSize;
            set => Set(ref _cacheSize, value);
        }

        public int SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                if (Set(ref _selectedAccount, value))
                    CanSave = true;
                RaisePropertyChanged("AutoReplie");
                RaisePropertyChanged("AutoReplieText");
            }
        }

        public List<SettingsLanguage> Languages => _languages;

        public SettingsLanguage SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (Set(ref _selectedLanguage, value))
                {
                    CanSave = true;

                    if (value.LanguageCode != Domain.Settings.Instance.Language)
                        RestartRequired = true;
                }
            }
        }

        public bool AutoReplie
        {
            get => Domain.Settings.Instance.Accounts[SelectedAccount].AutoReplie;
            set
            {
                Domain.Settings.Instance.Accounts[SelectedAccount].AutoReplie = value;
                CanSave = true;
                RaisePropertyChanged("AutoReplie");
            }
        }

        public string AutoReplieText
        {
            get => Domain.Settings.Instance.Accounts[SelectedAccount].AutoReplieText;
            set
            {
                Domain.Settings.Instance.Accounts[SelectedAccount].AutoReplieText = value;
                CanSave = true;
                RaisePropertyChanged("AutoReplieText");
            }
        }

        public new async void Activate()
        {
            //check cache
            if (Directory.Exists("Cache"))
            {
                var cacheSize = await CalculateFolderSizeAsync("Cache");
                CacheSize = StringHelper.FormatSize(Math.Round(cacheSize, 1));
            }
        }

        private void InitializeCommands()
        {
            EditAccountCommand = new RelayCommand(EditAccount);
            DeleteAccountCommand = new RelayCommand(DeleteAccount);
            AddAccountCommand = new RelayCommand(AddAccount);
            OmsSetupCommand = new RelayCommand(OmsSetup);
            CloseSettingsCommand = new RelayCommand(() =>
            {
                ViewModelLocator.MainViewModel.GoBackCommand.Execute(null);
            });

            SaveCommand = new RelayCommand(SaveSettings);

            SaveRestartCommand = new RelayCommand(() =>
            {
                SaveSettings();

                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            });

            //TODO CheckUpdatesCommand = new RelayCommand(() => ViewModelLocator.UpdateService.CheckUpdates());

            ClearCacheCommand = new RelayCommand(async () =>
            {
                if (!Directory.Exists("Cache"))
                    return;

                foreach (var file in Directory.EnumerateFiles("Cache"))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }

                foreach (var dir in Directory.EnumerateDirectories("Cache"))
                {
                    try
                    {
                        Directory.Delete(dir, true);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                }

                var cacheSize = await CalculateFolderSizeAsync("Cache");
                CacheSize = StringHelper.FormatSize(Math.Round(cacheSize, 1));
            });
        }

        private void AddAccount()
        {
            ViewModelLocator.AccountSetupViewModel.UserName = "";
            ViewModelLocator.AccountSetupViewModel.Login = "";
            ViewModelLocator.AccountSetupViewModel.Password = "";
            ViewModelLocator.AccountSetupViewModel.ImapServer = "";
            ViewModelLocator.AccountSetupViewModel.ImapSsl = false;
            ViewModelLocator.AccountSetupViewModel.NewAccount = true;
            ViewModelLocator.AccountSetupViewModel.Id = -1;
            Messenger.Default.Send(new NavigateToPageMessage()
            {
                Page = "/Accounts.AccountSetupView"
            });
        }

        private void EditAccount()
        {
            //sorry for that ((
            //TODO Fix it
            ViewModelLocator.AccountSetupViewModel.UserName =
                Domain.Settings.Instance.Accounts[SelectedAccount].UserName;
            ViewModelLocator.AccountSetupViewModel.Login =
                Domain.Settings.Instance.Accounts[SelectedAccount].ImapData.Login;
            ViewModelLocator.AccountSetupViewModel.Password =
                Domain.Settings.Instance.Accounts[SelectedAccount].ImapData.Password;
            ViewModelLocator.AccountSetupViewModel.ImapServer =
                Domain.Settings.Instance.Accounts[SelectedAccount].ImapData.Address;
            ViewModelLocator.AccountSetupViewModel.ImapSsl =
                Domain.Settings.Instance.Accounts[SelectedAccount].ImapData.UseSsl;
            ViewModelLocator.AccountSetupViewModel.NewAccount = false;
            ViewModelLocator.AccountSetupViewModel.Id = SelectedAccount;
            Messenger.Default.Send(new NavigateToPageMessage()
            {
                Page = "/Accounts.AccountSetupView"
            });
        }

        private void DeleteAccount()
        {
            AccountManager.ImapLogout(SelectedAccount);
            //TODO Delete confirmation
        }

        private void OmsSetup()
        {
            var flyout = new FlyoutControl {FlyoutContent = new SmtpSetupView(null, 0)};
            flyout.Show();
        }

        private void SaveSettings()
        {
            switch (SelectedTheme)
            {
                case "Dark":
                case "Light":
                    Application.Current.Resources.MergedDictionaries[1].Source =
                        new Uri($"/Resources/Themes/{SelectedTheme}.xaml", UriKind.Relative);
                    break;

                default:
                    Application.Current.Resources.MergedDictionaries[1].Source = new Uri("/Resources/Themes/Dark.xaml",
                        UriKind.Relative);
                    break;
            }

            switch (SelectedColorScheme.Name)
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
                    Application.Current.Resources.MergedDictionaries[0].Source =
                        new Uri($"/Resources/Themes/Accents/{SelectedColorScheme.Name}.xaml",
                            UriKind.Relative);
                    break;

                default:
                    Application.Current.Resources.MergedDictionaries[0].Source =
                        new Uri("/Resources/Themes/Accents/Grey.xaml", UriKind.Relative);
                    break;
            }

            Domain.Settings.Instance.AccentColor = SelectedColorScheme.Name;
            Domain.Settings.Instance.Theme = SelectedTheme;
            Domain.Settings.Instance.ShowBackgroundArt = ShowBackgroundArt;
            Domain.Settings.Instance.BlurBackground = BlurBackground;
            Domain.Settings.Instance.CheckForUpdates = CheckForUpdates;
            Domain.Settings.Instance.EnableTrayIcon = EnableTrayIcon;
            Domain.Settings.Instance.InstallDevUpdates = InstallDevUpdates;
            Domain.Settings.Instance.Language = SelectedLanguage.LanguageCode;
            Domain.Settings.Instance.SelectedAccount = SelectedAccount;

            Domain.Settings.Instance.Save();

        }

        private static Task<float> CalculateFolderSizeAsync(string folder)
        {
            return Task.Run(() => CalculateFolderSize(folder));
        }

        private static float CalculateFolderSize(string folder)
        {
            float folderSize = 0.0f;
            try
            {
                if (!Directory.Exists(folder))
                    return folderSize;
                try
                {
                    folderSize = (from file in Directory.EnumerateFiles(folder) where File.Exists(file) select new FileInfo(file)).Aggregate(folderSize, (current, finfo) => current + finfo.Length);

                    folderSize += Directory.GetDirectories(folder).Sum(dir => CalculateFolderSize(dir));
                }
                catch (NotSupportedException ex)
                {
                    LoggingService.Log($"Unable to calculate folder size: {ex.Message}");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                LoggingService.Log($"Unable to calculate folder size: {ex.Message}");
            }
            return folderSize;
        }
    }
}
