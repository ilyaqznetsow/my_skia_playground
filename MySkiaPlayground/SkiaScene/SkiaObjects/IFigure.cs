﻿using SkiaSharp;

namespace MySkiaPlayground.SkiaScene.SkiaObjects
{
    public interface IFigure
    {
        SKColor Color { get; set; }
        float X { get; set; }
        float Y { get; set; }
    }
}
