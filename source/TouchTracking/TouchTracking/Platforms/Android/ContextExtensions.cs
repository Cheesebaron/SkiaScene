using System;
using Android.Content;
using Android.Util;

namespace TouchTracking.Platforms.Android
{
    public static class ContextExtensions
    {
        static float _displayDensity = float.MinValue;

        public static double FromPixels(this Context self, double pixels)
        {
            SetupMetrics(self);
            return pixels / _displayDensity;
        }

        static void SetupMetrics(Context context)
        {
            if (_displayDensity != float.MinValue)
            {
                return;
            }

            using (DisplayMetrics metrics = context.Resources.DisplayMetrics)
            {
                _displayDensity = metrics.Density;
            }
        }

        public static float ToPixels(this Context self, double dp)
        {
            SetupMetrics(self);

            return (float)Math.Round(dp * _displayDensity);
        }
    }
}
