using System.Diagnostics.CodeAnalysis;
using SkiaScene.Sample;
using SkiaScene.TouchManipulation;
using SkiaSharp;
using SkiaSharp.Views.iOS;
using TouchTracking.Platforms.iOS;

namespace SkiaScene.NativeSample.iOS;

public sealed class ViewController : UIViewController
{
    private SKScene? _scene;
    private TouchGestureRecognizer? _touchGestureRecognizer;
    private SceneGestureResponder? _sceneGestureResponder;
    private SKCanvasView? _canvasView;
    private TouchHandler? _touchHandler;

    public override void LoadView()
    {
        base.LoadView();

        _canvasView = new SKCanvasView();
        _canvasView.TranslatesAutoresizingMaskIntoConstraints = false;
        Add(_canvasView);

        _canvasView.TopAnchor.ConstraintEqualTo(View!.TopAnchor).Active = true;
        _canvasView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
        _canvasView.LeadingAnchor.ConstraintEqualTo(View.LeadingAnchor).Active = true;
        _canvasView.TrailingAnchor.ConstraintEqualTo(View.TrailingAnchor).Active = true;
    }

    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        if (_canvasView == null)
        {
            return;
        }

        _canvasView.PaintSurface += OnPaint;
        _touchHandler = new TouchHandler();
        _touchHandler.RegisterEvents(_canvasView);
        _touchHandler.TouchAction += OnTouch;

        NSNotificationCenter.DefaultCenter.AddObserver(
            new NSString("UIDeviceOrientationDidChangeNotification"), OnDeviceRotation);
    }

    private void OnDeviceRotation(NSNotification notification)
    {
        SetSceneCenter();
    }

    private void OnTouch(object? sender, TouchTracking.TouchActionEventArgs args)
    {
        if (_canvasView == null)
        {
            return;
        }

        var viewPoint = args.Location;
        var point =
            new SKPoint(
                (float)(_canvasView.CanvasSize.Width * viewPoint.X / _canvasView.Frame.Width),
                (float)(_canvasView.CanvasSize.Height * viewPoint.Y / _canvasView.Frame.Height));

        var actionType = args.Type;
        _touchGestureRecognizer?.ProcessTouchEvent(args.Id, actionType, point);
    }

    private void SetSceneCenter()
    {
        if (_scene == null || _canvasView == null)
        {
            return;
        }

        var centerPoint = new SKPoint(_canvasView.CanvasSize.Width / 2, _canvasView.CanvasSize.Height / 2);
        _scene.ScreenCenter = centerPoint;
    }

    [MemberNotNull(nameof(_scene), nameof(_touchGestureRecognizer), nameof(_sceneGestureResponder))]
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
            new SceneGestureRenderingResponder(() => _canvasView?.SetNeedsDisplay(), _scene, _touchGestureRecognizer)
            {
                TouchManipulationMode = TouchManipulationMode.ScaleRotate,
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

        SKSurface surface = args.Surface;
        SKCanvas canvas = surface.Canvas;
        _scene.Render(canvas);
    }
}
