using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFCovid19.Helpers;
using XFCovid19.ViewModel;

namespace XFCovid19.ViewModels
{
    public class InternetWarningPageViewModel : BaseViewModel
    {
        public InternetWarningPageViewModel()
        {
            ClosePopUpCommand = new Command(async () => await ExecuteClosePopUpCommand());
            InternetNotConnected = TranslateExtension.TranslateText("InternetNotConnected");
        }

        public Command ClosePopUpCommand { get; }

        private string _internetNotConnected;
        public string InternetNotConnected
        {
            get { return _internetNotConnected; }
            set { SetProperty(ref _internetNotConnected, value); }
        }

        private async Task ExecuteClosePopUpCommand()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}
