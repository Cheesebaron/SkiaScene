namespace SkiaScene.TouchManipulation
{
    public interface ISceneGestureResponder
    {
        TouchManipulationMode TouchManipulationMode { get; set; }
        void StartResponding();
        void StopResponding();
    }
}
