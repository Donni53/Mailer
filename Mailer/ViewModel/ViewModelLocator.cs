using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Mailer.ViewModel.Settings;

namespace Mailer.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            
        }

        public static SettingsViewModel SettingsViewModel { get; } = new SettingsViewModel();
        public static MainViewModel MainViewModel { get; } = new MainViewModel();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}