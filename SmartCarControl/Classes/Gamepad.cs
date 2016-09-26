using SlimDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SmartCarControl.Classes {
    public class Gamepad {
        private readonly Action<JoystickState> _callback;

        public Gamepad(Action<JoystickState> callback) {
            _callback = callback;
            ThreadPool.QueueUserWorkItem(WorkerThread);
        }

        private void WorkerThread(object obj) {
            var input = new DirectInput();
            while (!input.Disposed) {
                try {
                    using (var pad = getAllConnectedGamepads(input).FirstOrDefault()) {
                        if (pad == null)
                            continue;

                        pad.Acquire();
                        JoystickState state = new JoystickState();
                        while (pad.Poll().IsSuccess && pad.GetCurrentState(ref state).IsSuccess) {
                            
                        }
                    }
                } catch (Exception ex) {
                    System.Diagnostics.Debug.WriteLine("Error while handling GamePad: " + ex.Message);
                } finally {
                    Thread.Sleep(1000);
                }
            }
        }

        private IEnumerable<Joystick> getAllConnectedGamepads(DirectInput input) {
            foreach (var di in input.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AttachedOnly)) {
                yield return new Joystick(input, di.InstanceGuid);
            }
        }
    }
}
