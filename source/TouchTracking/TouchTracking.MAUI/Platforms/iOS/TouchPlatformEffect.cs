using TouchTracking.Platforms.iOS;
using UIKit;

namespace TouchTracking.MAUI.Platforms.iOS;

public class TouchPlatformEffect : Microsoft.Maui.Controls.Platform.PlatformEffect
{
    private TouchHandler? _touchHandler;
    private UIView? _view;
    private TouchTracking.MAUI.TouchEffect? _touchEffect;

    protected override void OnAttached()
    {
        _view = Control ?? Container;

        _touchEffect =
            (TouchTracking.MAUI.TouchEffect?)Element.Effects.FirstOrDefault(e => e is TouchTracking.MAUI.TouchEffect);

        if (_touchEffect == null)
        {
            return;
        }

        _touchHandler = new TouchHandler();
        _touchHandler.TouchAction += TouchHandlerOnTouch;
        _touchHandler.Capture = _touchEffect.Capture;
        _touchHandler.RegisterEvents(_view);
    }

    private void TouchHandlerOnTouch(object sender, TouchActionEventArgs args)
    {
        _touchEffect?.OnTouchAction(sender, args);
    }

    protected override void OnDetached()
    {
        if (_touchHandler == null)
        {
            return;
        }
        _touchHandler.TouchAction -= TouchHandlerOnTouch;
        _touchHandler.UnregisterEvents(_view);
    }
}
