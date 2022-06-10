using System.Collections.Generic;
using MySkiaPlayground.SkiaScene.SkiaObjects;
using SkiaSharp;

namespace MySkiaPlayground.SkiaScene
{
    public class SKSceneRendererModel
    {
        public SKCanvas Canvas { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float AngleInRadnians { get; set; }
        public SKPoint Center { get; set; }
        public float Scale { get; set; }
        public float GridSize { get; set; }
        public IList<IFigure> Figures { get; set; }
    }
}
