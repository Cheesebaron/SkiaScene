using System;
using SkiaSharp;
using TouchTracking;

namespace SkiaScene.TouchManipulation
{
    public class PinchEventArgs : EventArgs
    {
        public SKPoint PreviousPoint { get; }

        public SKPoint NewPoint { get; }

        public SKPoint PivotPoint { get; }

        public TouchActionType TouchActionType { get; }

        public PinchEventArgs(SKPoint previousPoint, SKPoint newPoint, SKPoint pivotPoint, TouchActionType touchActionType)
        {
            PreviousPoint = previousPoint;
            NewPoint = newPoint;
            PivotPoint = pivotPoint;
            TouchActionType = touchActionType;
        }
    }
}
