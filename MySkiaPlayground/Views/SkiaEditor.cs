using System.Collections.Generic;
using MySkiaPlayground.SkiaScene.SkiaObjects;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace MySkiaPlayground.Views
{
    public class SkiaEditor : SKCanvasView
    {
        public SkiaEditor()
        {
        }

        public float SceneHeight
        {
            get => (float)GetValue(SceneHeightProperty);
            set => SetValue(SceneHeightProperty, value);
        }

        public static readonly BindableProperty SceneHeightProperty =
            BindableProperty.Create(nameof(SceneHeight), typeof(float),
                typeof(SkiaEditor), 1000f);

        public float SceneWidth
        {
            get => (float)GetValue(SceneWidthProperty);
            set => SetValue(SceneWidthProperty, value);
        }

        public static readonly BindableProperty SceneWidthProperty =
            BindableProperty.Create(nameof(SceneWidth), typeof(float),
                typeof(SkiaEditor), 1000f);

        public float GridSize
        {
            get => (float)GetValue(GridSizeProperty);
            set => SetValue(GridSizeProperty, value);
        }

        public static readonly BindableProperty GridSizeProperty =
            BindableProperty.Create(nameof(GridSize), typeof(float),
                typeof(SkiaEditor), 50f);

        public IList<IFigure> SceneObjects
        {
            get => (IList<IFigure>)GetValue(SceneObjectsProperty);
            set => SetValue(SceneObjectsProperty, value);
        }

        public static readonly BindableProperty SceneObjectsProperty =
            BindableProperty.Create(nameof(SceneObjects), typeof(IList<IFigure>),
                typeof(SkiaEditor));
    }
}
