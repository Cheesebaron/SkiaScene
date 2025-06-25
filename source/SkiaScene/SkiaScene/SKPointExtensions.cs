using System;
using SkiaSharp;

namespace SkiaScene
{
    public static class SKPointExtensions
    {
        public static float GetMagnitude(this SKPoint point)
        {
            return MathF.Sqrt(MathF.Pow(point.X, 2f) + MathF.Pow(point.Y, 2f));
        }
    }
}
