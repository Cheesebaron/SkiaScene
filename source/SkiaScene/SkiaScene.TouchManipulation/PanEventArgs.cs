using System;
using SkiaSharp;
using TouchTracking;

namespace SkiaScene.TouchManipulation
{
    public class PanEventArgs : EventArgs
    {
        public SKPoint PreviousPoint { get; }

        public SKPoint NewPoint { get; }

        public TouchActionType TouchActionType { get; }

        public PanEventArgs(SKPoint previousPoint, SKPoint newPoint, TouchActionType touchActionType)
        {
            PreviousPoint = previousPoint;
            NewPoint = newPoint;
            TouchActionType = touchActionType;
        }
    }
}
