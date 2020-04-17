using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;
using XFCovid19.Services;
using XFCovid19.ViewModels;

namespace XFCovid19.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchCountryPage : PopupPage
    {
        SearchCountryViewModel viewModel;

        public SearchCountryPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new SearchCountryViewModel(new RestService());
        }

        protected override async void OnAppearing()
        {
            await viewModel.InitData();
        }
    }
}