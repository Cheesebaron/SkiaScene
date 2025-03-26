# SkiaScene
Collection of lightweight libraries which can be used to simplify manipulation of SkiaSharp graphic objects. 

Supported platforms are Android, iOS and MAUI android and iOS

## Libraries

### SkiaScene
```
Install-Package SkiaScene
```

Basic class which allows controlling `SKCanvasView` transformations without the need to directly manipulate underlying Matrix.
First you need to implement `Render` method of `ISKSceneRenderer` interface, where you define all your drawing logic on `SKCanvasView`.
Then create the `SKScene` instance:

```csharp
//Create scene
ISKSceneRenderer myRenderer = new MyRenderer(); //user defined instance 
scene = new SKScene(myRenderer);

//In your paint method
scene.Render(canvas); //Pass canvas from SKPaintSurfaceEventArgs

//Scene manipulation
scene.MoveByVector(new SKPoint(10, 0)); //Move by 10 units to the right independently from current rotation and zoom
scene.ZoomByScaleFactor(scene.GetCenter(), 1.2f); //Zoom to the center
scene.RotateByRadiansDelta(scene.GetCenter(), 0.1f); //Rotate around center
canvasView.InvalidateSurface(); //Force to repaint
```

### TouchTracking
```
Install-Package TouchTracking
```

TouchTracking provides unified API for multi-touch gestures on Android, iOS. It can be used without MAUI UI framework too. 
Basic principles are described in MAUI Documentation https://learn.microsoft.com/en-us/dotnet/maui/migration/effects?view=net-maui-9.0

Usage is similar on each platform. 

Android example:

```csharp
//Initialization
canvasView = FindViewById<SKCanvasView>(Resource.Id.canvasView); //Get SKCanvasView
touchHandler = new TouchHandler() { UseTouchSlop = true };
touchHandler.RegisterEvents(canvasView); //Pass View to the touch handler
touchHandler.TouchAction += OnTouch; //Listen to the touch gestures

void OnTouch(object sender, TouchActionEventArgs args) {
    var point = args.Location; //Point location
    var type = args.Type; //Entered, Pressed, Moved ... etc.
    //... do something
}
```

### TouchTracking.MAUI
```
Install-Package TouchTracking.MAUI
```

Same functionality as TouchTracking library but can be consumed in MAUI as an Effect called TouchEffect.

```xml
xmlns:maui="clr-namespace:TouchTracking.MAUI;assembly=TouchTracking.MAUI"

<Grid
    BackgroundColor="White">
    <controls:SKCanvasView
        x:Name="CanvasView"
        PaintSurface="OnPaint"
        EnableTouchEvents="true">
    </controls:SKCanvasView>
    <Grid.Effects>
        <maui:TouchEffect
            Capture="True"
            TouchAction="OnTouchEffectAction" />
    </Grid.Effects>
</Grid>
```

Make sure to register the `Effect` in your `MauiProgram.cs` file adding:

```csharp
.ConfigureEffects(effects =>
{
    effects.InitTouchTrackingEffects();
})
```

### SkiaScene.TouchManipulations
```
Install-Package SkiaScene.TouchManipulations
```

Combines SkiaScene and TouchTracking libraries to detect and respond to the tap, pinch and pan gestures. Most of the functionality is described in Xamarin Documentation https://developer.xamarin.com/guides/xamarin-forms/advanced/skiasharp/transforms/touch/

`TouchGestureRecognizer` recieves touch event info in 'ProcessTouchEvent' method and fires concrete gesture event.

`SceneGestureResponder` subscribes to the events of `TouchGestureRecognizer` and executes correct actions in underlying `SKScene`.

`SceneGestureRenderingResponder` inherits from `SceneGestureResponder` and additionally controls frequency of calling `InvalidateSurface` method through `MaxFramesPerSecond` property.
