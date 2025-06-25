using System.Reflection;
using SkiaSharp;
using Svg.Skia;

namespace SkiaScene.Sample
{
    public class SvgSceneRenderer : ISKSceneRenderer
    {
        private SKPicture _svgScene;
        private string _fileName = "test.svg";

        public void Render(SKCanvas canvas, float angleInRadians, SKPoint center, float scale)
        {
            if (_svgScene == null)
            {
                _svgScene = LoadScene();
            }
            canvas.Clear(SKColors.White);
            canvas.DrawPicture(_svgScene);
        }

        private SKPicture LoadScene()
        {
            var svg = new SKSvg();
            var fileName = $"SkiaScene.Sample.{_fileName}";
            var assembly = typeof(SvgSceneRenderer).GetTypeInfo().Assembly;
            var stream = assembly.GetManifestResourceStream(fileName);
            var result = svg.Load(stream!);
            return result;
        }
    }
}
