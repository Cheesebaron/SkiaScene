using SkiaSharp;
using System.Collections.Generic;

namespace SkiaScene.NativeSample
{
    public class TestSceneRenderer : ISKSceneRenderer
    {
        private const int Columns = 5;
        private const int Rows = 10;
        private CachedScene _cachedScene;
                
        public void Render(SKCanvas canvas, float angleInRadians, SKPoint center, float scale)
        {
            if (_cachedScene == null)
            {
                _cachedScene = new CachedScene();
                var paint = new SKPaint();
                paint.IsAntialias = true;
                paint.Color = new SKColor(0x2c, 0x3e, 0x50);
                paint.StrokeCap = SKStrokeCap.Round;
                _cachedScene.LogoPaint = paint;
                _cachedScene.BackgroundRect = new SKRect(0, 0, 1080, 1800);
                _cachedScene.BackgroundPaint = new SKPaint { Color = new SKColor(255, 10, 10) };
                _cachedScene.LogoPaths = new List<SKPath>();
                for (int i = 1; i <= Columns; i++)
                {
                    for (int y = 1; y <= Rows; y++)
                    {
                        var path = new SKPath();
                        path.MoveTo(71.4311121f, 56f);
                        path.CubicTo(68.6763107f, 56.0058575f, 65.9796704f, 57.5737917f, 64.5928855f, 59.965729f);
                        path.LineTo(43.0238921f, 97.5342563f);
                        path.CubicTo(41.6587026f, 99.9325978f, 41.6587026f, 103.067402f, 43.0238921f, 105.465744f);
                        path.LineTo(64.5928855f, 143.034271f);
                        path.CubicTo(65.9798162f, 145.426228f, 68.6763107f, 146.994582f, 71.4311121f, 147f);
                        path.LineTo(114.568946f, 147f);
                        path.CubicTo(117.323748f, 146.994143f, 120.020241f, 145.426228f, 121.407172f, 143.034271f);
                        path.LineTo(142.976161f, 105.465744f);
                        path.CubicTo(144.34135f, 103.067402f, 144.341209f, 99.9325978f, 142.976161f, 97.5342563f);
                        path.LineTo(121.407172f, 59.965729f);
                        path.CubicTo(120.020241f, 57.5737917f, 117.323748f, 56.0054182f, 114.568946f, 56f);
                        path.LineTo(71.4311121f, 56f);
                        path.Close();
                        path.Offset(160 * i, 160 * y);
                        _cachedScene.LogoPaths.Add(path);
                    }
                }
            }

            canvas.Clear(SKColors.White);
            canvas.DrawRect(_cachedScene.BackgroundRect, _cachedScene.BackgroundPaint);
            foreach (var path in _cachedScene.LogoPaths)
            {
                canvas.DrawPath(path, _cachedScene.LogoPaint);
            }
        }

        private class CachedScene
        {
            public SKPaint LogoPaint { get; set; }
            public IList<SKPath> LogoPaths { get; set; }
            public SKRect BackgroundRect { get; set; }
            public SKPaint BackgroundPaint { get; set; }
        }
    }
}
