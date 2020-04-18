using Plugin.Multilingual;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFCovid19.Resources;

namespace XFCovid19
{
    public partial class App : Application
    {
        public static string AppCultureInfo;

        public App()
        {
            InitializeComponent();

            AppCultureInfo = Preferences.Get("appLanguage", "pt");
            AppResources.Culture = new CultureInfo(AppCultureInfo);
            CrossMultilingual.Current.CurrentCultureInfo = new CultureInfo(AppCultureInfo);

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
