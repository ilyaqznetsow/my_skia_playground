using System;
using System.Diagnostics;
using SkiaSharp;

namespace MySkiaPlayground.SkiaScene.SkiaObjects
{
    public class Figure : IFigure
    {
        public float X { get; set; }
        public float Y { get; set; }
        public SKColor Color { get; set; }

        public Figure(float x, float y, SKColor color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }

    public class Line : Figure
    {
        public SKPoint EndPoint { get; set; }

        public Line(float x, float y, SKPoint endPoint, SKColor color) : base(x, y, color)
        {
            EndPoint = endPoint;
        }
    }

    public class Rect : Figure
    {
        public float Width { get; set; }
        public float Height { get; set; }

        public Rect(float x, float y, float width, float height, SKColor color) : base(x, y, color)
        {
            Width = width;
            Height = height;
        }

        //Center x = x + 1 / 2 of width
        //Center y = y + 1 / 2 of height
        public SKPoint GetCenter() => new SKPoint((float)(X + (Width * 0.5)), (float)(Y + (Height * 0.5)));

        public bool IsPointInRect(SKPoint point)
        {
            var isInHorizontal = point.X > X && point.X < Width + X;
            var isInVertical = point.Y > Y && point.Y < Height + Y;
            Debug.WriteLine($"h: {isInHorizontal} v:{isInVertical}");
            return isInHorizontal && isInVertical;
        }
    }

    public class Circle : Figure
    {
        public float Radius { get; set; }
        public Circle(float x, float y, float radius, SKColor color) : base(x, y, color)
        {
            Radius = radius;
        }
    }
}
