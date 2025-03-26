using System.Diagnostics.CodeAnalysis;
using TouchTracking;
using SkiaScene.Sample;
using SkiaScene.TouchManipulation;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace SkiaScene.MAUISample;

public partial class MainPage : ContentPage
{
    private SKScene? _scene;
    private TouchGestureRecognizer? _touchGestureRecognizer;
    private SceneGestureRenderingResponder? _sceneGestureResponder;

    public MainPage()
    {
        InitializeComponent();
        SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        SetSceneCenter();
    }

    [MemberNotNull(nameof(_scene), nameof(_touchGestureRecognizer), nameof(_sceneGestureResponder))]
    private void InitSceneObjects()
    {
        _scene = new SKScene(new SvgSceneRenderer())
        {
            MaxScale = 10,
            MinScale = 0.3f,
        };
        SetSceneCenter();
        _touchGestureRecognizer = new TouchGestureRecognizer();
        _sceneGestureResponder = new SceneGestureRenderingResponder(() => CanvasView.InvalidateSurface(), _scene, _touchGestureRecognizer)
        {
            TouchManipulationMode = TouchManipulationMode.IsotropicScale,
            MaxFramesPerSecond = 100,
        };
        _sceneGestureResponder.StartResponding();
    }

    private void SetSceneCenter()
    {
        if (_scene == null)
        {
            return;
        }
        var centerPoint = new SKPoint(CanvasView.CanvasSize.Width / 2, CanvasView.CanvasSize.Height / 2);
        _scene.ScreenCenter = centerPoint;
    }

    private void OnTouchEffectAction(object sender, TouchActionEventArgs args)
    {
        var viewPoint = args.Location;
        SKPoint point =
            new SKPoint(
                (float)(CanvasView.CanvasSize.Width * viewPoint.X / CanvasView.Width),
                (float)(CanvasView.CanvasSize.Height * viewPoint.Y / CanvasView.Height));

        var actionType = args.Type;
        _touchGestureRecognizer?.ProcessTouchEvent(args.Id, actionType, point);
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
        _scene.Render(canvas);
    }
}
