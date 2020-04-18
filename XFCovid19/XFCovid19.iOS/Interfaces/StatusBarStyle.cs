using UIKit;
using Xamarin.Forms;
using XFCovid19.Interfaces;
using XFCovid19.iOS.Interfaces;

[assembly: Dependency(typeof(StatusBarStyle))]
namespace XFCovid19.iOS.Interfaces
{
    public class StatusBarStyle : IStatusBarStyle
    {
        public void ChangeTextColor(bool darkTheme)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var currentUIViewController = GetCurrentViewController();
                var userInterfaceStyle = currentUIViewController.TraitCollection.UserInterfaceStyle;

                if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
                {
                    UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
                }
                else
                {
                    if (userInterfaceStyle == UIUserInterfaceStyle.Light && !darkTheme)
                    {
                        UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.Default, false);
                    }
                    else
                    {
                        UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false);
                    }
                }

                GetCurrentViewController().SetNeedsStatusBarAppearanceUpdate();
            });
        }

        UIViewController GetCurrentViewController()
        {
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
                vc = vc.PresentedViewController;
            return vc;
        }
    }
}