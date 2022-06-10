using System;
using System.Collections.Generic;
using MySkiaPlayground.Effects;
using MySkiaPlayground.SkiaScene;
using MySkiaPlayground.SkiaScene.SkiaObjects;
using MySkiaPlayground.Views;
using SkiaScene.TouchManipulation;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace MySkiaPlayground
{
    public class MainPage : ContentPage
    {
        SkiaEditor canvasView;
        public MainPage()
        {
            Title = "playground";
            this.SizeChanged += OnSizeChanged;

            canvasView = new SkiaEditor()
            {
                EnableTouchEvents = true,
                SceneHeight = 1000,
                SceneWidth = 1000,
                SceneObjects = new List<IFigure>
                {
                    new Circle(1000, 1000, 200, Color.Red.ToSKColor()),
                    new SkiaScene.SkiaObjects.Rect(500, 200, 100,100, Color.Blue.ToSKColor()),
                }
            };
            canvasView.PaintSurface += OnPaint;
            var effect = new TouchEffect() { Capture = true };

            effect.TouchAction += OnTouchEffectAction;
            Content = new Grid
            {
                Effects = { effect },
                Children ={
                    canvasView
                }
            };
        }

        private ISKScene _scene;
        private ITouchGestureRecognizer _touchGestureRecognizer;
        private ISceneGestureResponder _sceneGestureResponder;


        private void OnSizeChanged(object sender, EventArgs eventArgs)
        {
            SetSceneCenter();
        }


        private void SetSceneCenter()
        {
            if (_scene == null)
            {
                return;
            }
            var centerPoint = new SKPoint(canvasView.CanvasSize.Width / 2, canvasView.CanvasSize.Height / 2);
            _scene.ScreenCenter = centerPoint;
        }

        private void InitScene()
        {
            _scene = new SKScene(new TestSceneRenderer())
            {
                MaxScale = 10,
                MinScale = 0.1f,
            };
            SetSceneCenter();
            _touchGestureRecognizer = new TouchGestureRecognizer();
            _sceneGestureResponder = new SceneGestureRenderingResponder(() => canvasView.InvalidateSurface(), _scene, _touchGestureRecognizer)
            {
                TouchManipulationMode = TouchManipulationMode.IsotropicScale,
                MaxFramesPerSecond = 100,
            };
            _sceneGestureResponder.StartResponding();
        }

        private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            var viewPoint = args.Location;
            SKPoint point =
                new SKPoint((float)(canvasView.CanvasSize.Width * viewPoint.X / canvasView.Width),
                            (float)(canvasView.CanvasSize.Height * viewPoint.Y / canvasView.Height));

            var actionType = args.Type;
            _touchGestureRecognizer.ProcessTouchEvent(args.Id, actionType, point);
            //Debug.WriteLine($"id {args.Id}, type {actionType} point {point}");
        }

        private void OnPaint(object sender, SKPaintSurfaceEventArgs args)
        {
            if (_scene == null)
            {
                InitScene();
            }
            var info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            var rendererModel = new SKSceneRendererModel
            {
                Canvas = canvas,
                Width = canvasView.SceneWidth,
                Height = canvasView.SceneHeight,
                GridSize = canvasView.GridSize,
                Figures = canvasView.SceneObjects
            };
            _scene.Render(rendererModel);
        }
    }
}