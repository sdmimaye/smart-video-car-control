namespace SmartCarControl.Steering {
    public interface ISteeringEngine {
        void StartEngine(string host);
        void EndEngine();
    }
}