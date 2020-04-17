using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFCovid19.Droid.Renderers;
using XFCovid19.Renderers;

[assembly: ExportRenderer(typeof(CustomEntryBorderless), typeof(CustomEntryBorderlessRenderer))]
namespace XFCovid19.Droid.Renderers
{
    public class CustomEntryBorderlessRenderer : EntryRenderer
    {
        public CustomEntryBorderlessRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.SetBackground(null);
            }
        }
    }
}