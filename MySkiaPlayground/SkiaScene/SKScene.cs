using System;
using System.Collections.Generic;
using MySkiaPlayground.SkiaScene.SkiaObjects;
using SkiaSharp;

namespace MySkiaPlayground.SkiaScene
{
    public class SKScene : ISKScene
    {
        protected readonly ISKSceneRenderer _sceneRenderer;
        protected SKMatrix Matrix = SKMatrix.CreateIdentity();

        protected const float DefaultMaxScale = 8;
        protected const float DefaultMinScale = 1f / 8;

        public SKScene(ISKSceneRenderer sceneRenderer)
        {
            _sceneRenderer = sceneRenderer;
        }

        public void Render(SKSceneRendererModel sKSceneRendererModel)
        {
            if (sKSceneRendererModel == null)
                throw new Exception("renderer model must not be null or empty");

            sKSceneRendererModel.Canvas.Concat(ref Matrix);
            sKSceneRendererModel.Center = GetCenter();
            sKSceneRendererModel.AngleInRadnians = GetAngleInRadians();
            sKSceneRendererModel.Scale = GetScale();
            Figures = sKSceneRendererModel.Figures;
            _sceneRenderer.Render(sKSceneRendererModel);
        }

        public float MaxScale { get; set; } = DefaultMaxScale;
        public float MinScale { get; set; } = DefaultMinScale;
        public SKRect CenterBoundary { get; set; }
        public SKPoint ScreenCenter { get; set; }
        public IList<IFigure> Figures { get; set; }

        public void MoveByVector(SKPoint vector)
        {
            SKMatrix invertedMatrix;
            if (!Matrix.TryInvert(out invertedMatrix))
            {
                return;
            }
            var resultPoint = invertedMatrix.MapVector(vector.X, vector.Y);
            SKMatrix.PreConcat(ref Matrix, SKMatrix.CreateTranslation(resultPoint.X, resultPoint.Y));
            if (CenterBoundary.IsEmpty)
            {
                return;
            }
            var center = GetCenter();
            if (!CenterBoundary.Contains(center))
            {
                //rollback
                SKMatrix.PreConcat(ref Matrix, SKMatrix.CreateTranslation(-resultPoint.X, -resultPoint.Y));
            }
        }

        public void MoveToPoint(SKPoint point)
        {
            var center = GetCenter();
            SKPoint diff = center - point;
            SKMatrix.PreConcat(ref Matrix, SKMatrix.CreateTranslation(diff.X, diff.Y));
        }

        public void Rotate(SKPoint point, float radians)
        {
            var currentAngle = GetAngleInRadians();
            var angleDiff = radians - currentAngle;
            SKMatrix.PreConcat(ref Matrix, SKMatrix.CreateRotation(angleDiff, point.X, point.Y));
        }

        public void RotateByRadiansDelta(SKPoint point, float radiansDelta)
        {
            SKMatrix.PreConcat(ref Matrix, SKMatrix.CreateRotation(radiansDelta, point.X, point.Y));
        }

        public void Zoom(SKPoint point, float scale)
        {
            var currentScale = GetScale();
            var scaleFactor = scale / currentScale;
            SKMatrix.PreConcat(ref Matrix, SKMatrix.CreateScale(scaleFactor, scaleFactor, point.X, point.Y));
        }

        public void ZoomByScaleFactor(SKPoint point, float scaleFactor)
        {
            var currentScale = GetScale();
            currentScale *= scaleFactor;
            if (currentScale < MinScale || currentScale > MaxScale)
            {
                return;
            }
            if (!CenterBoundary.IsEmpty)
            {
                var center = GetCenter();
                if (!CenterBoundary.Contains(center))
                {
                    return;
                }
            }
            SKMatrix.PreConcat(ref Matrix, SKMatrix.CreateScale(scaleFactor, scaleFactor, point.X, point.Y));
        }


        public SKPoint GetCanvasPointFromViewPoint(SKPoint viewPoint)
        {

            SKMatrix invertedMatrix;
            if (!Matrix.TryInvert(out invertedMatrix))
            {
                return SKPoint.Empty;
            }
            return invertedMatrix.MapPoint(viewPoint);
        }

        public SKPoint GetCenter()
        {
            return GetCanvasPointFromViewPoint(ScreenCenter);
        }

        /// <summary>
        /// Returns number from interval [-pi to pi]
        /// </summary>
        /// <returns></returns>
        public float GetAngleInRadians()
        {
            //https://stackoverflow.com/questions/4361242/extract-rotation-scale-values-from-2d-transformation-matrix
            var skewY = Matrix.SkewY;
            var scaleY = Matrix.ScaleY;
            var result = Math.Atan2(skewY, scaleY);
            return (float)result;
        }

        public float GetScale()
        {
            //https://stackoverflow.com/questions/4361242/extract-rotation-scale-values-from-2d-transformation-matrix
            var scaleX = Matrix.ScaleX;
            var skewY = Matrix.SkewY;
            var result = Math.Sqrt(scaleX * scaleX + skewY * skewY);
            return (float)result;
        }
    }
}
