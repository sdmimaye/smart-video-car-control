using SmartCarControl.Steer;
using System;

namespace SmartCarControl.Controllers {
    interface ICarController : IDisposable{
        SteeringStep Update();
    }
}
