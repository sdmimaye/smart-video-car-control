namespace SmartCarControl.Steer {
    public interface ISteeringEngine {
        void StartEngine();
        void ExecuteStep(SteeringStep step);
        void EndEngine();
    }
}