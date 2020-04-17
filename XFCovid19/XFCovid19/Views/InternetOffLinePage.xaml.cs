using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using XFCovid19.ViewModels;

namespace XFCovid19.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InternetOffLinePage : PopupPage
    {
        public InternetOffLinePage()
        {
            InitializeComponent();
            BindingContext = new InternetOffLinePageViewModel();
        }
    }
}