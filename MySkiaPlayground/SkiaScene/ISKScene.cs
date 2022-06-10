using System;
using System.Collections.Generic;
using MySkiaPlayground.SkiaScene.SkiaObjects;
using SkiaSharp;

namespace MySkiaPlayground.SkiaScene
{
    public interface ISKScene
    {
        void Render(SKSceneRendererModel sKSceneRendererModel);
        void Zoom(SKPoint point, float scale);
        void ZoomByScaleFactor(SKPoint point, float scaleFactor);
        void MoveToPoint(SKPoint point);
        void MoveByVector(SKPoint vector);
        void Rotate(SKPoint point, float radians);
        void RotateByRadiansDelta(SKPoint point, float radiansDelta);
        SKPoint GetCanvasPointFromViewPoint(SKPoint viewPoint);
        SKPoint GetCenter();
        float GetAngleInRadians();
        float GetScale();
        float MaxScale { get; set; }
        float MinScale { get; set; }
        SKPoint ScreenCenter { get; set; }
        SKRect CenterBoundary { get; set; }
        IList<IFigure> Figures { get; set; }
    }
}
