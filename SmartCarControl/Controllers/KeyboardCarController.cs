using SmartCarControl.Steer;
using System;
using SlimDX.DirectInput;
using System.Diagnostics;
using System.Linq;

namespace SmartCarControl.Controllers {
    public class KeyboardCarController : ICarController {
        private static readonly double VALOCITY = 0.8;
        private readonly Keyboard _keyboard;

        private KeyboardState _last;

        public KeyboardCarController(IntPtr handle, DirectInput input) {
            _keyboard = new Keyboard(input);
            _keyboard.SetCooperativeLevel(handle, CooperativeLevel.Nonexclusive | CooperativeLevel.Background);
        }

        public void Dispose() {
            _keyboard.Unacquire();
            _keyboard.Dispose();
        }

        private bool IsKeyboardUpdateRequired(KeyboardState state) {
            try {
                return state.PressedKeys.Count != _last?.PressedKeys.Count;
            } finally {
                _last = state;
            }
        }

        public SteeringStep Update() {
            try {
                if (_keyboard.Acquire().IsFailure)
                    return null;

                if (_keyboard.Poll().IsFailure)
                    return null;

                var state = _keyboard.GetCurrentState();
                if (!IsKeyboardUpdateRequired(state))
                    return null;

                var step = new SteeringStep {DirectionPercentage = VALOCITY, CamDirectionPercentage = VALOCITY};
                step.WithDirection(SteeringStep.MovingDirection.Left, state.PressedKeys.Contains(Key.A) && !state.PressedKeys.Contains(Key.D));
                step.WithDirection(SteeringStep.MovingDirection.Right, !state.PressedKeys.Contains(Key.A) && state.PressedKeys.Contains(Key.D));
                step.SpeedPercentage = state.PressedKeys.Contains(Key.W) ? VALOCITY : state.PressedKeys.Contains(Key.S) ? -VALOCITY : 0.0;
                step.WithCamDirection(SteeringStep.CameraDirection.Up, state.PressedKeys.Contains(Key.UpArrow) && !state.PressedKeys.Contains(Key.DownArrow));
                step.WithCamDirection(SteeringStep.CameraDirection.Down, state.PressedKeys.Contains(Key.DownArrow) && !state.PressedKeys.Contains(Key.UpArrow));
                step.WithCamDirection(SteeringStep.CameraDirection.Left, state.PressedKeys.Contains(Key.LeftArrow) && !state.PressedKeys.Contains(Key.RightArrow));
                step.WithCamDirection(SteeringStep.CameraDirection.Right, state.PressedKeys.Contains(Key.RightArrow) && !state.PressedKeys.Contains(Key.LeftArrow));
                return step;
            } catch (Exception ex){
                Trace.TraceError("Could not poll keyboard event(s): {0}", ex.StackTrace);
                return null;
            } finally { }
        }
    }
}
