using Plugin.Multilingual;
using Rg.Plugins.Popup.Services;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFCovid19.Helpers;
using XFCovid19.Resources;
using XFCovid19.ViewModel;

namespace XFCovid19.ViewModels
{
    public class ChangeLanguagePageViewModel : BaseViewModel
    {
        public ChangeLanguagePageViewModel()
        {
            LanguageSelection = TranslateExtension.TranslateText("SelectionLanguage");
            ThemeSelection = TranslateExtension.TranslateText("ThemeSelection");
            ChangeLanguageCommand = new Command<string>(async (args) => await ExecuteChangeLanguageCommand(args));
            ChangeAppThemeCommand = new Command(async () => await ExecuteChangeAppThemeCommand());
            ClosePopUpCommand = new Command(async () => await ExecuteClosePopUpCommand());
            SetCheckFlag();
            CheckAppTheme();
        }

        public Command ChangeLanguageCommand { get; }
        public Command ClosePopUpCommand { get; }
        public Command ChangeAppThemeCommand { get; }

        private bool _isVisibleCheckFlagBR;
        public bool IsVisibleCheckFlagBR
        {
            get { return _isVisibleCheckFlagBR; }
            set { SetProperty(ref _isVisibleCheckFlagBR, value); }
        }

        private bool _isVisibleCheckFlagUSA;
        public bool IsVisibleCheckFlagUSA
        {
            get { return _isVisibleCheckFlagUSA; }
            set { SetProperty(ref _isVisibleCheckFlagUSA, value); }
        }

        private async Task ExecuteClosePopUpCommand()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private string _languageSelection;
        public string LanguageSelection
        {
            get { return _languageSelection; }
            set { SetProperty(ref _languageSelection, value); }
        }

        private bool _appDarkTheme;
        public bool AppDarkTheme
        {
            get { return _appDarkTheme; }
            set
            {
                SetProperty(ref _appDarkTheme, value);
            }
        }

        private string _themeSelection;
        public string ThemeSelection
        {
            get { return _themeSelection; }
            set { SetProperty(ref _themeSelection, value); }
        }

        void SetCheckFlag()
        {
            IsVisibleCheckFlagBR = App.AppCultureInfo.Equals("pt");
            IsVisibleCheckFlagUSA = !IsVisibleCheckFlagBR;
        }

        void CheckAppTheme()
        {
            AppDarkTheme = Preferences.Get("appDarkTheme", false);
        }

        private async Task ExecuteChangeLanguageCommand(string language)
        {
            App.AppCultureInfo = language;
            Preferences.Set("appLanguage", language);
            CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo(language);
            AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
            MessagingCenter.Send(this, "changeLanguage");
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async Task ExecuteChangeAppThemeCommand()
        {
            Preferences.Set("appDarkTheme", AppDarkTheme);
            MessagingCenter.Send(this, "changeAppTheme", AppDarkTheme);
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}
