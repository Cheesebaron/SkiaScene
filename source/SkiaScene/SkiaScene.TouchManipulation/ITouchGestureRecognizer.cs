using SkiaSharp;
using TouchTracking;

namespace SkiaScene.TouchManipulation
{
    public interface ITouchGestureRecognizer
    {
        void ProcessTouchEvent(long id, TouchActionType type, SKPoint location);
        event TapEventHandler OnTap;
        event TapEventHandler OnDoubleTap;
        event TapEventHandler OnSingleTap;
        event PinchEventHandler OnPinch;
        event PanEventHandler OnPan;
    }
}
