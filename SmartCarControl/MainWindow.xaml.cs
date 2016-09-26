using System.Windows;
using System.Windows.Input;
using SmartCarControl.Steer;

namespace SmartCarControl {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private readonly ISteeringEngine _engine;
        private readonly SteerGenerator _generator;
        private readonly Classes.Gamepad _gamepad;

        public MainWindow() {
            InitializeComponent();
            _engine = new UdpSteeringEngine();
            _engine.StartEngine();
            _generator = new SteerGenerator();
            _gamepad = new Classes.Gamepad(HandleJoystick);

            HandleKey(Key.None, SteerGenerator.KeyMovement.Up);
        }

        private void MainWindow_OnKeyEvent(object sender, KeyEventArgs e) {
            var movement = e.IsDown ? SteerGenerator.KeyMovement.Down : SteerGenerator.KeyMovement.Up;
            HandleKey(e.Key, movement);
        }

        private void HandleStep(SteeringStep step) {
            Car.IsTopActive = step.SpeedPercentage > 0;
            Car.IsBottomActive = step.SpeedPercentage < 0;
            Car.IsRightActive = (step.Direction & SteeringStep.MovingDirection.Right) == SteeringStep.MovingDirection.Right;
            Car.IsLeftActive = (step.Direction & SteeringStep.MovingDirection.Left) == SteeringStep.MovingDirection.Left;

            Camera.IsTopActive = (step.CamDirection & SteeringStep.CameraDirection.Up) == SteeringStep.CameraDirection.Up;
            Camera.IsBottomActive = (step.CamDirection & SteeringStep.CameraDirection.Down) == SteeringStep.CameraDirection.Down;
            Camera.IsRightActive = (step.CamDirection & SteeringStep.CameraDirection.Right) == SteeringStep.CameraDirection.Right;
            Camera.IsLeftActive = (step.CamDirection & SteeringStep.CameraDirection.Left) == SteeringStep.CameraDirection.Left;

            _engine.ExecuteStep(step);
        }

        private void HandleJoystick(SlimDX.DirectInput.JoystickState state) {
            HandleStep(_generator.Update(state));
        }

        private void HandleKey(Key key, SteerGenerator.KeyMovement movement) {
            HandleStep(_generator.Update(key, movement));
        }
    }
}
