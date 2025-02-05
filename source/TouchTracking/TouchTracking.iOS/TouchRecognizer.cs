using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

namespace TouchTracking.iOS
{
    class TouchRecognizer : UIGestureRecognizer
    {
        private readonly UIView _view;            // iOS UIView 
        private readonly TouchHandler _touchHandler;
        bool _capture;

        private static readonly Dictionary<UIView, TouchRecognizer> ViewDictionary = new();

        private static readonly Dictionary<long, TouchRecognizer> IdToTouchDictionary = new();

        public TouchRecognizer(UIView view, TouchHandler touchHandler)
        {
            _view = view;
            this._touchHandler = touchHandler;
            ViewDictionary.Add(view, this);
        }

        public void Detach()
        {
            ViewDictionary.Remove(_view);
        }

        // touches = touches of interest; evt = all touches of type UITouch
        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                nint id = touch.Handle;
                FireEvent(this, id, TouchActionType.Pressed, touch, true);

                if (!IdToTouchDictionary.ContainsKey(id))
                    IdToTouchDictionary.Add(id, this);
            }

            // Save the setting of the Capture property
            _capture = _touchHandler.Capture;
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                nint id = touch.Handle;

                if (_capture)
                {
                    FireEvent(this, id, TouchActionType.Moved, touch, true);
                }
                else
                {
                    CheckForBoundaryHop(touch);

                    if (IdToTouchDictionary[id] != null)
                    {
                        FireEvent(IdToTouchDictionary[id], id, TouchActionType.Moved, touch, true);
                    }
                }
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                nint id = touch.Handle;

                if (_capture)
                {
                    FireEvent(this, id, TouchActionType.Released, touch, false);
                }
                else
                {
                    CheckForBoundaryHop(touch);

                    if (IdToTouchDictionary[id] != null)
                    {
                        FireEvent(IdToTouchDictionary[id], id, TouchActionType.Released, touch, false);
                    }
                }
                IdToTouchDictionary.Remove(id);
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            foreach (UITouch touch in touches.Cast<UITouch>())
            {
                nint id = touch.Handle;

                if (_capture)
                {
                    FireEvent(this, id, TouchActionType.Cancelled, touch, false);
                }
                else if (IdToTouchDictionary[id] != null)
                {
                    FireEvent(IdToTouchDictionary[id], id, TouchActionType.Cancelled, touch, false);
                }
                IdToTouchDictionary.Remove(id);
            }
        }

        void CheckForBoundaryHop(UITouch touch)
        {
            nint id = touch.Handle;

            // TODO: Might require converting to a List for multiple hits
            TouchRecognizer recognizerHit = null;

            foreach (UIView view in ViewDictionary.Keys)
            {
                CGPoint location = touch.LocationInView(view);

                if (new CGRect(new CGPoint(), view.Frame.Size).Contains(location))
                {
                    recognizerHit = ViewDictionary[view];
                }
            }

            if (recognizerHit != IdToTouchDictionary[id])
            {
                if (IdToTouchDictionary[id] != null)
                {
                    FireEvent(IdToTouchDictionary[id], id, TouchActionType.Exited, touch, true);
                }
                if (recognizerHit != null)
                {
                    FireEvent(recognizerHit, id, TouchActionType.Entered, touch, true);
                }
                IdToTouchDictionary[id] = recognizerHit;
            }
        }

        private void FireEvent(TouchRecognizer recognizer, nint id, TouchActionType actionType, UITouch touch, bool isInContact)
        {
            // Convert touch location to Xamarin.Forms Point value
            CGPoint cgPoint = touch.LocationInView(recognizer.View);
            TouchTrackingPoint xfPoint = new TouchTrackingPoint((float)cgPoint.X, (float)cgPoint.Y);

            // Get the method to call for firing events
            Action<UIView, TouchActionEventArgs> onTouchAction = recognizer._touchHandler.OnTouchAction;

            // Call that method
            onTouchAction(recognizer._view,
                new TouchActionEventArgs(id, actionType, xfPoint, isInContact));
        }
    }
}
