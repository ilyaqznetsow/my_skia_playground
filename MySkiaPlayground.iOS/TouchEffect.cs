using System.Diagnostics;
using System.Linq;
using MySkiaPlayground.Effects;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("MySkiaPlayground")]
[assembly: ExportEffect(typeof(MySkiaPlayground.iOS.TouchEffect), nameof(MySkiaPlayground.iOS.TouchEffect))]
namespace MySkiaPlayground.iOS
{
    public class TouchEffect : PlatformEffect
    {
        private TouchHandler _touchHandler;
        private UIView _view;
        private Effects.TouchEffect _touchEffect;

        protected override void OnAttached()
        {
            Debug.WriteLine("00000000000000");
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