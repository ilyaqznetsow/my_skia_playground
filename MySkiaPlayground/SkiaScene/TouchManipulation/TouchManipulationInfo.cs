﻿using SkiaSharp;

namespace SkiaScene.TouchManipulation
{
    public class TouchManipulationInfo
    {
        public SKPoint PreviousPoint { set; get; }

        public SKPoint NewPoint { set; get; }

        public int MoveCounter { get; set; }
    }
}
