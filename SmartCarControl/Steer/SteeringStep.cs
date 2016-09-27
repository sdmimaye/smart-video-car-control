using SmartCarControl.Utils;
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
        public uint Crc { get; set; }

        public override int GetHashCode() {
            return base.GetHashCode();
        }

        public override bool Equals(object obj) {
            if (obj == this) return true;

            SteeringStep step = obj as SteeringStep;
            if (step == null) return false;

            if (step.Direction != Direction) return false;
            if (step.DirectionPercentage != DirectionPercentage) return false;
            if (step.SpeedPercentage != SpeedPercentage) return false;
            if (step.CamDirection != CamDirection) return false;
            if (step.CamDirectionPercentage != CamDirectionPercentage) return false;

            return true;
        }

        internal SteeringStep Copy() {
            return new SteeringStep {
                Direction = Direction,
                DirectionPercentage = DirectionPercentage,
                SpeedPercentage = SpeedPercentage,
                CamDirection = CamDirection,
                CamDirectionPercentage = CamDirectionPercentage
            };
        }

        public SteeringStep WithDirection(MovingDirection direction, bool value) {
            if (value) {
                Direction |= direction;
            }else {
                Direction &= ~direction;
            }

            return this;
        }

        public SteeringStep WithCamDirection(CameraDirection direction, bool value) {
            if (value) {
                CamDirection |= direction;
            } else {
                CamDirection &= ~direction;
            }

            return this;
        }

        public SteeringStep WithSpeed(double speed) {
            SpeedPercentage = speed;
            return this;
        }

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

                if(Crc != 0)
                    stream.PutUInt32(Crc);

                return stream.ToArray();
            }
        }
    }
}