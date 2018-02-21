using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Mailer.Model;
using Mailer.Resources.Localization;

namespace Mailer.ViewModel.Settings
{
    public class SettingsViewModel : ViewModelBase
    {

        private bool _canSave;
        private bool _restartRequired;
        private ColorScheme _selectedColorScheme;
        private string _selectedTheme;

        public SettingsViewModel()
        {

        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SaveRestartCommand { get; private set; }

        public Dictionary<string, string> MenuItems { get; } = new Dictionary<string, string>
        {
            {MainResources.SettingsAccounts, "/View/Settings/AccountsView.xaml"},
            {MainResources.SettingsPersonalization, "/View/Settings/PersonalizationView.xaml"},
            {MainResources.SettingsSecurity, "/View/Settings/SecurityView.xaml"},
            {MainResources.SettingsAutoResponses, "/View/Settings/AutoResponsesView.xaml"},
            {MainResources.SettingsNotifications, "/View/Settings/NotificationsView.xaml"},
            {MainResources.SettingsAbout, "/View/Settings/AboutView.xaml"}
        };

        public List<string> Themes { get; } = new List<string>
        {
            "Dark",
            "Light",
            "Graphite",
            "Accent"
        };



        public string SelectedTheme
        {
            get => _selectedTheme;
            set
            {
                if (Set(ref _selectedTheme, value))
                {
                    CanSave = true;
                }
            }
        }

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

        public ColorScheme SelectedColorScheme
        {
            get => _selectedColorScheme;
            set
            {
                if (Set(ref _selectedColorScheme, value))
                {
                    CanSave = true;
                }
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

        private void SaveSettings()
        {
            switch (SelectedTheme)
            {
                case "Dark":
                case "Light":
                case "Graphite":
                case "Accent":
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

            Domain.Settings.Instance.Save();

        }
    }
}
