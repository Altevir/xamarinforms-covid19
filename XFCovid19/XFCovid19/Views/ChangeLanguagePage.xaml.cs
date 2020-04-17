using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using XFCovid19.ViewModels;

namespace XFCovid19.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeLanguagePage : PopupPage
    {
        ChangeLanguagePageViewModel viewModel;

        public ChangeLanguagePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ChangeLanguagePageViewModel();
            switchChangeTheme.Toggled += (s, e) =>
            {
                viewModel.ChangeAppThemeCommand.Execute(null);
            };
        }
    }
}