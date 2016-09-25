using System.Windows;
using System.Windows.Input;
using SmartCarControl.Steer;

namespace SmartCarControl {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private readonly ISteeringEngine _engine = new UdpSteeringEngine();
        private readonly SteerGenerator _generator = new SteerGenerator();

        public MainWindow() {
            InitializeComponent();
        }

        private void MainWindow_OnKeyEvent(object sender, KeyEventArgs e) {
            var step = _generator.Update(e);
            Car.IsTopActive = step.Speed > 0;
            Car.IsBottomActive = step.Speed < 0;
            Car.IsRightActive = (step.Direction & SteeringStep.MovingDirection.Right) == SteeringStep.MovingDirection.Right;
            Car.IsLeftActive = (step.Direction & SteeringStep.MovingDirection.Left) == SteeringStep.MovingDirection.Left;

            Camera.IsTopActive = (step.CamDirection & SteeringStep.CameraDirection.Up) == SteeringStep.CameraDirection.Up;
            Camera.IsBottomActive = (step.CamDirection & SteeringStep.CameraDirection.Down) == SteeringStep.CameraDirection.Down;
            Camera.IsRightActive = (step.CamDirection & SteeringStep.CameraDirection.Right) == SteeringStep.CameraDirection.Right;
            Camera.IsLeftActive = (step.CamDirection & SteeringStep.CameraDirection.Left) == SteeringStep.CameraDirection.Left;

            _engine.ExecuteStep(step);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) {
            _engine.StartEngine();
        }
    }
}
