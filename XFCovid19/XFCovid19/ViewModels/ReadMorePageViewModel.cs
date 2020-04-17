using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using XFCovid19.ViewModel;
using XFCovid19.Views;

namespace XFCovid19.ViewModels
{
    public class ReadMorePageViewModel : BaseViewModel
    {
        public ReadMorePageViewModel()
        {
            ClosePopUpCommand = new Command(async () => await ExecuteClosePopUpCommand());
            OpenUrlCommand = new Command<string>(async (args) => await ExecuteOpenUrlCommand(args));
            OpenUrlGitCommand = new Command<string>(async (args) => await ExecuteOpenUrlGitCommand(args));
        }

        public Command OpenUrlCommand { get; }
        public Command OpenUrlGitCommand { get; }
        public Command ClosePopUpCommand { get; }

        private async Task ExecuteClosePopUpCommand()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }

        private async Task ExecuteOpenUrlCommand(string args)
        {
            if (!InternetConnectivity())
            {
                await Navigation.PushPopupAsync(new InternetWarningPage());
                return;
            }

            await Browser.OpenAsync(args, BrowserLaunchMode.SystemPreferred);
        }

        private async Task ExecuteOpenUrlGitCommand(string args)
        {
            if (!InternetConnectivity())
            {
                await Navigation.PushPopupAsync(new InternetWarningPage());
                return;
            }

            await Browser.OpenAsync(args, BrowserLaunchMode.SystemPreferred);
        }
    }
}
