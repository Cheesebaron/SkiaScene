using System;
using SkiaSharp;

namespace SkiaScene.TouchManipulation
{
    public class TapEventArgs : EventArgs
    {
        public SKPoint ViewPoint { get; }

        public TapEventArgs(SKPoint viewPoint)
        {
            ViewPoint = viewPoint;
        }
    }
}
