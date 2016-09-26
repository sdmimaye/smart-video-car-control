using System;
using System.Windows.Input;

namespace SmartCarControl.Steer {
    public class SteerGenerator {
        private readonly Key MovementUpKey = DoParseKeyOrFallback(Properties.Settings.Default.MovementUpKey, Key.W);
        private readonly Key MovementDownKey = DoParseKeyOrFallback(Properties.Settings.Default.MovementDownKey, Key.S);
        private readonly Key MovementLeftKey = DoParseKeyOrFallback(Properties.Settings.Default.MovementLeftKey, Key.A);
        private readonly Key MovementRightKey = DoParseKeyOrFallback(Properties.Settings.Default.MovementRightKey, Key.D);
        private readonly Key CameraUpKey = DoParseKeyOrFallback(Properties.Settings.Default.CameraUpKey, Key.Up);
        private readonly Key CameraDownKey = DoParseKeyOrFallback(Properties.Settings.Default.CameraDownKey, Key.Down);
        private readonly Key CameraLeftKey = DoParseKeyOrFallback(Properties.Settings.Default.CameraLeftKey, Key.Left);
        private readonly Key CameraRightKey = DoParseKeyOrFallback(Properties.Settings.Default.CameraRightKey, Key.Right);

        private SteeringStep CurrentStep { get; }

        public SteerGenerator() {
            CurrentStep = new SteeringStep { DirectionPercentage = 0.5, CamDirectionPercentage = 0.5 };
        }

        private static Key DoParseKeyOrFallback(string key, Key fallback) {
            if (string.IsNullOrWhiteSpace(key))
                return fallback;

            try {
                return (Key)Enum.Parse(typeof(Key), key);
            } catch {
                return fallback;
            }
        }

        public enum KeyMovement {
            Up,
            Down
        }

        public SteeringStep Update(SlimDX.DirectInput.JoystickState state) {
            return CurrentStep;
        }

        public SteeringStep Update(Key key, KeyMovement movement) {
            switch (movement) {
                case KeyMovement.Up:
                    if (key == MovementUpKey || key == MovementDownKey) {
                        CurrentStep.SpeedPercentage = 0;
                    } else if (key == MovementLeftKey) {
                        CurrentStep.Direction &= ~SteeringStep.MovingDirection.Left;
                    } else if (key == MovementRightKey) {
                        CurrentStep.Direction &= ~SteeringStep.MovingDirection.Right;
                    } else if (key == CameraUpKey) {
                        CurrentStep.CamDirection &= ~SteeringStep.CameraDirection.Up;
                    } else if (key == CameraDownKey) {
                        CurrentStep.CamDirection &= ~SteeringStep.CameraDirection.Down;
                    } else if (key == CameraLeftKey) {
                        CurrentStep.CamDirection &= ~SteeringStep.CameraDirection.Left;
                    } else if (key == CameraRightKey) {
                        CurrentStep.CamDirection &= ~SteeringStep.CameraDirection.Right;
                    }
                    break;
                case KeyMovement.Down:
                    if (key == MovementUpKey) {
                        CurrentStep.SpeedPercentage = 80;
                    } else if (key == MovementDownKey) {
                        CurrentStep.SpeedPercentage = -80;
                    } else if (key == MovementLeftKey) {
                        CurrentStep.Direction |= SteeringStep.MovingDirection.Left;
                    } else if (key == MovementRightKey) {
                        CurrentStep.Direction |= SteeringStep.MovingDirection.Right;
                    } else if (key == CameraUpKey) {
                        CurrentStep.CamDirection |= SteeringStep.CameraDirection.Up;
                    } else if (key == CameraDownKey) {
                        CurrentStep.CamDirection |= SteeringStep.CameraDirection.Down;
                    } else if (key == CameraLeftKey) {
                        CurrentStep.CamDirection |= SteeringStep.CameraDirection.Left;
                    } else if (key == CameraRightKey) {
                        CurrentStep.CamDirection |= SteeringStep.CameraDirection.Right;
                    }
                    break;
            }
            return CurrentStep;
        }
    }
}
