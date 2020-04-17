using System.ComponentModel;
using Xamarin.Forms;
using XFCovid19.Services;
using XFCovid19.ViewModels;

namespace XFCovid19
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        MainPageViewModel viewModel;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MainPageViewModel(new RestService());
        }

        protected override async void OnAppearing()
        {
            viewModel.SubscribeChangeLanguage();
            await viewModel.GetAll();
        }

        protected override void OnDisappearing()
        {
            viewModel.OnDisappearing();
        }
    }
}
