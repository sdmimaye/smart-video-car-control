namespace SmartCarControl.Steer.Engines {
    public interface ISteeringEngine {
        void StartEngine();
        void ExecuteStep(SteeringStep step);
        void EndEngine();
    }
}