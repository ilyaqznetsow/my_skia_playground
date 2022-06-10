using System.Collections.Generic;
using MySkiaPlayground.SkiaScene;
using MySkiaPlayground.SkiaScene.SkiaObjects;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace MySkiaPlayground.Views
{
    public class TestSceneRenderer : ISKSceneRenderer
    {
        public void Render(SKSceneRendererModel sKSceneRendererModel)
        {
            var canvas = sKSceneRendererModel.Canvas;
            var width = sKSceneRendererModel.Width;
            var height = sKSceneRendererModel.Height;
            var gridSize = sKSceneRendererModel.GridSize;

            canvas.Clear(SKColors.White);
            // set up drawing tools
            using (var paint = new SKPaint())
            {
                paint.IsAntialias = true;
                paint.Color = Color.Gray.ToSKColor();

                //bounds
                var rect = new SKRect(0, 0, width, height);
                canvas.DrawRect(rect, new SKPaint() { Color = Color.Gray.ToSKColor(), IsStroke = true });

                //horizontal lines
                var horizontalCount = width / gridSize * (height / width);
                // i = 1 - because we already have bounds drawed
                for (var i = 1; i < horizontalCount; i++)
                {
                    var starPoint = new SKPoint(0, i * gridSize);
                    var endPoint = new SKPoint(width, i * gridSize);
                    canvas.DrawLine(starPoint, endPoint, paint);
                }

                //vertical lines
                var verticalCount = height / gridSize * (width / height);
                for (var i = 1; i < verticalCount; i++)
                {
                    var starPoint = new SKPoint(i * gridSize, 0);
                    var endPoint = new SKPoint(i * gridSize, height);
                    canvas.DrawLine(starPoint, endPoint, paint);
                }
            }

            if (sKSceneRendererModel.Figures != null)
                RenderObjectList(canvas, sKSceneRendererModel.Figures);

        }

        void RenderObjectList(SKCanvas canvas, IList<IFigure> figures)
        {
            foreach (var figure in figures)
            {
                var paint = new SKPaint() { Color = figure.Color };
                if (figure is Line line)
                    canvas.DrawLine(new SKPoint(line.X, line.Y), line.EndPoint, paint);
                if (figure is SkiaScene.SkiaObjects.Rect rect)
                    canvas.DrawRect(rect.X, rect.Y, rect.Width, rect.Height, paint);
                if (figure is SkiaScene.SkiaObjects.Circle circle)
                    canvas.DrawCircle(new SKPoint(circle.X, circle.Y), circle.Radius, paint);
            }
        }
    }
}
