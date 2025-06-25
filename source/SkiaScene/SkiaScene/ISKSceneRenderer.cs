using SkiaSharp;

namespace SkiaScene
{
    public interface ISKSceneRenderer
    {
        void Render(SKCanvas canvas, float angleInRadians, SKPoint center, float scale);
    }
}
