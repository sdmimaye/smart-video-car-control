using System.Windows;
using SmartCarControl.Steer;
using System;
using System.Threading;
using System.Windows.Interop;
using SmartCarControl.Steer.Engines;

namespace SmartCarControl {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private readonly ISteeringEngine _engine;
        private readonly SteerGenerator _generator;

        public MainWindow() {
            InitializeComponent();
            _engine = new UdpSteeringEngine();
            _engine.StartEngine();

            var interop = new WindowInteropHelper(this);
            _generator = new SteerGenerator(interop.Owner);
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

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            ThreadPool.QueueUserWorkItem(delegate {
                while (!_generator.IsDisposed)
                {
                    var step = _generator.Update();
                    if(step == null)
                        continue;

                    Dispatcher.BeginInvoke(new Action(delegate {
                        HandleStep(step);
                    }));
                    Thread.Sleep(1);
                }
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            _generator.Dispose();
        }
    }
}
