namespace TouchTracking.MAUI;

public class TouchEffect : RoutingEffect
{
    public event TouchActionEventHandler? TouchAction;

    public TouchEffect() : base("TouchTracking.TouchEffect")
    {
    }

    public bool Capture { set; get; }

    public void OnTouchAction(object element, TouchActionEventArgs args)
    {
        TouchAction?.Invoke(element, args);
    }
}
