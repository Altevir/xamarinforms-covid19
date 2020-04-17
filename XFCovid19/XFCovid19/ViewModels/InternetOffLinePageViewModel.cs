using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFCovid19.Helpers;
using XFCovid19.ViewModel;

namespace XFCovid19.ViewModels
{
    public class InternetOffLinePageViewModel : BaseViewModel
    {
        public InternetOffLinePageViewModel()
        {
            ClosePopUpCommand = new Command(async () => await ExecuteClosePopUpCommand());
            InternetMessageOffLine = TranslateExtension.TranslateText("InternetMessageOffLine");
        }

        public Command ClosePopUpCommand { get; }

        private string _internetMessageOffLine;

        public string InternetMessageOffLine
        {
            get { return _internetMessageOffLine; }
            set { SetProperty(ref _internetMessageOffLine, value); }
        }

        private async Task ExecuteClosePopUpCommand()
        {
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}
