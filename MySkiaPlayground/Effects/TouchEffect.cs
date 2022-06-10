using System;
using Xamarin.Forms;

namespace MySkiaPlayground.Effects
{
    public class TouchEffect : RoutingEffect
    {
        public event TouchActionEventHandler TouchAction;

        public TouchEffect() : base($"MySkiaPlayground.{nameof(TouchEffect)}")
        {
        }

        public bool Capture { set; get; }

        public void OnTouchAction(object element, TouchActionEventArgs args)
        {
            TouchAction?.Invoke(element, args);
        }
    }
}
