using SkiaSharp;

namespace MySkiaPlayground.SkiaScene.SkiaObjects
{
    public interface IFigure
    {
        SKColor Color { get; set; }
        float X { get; set; }
        float Y { get; set; }
        bool IsMoving { get; set; }
        bool IsPointOverlap(SKPoint point);
        void Draw(SKCanvas canvas);
    }
}
