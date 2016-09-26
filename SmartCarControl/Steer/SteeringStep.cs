using SmartCarControl.Classes;
using System;

namespace SmartCarControl.Steer {
    public class SteeringStep {
        [Flags]
        public enum MovingDirection {
            None = 0,
            Left = 1 << 1,
            Right = 1 << 2
        }

        [Flags]
        public enum CameraDirection {
            None = 0,
            Up = 1 << 1,
            Down = 1 << 2,
            Left = 1 << 3,
            Right = 1 << 4
        }

        public MovingDirection Direction { get; set; }
        public double DirectionPercentage { get; set; }
        public double SpeedPercentage { get; set; }
        public CameraDirection CamDirection { get; set; }
        public double CamDirectionPercentage { get; set; }

        public byte[] Serialize() {
            using (var stream = new ByteBuffer { Endian = ByteBuffer.ByteOrder.BigEndian }) {
                stream.PutDouble(SpeedPercentage);
                if ((Direction & MovingDirection.Left) == MovingDirection.Left) {
                    stream.PutByte(1);
                } else if ((Direction & MovingDirection.Right) == MovingDirection.Right) {
                    stream.PutByte(2);
                } else {
                    stream.PutByte(0);
                }
                stream.PutDouble(DirectionPercentage);
                if ((CamDirection & CameraDirection.Up) == CameraDirection.Up) {
                    stream.PutByte(1);
                } else if ((CamDirection & CameraDirection.Down) == CameraDirection.Down) {
                    stream.PutByte(2);
                } else {
                    stream.PutByte(0);
                }

                stream.PutDouble(CamDirectionPercentage);
                if ((CamDirection & CameraDirection.Left) == CameraDirection.Left) {
                    stream.PutByte(1);
                } else if ((CamDirection & CameraDirection.Right) == CameraDirection.Right) {
                    stream.PutByte(2);
                } else {
                    stream.PutByte(0);
                }
                stream.PutDouble(CamDirectionPercentage);

                return stream.ToArray();
            }
        }
    }
}