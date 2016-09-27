using SmartCarControl.Steer;
using SlimDX.DirectInput;
using System;
using System.Linq;
using SlimDX;

namespace SmartCarControl.Controllers {
    public class GamepadCarController : ICarController {
        private readonly DirectInput _input;
        private readonly IntPtr _handle;
        private Joystick _joystick;
        private JoystickState _last;

        public GamepadCarController(IntPtr ctrl, DirectInput input) {
            _input = input;
            _handle = ctrl;
        }

        public void Dispose() {
            if (_joystick == null)
                return;

            _joystick.Unacquire();
            _joystick.Dispose();
            _joystick = null;
        }

        private Joystick GetCachedOrRetry() {
            if (_joystick != null)
                return _joystick;

            var pad = _input.GetDevices(DeviceType.Gamepad, DeviceEnumerationFlags.AttachedOnly).FirstOrDefault();
            if (pad == null)
                return null;

            _joystick = new Joystick(_input, pad.InstanceGuid);
            _joystick.SetCooperativeLevel(_handle, CooperativeLevel.Nonexclusive | CooperativeLevel.Background);

            return _joystick;
        }

        private bool IsGamepadUpdateRequired(JoystickState state) {
            try {
                if (_last == null)
                    return true;

                return _last.RotationY != state.RotationY;
            } finally {
                _last = state;
            }
        }

        public SteeringStep Update() {
            var pad = GetCachedOrRetry();
            if (pad == null)
                return null;

            if (pad.Acquire().IsFailure)
                return null;

            if (pad.Poll().IsFailure)
                return null;

            var state = pad.GetCurrentState();
            if (Result.Last.IsFailure)
                return null;

            if (!IsGamepadUpdateRequired(state))
                return null;

            var step = new SteeringStep();
            step.SpeedPercentage = Percentage(state.RotationY, 65535, 0);

            return step;
        }

        private double Percentage(double value, double max, double offset) {
            return 1 / (max - offset) * value;
        }
    }
}
