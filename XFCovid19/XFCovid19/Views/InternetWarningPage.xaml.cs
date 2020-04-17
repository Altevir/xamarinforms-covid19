using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using XFCovid19.ViewModels;

namespace XFCovid19.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InternetWarningPage : PopupPage
    {
        public InternetWarningPage()
        {
            InitializeComponent();
            BindingContext = new InternetWarningPageViewModel();
        }
    }
}