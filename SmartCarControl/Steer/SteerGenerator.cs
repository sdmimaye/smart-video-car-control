using SlimDX.DirectInput;
using SmartCarControl.Controllers;
using System;
using System.Collections.Generic;

namespace SmartCarControl.Steer {
    public class SteerGenerator : IDisposable {
        public bool IsDisposed { get; private set; }

        private DirectInput Input { get; }
        private List<ICarController> Controllers { get; }

        public SteerGenerator(IntPtr handle) {
            Input = new DirectInput();
            Controllers = new List<ICarController>(new ICarController[] {
                new KeyboardCarController(handle, Input),
                new GamepadCarController(handle, Input)
            });
            IsDisposed = false;
        }

        public SteeringStep Update() {
            foreach(var ctrl in Controllers) {
                var step = ctrl.Update();
                if (step != null)
                    return step;
            }
            return null;
        }

        public void Dispose() {
            foreach(var ctrl in Controllers) {
                ctrl.Dispose();
            }
            IsDisposed = true;
        }
    }
}
