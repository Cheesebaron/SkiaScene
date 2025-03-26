namespace TouchTracking.MAUI;

public static class MauiHostingExtensions
{
    public static void InitTouchTrackingEffects(this IEffectsBuilder effects)
    {
#if __MACCATALYST__
        effects.Add<TouchTracking.MAUI.TouchEffect, TouchTracking.MAUI.Platforms.MacCatalyst.TouchPlatformEffect>();
#elif __IOS__
        effects.Add<TouchTracking.MAUI.TouchEffect, TouchTracking.MAUI.Platforms.iOS.TouchPlatformEffect>();
#elif __ANDROID__
        effects.Add<TouchTracking.MAUI.TouchEffect, TouchTracking.MAUI.Platforms.Android.TouchPlatformEffect>();
#endif
    }
}
