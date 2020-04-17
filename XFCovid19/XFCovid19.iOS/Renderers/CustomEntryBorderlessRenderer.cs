using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFCovid19.iOS.Renderers;
using XFCovid19.Renderers;

[assembly: ExportRenderer(typeof(CustomEntryBorderless), typeof(CustomEntryBorderlessRenderer))]
namespace XFCovid19.iOS.Renderers
{
    public class CustomEntryBorderlessRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.BorderStyle = UIKit.UITextBorderStyle.None;
            }
        }
    }
}