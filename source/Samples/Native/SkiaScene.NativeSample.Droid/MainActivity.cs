using SkiaSharp.Views.Android;
using SkiaSharp;
using SkiaScene.TouchManipulation;
using TouchTracking.Platforms.Android;

namespace SkiaScene.NativeSample.Droid;

[Activity(Label = "SkiaScene", MainLauncher = true, Icon = "@mipmap/appicon")]
public class MainActivity : Activity
{
    private ISKScene? _scene;
    private ITouchGestureRecognizer? _touchGestureRecognizer;
    private ISceneGestureResponder? _sceneGestureResponder;
    private SKCanvasView? _canvasView;
    private TouchHandler? _touchHandler;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_main);

        // Get our button from the layout resource,
        // and attach an event to it
        _canvasView = FindViewById<SKCanvasView>(Resource.Id.canvasView);
        if (_canvasView == null)
            return;

        _canvasView.PaintSurface += OnPaint;
        _touchHandler = new TouchHandler { UseTouchSlop = true };
        _touchHandler.RegisterEvents(_canvasView);
        _touchHandler.TouchAction += OnTouch;
    }

    private void OnTouch(object? sender, TouchTracking.TouchActionEventArgs args)
    {
        if (_canvasView == null)
            return;

        var viewPoint = args.Location;
        var pixelPoint = new SKPoint(this.ToPixels(viewPoint.X), this.ToPixels(viewPoint.Y));
        var point =
            new SKPoint(
                _canvasView.CanvasSize.Width * pixelPoint.X / _canvasView.Width,
                _canvasView.CanvasSize.Height * pixelPoint.Y / _canvasView.Height);

        var actionType = args.Type;
        _touchGestureRecognizer?.ProcessTouchEvent(args.Id, actionType, point);
    }

    private void SetSceneCenter()
    {
        if (_scene == null || _canvasView == null)
            return;

        var centerPoint = new SKPoint(
            _canvasView.CanvasSize.Width / 2,
            _canvasView.CanvasSize.Height / 2);
        _scene.ScreenCenter = centerPoint;
    }

    private void InitSceneObjects()
    {
        _scene = new SKScene(new SvgSceneRenderer())
        {
            MaxScale = 1000,
            MinScale = 0.001f,
        };
        SetSceneCenter();
        _touchGestureRecognizer = new TouchGestureRecognizer();
        _sceneGestureResponder =
            new SceneGestureRenderingResponder(() => _canvasView?.Invalidate(), _scene, _touchGestureRecognizer)
            {
                TouchManipulationMode = TouchManipulationMode.IsotropicScale,
                EnableTwoFingersPanInIsotropicScaleMode = true,
                MaxFramesPerSecond = 100,
            };
        _sceneGestureResponder.StartResponding();
    }

    private void OnPaint(object? sender, SKPaintSurfaceEventArgs args)
    {
        if (_scene == null)
        {
            InitSceneObjects();

        }
        SKImageInfo info = args.Info;
        SKSurface surface = args.Surface;
        SKCanvas canvas = surface.Canvas;
        _scene?.Render(canvas);
    }
}
