using System.Linq;
using MySkiaPlayground.Effects;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("MySkiaPlayground")]
[assembly: ExportEffect(typeof(MySkiaPlayground.Droid.TouchEffect), nameof(MySkiaPlayground.Droid.TouchEffect))]
namespace MySkiaPlayground.Droid
{
    public class TouchEffect : PlatformEffect
    {
        private TouchHandler _touchHandler;
        private Android.Views.View _view;
        private Effects.TouchEffect _touchEffect;

        protected override void OnAttached()
        {
            _view = Control == null ? Container : Control;

            // Get access to the TouchEffect class in the PCL
            _touchEffect =
                (Effects.TouchEffect)Element.Effects.FirstOrDefault(e => e is Effects.TouchEffect);

            if (_touchEffect == null)
            {
                return;
            }

            _touchHandler = new TouchHandler();
            _touchHandler.TouchAction += TouchHandlerOnTouch;
            _touchHandler.Capture = _touchEffect.Capture;
            _touchHandler.RegisterEvents(_view);

        }

        private void TouchHandlerOnTouch(object sender, TouchActionEventArgs args)
        {
            _touchEffect.OnTouchAction(sender, args);
        }

        protected override void OnDetached()
        {
            if (_touchHandler == null)
            {
                return;
            }
            _touchHandler.TouchAction -= TouchHandlerOnTouch;
            _touchHandler.UnregisterEvents(_view);
        }
    }
}