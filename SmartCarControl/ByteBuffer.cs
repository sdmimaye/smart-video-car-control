using System;
using System.IO;

namespace SmartCarControl {
    public class ByteBuffer : IDisposable {
        public enum ByteOrder {
            BigEndian,
            LittleEndian
        }

        private readonly MemoryStream _stream;
        public ByteOrder Endian { get; set; }
        public bool IsUsingSystemByteOrder => GetSystemByteOrder() == Endian;

        public ByteBuffer() {
            _stream = new MemoryStream();
            Endian = GetSystemByteOrder();
        }

        public byte[] ToArray() {
            return _stream.ToArray();
        }

        public static ByteOrder GetSystemByteOrder() {
            return BitConverter.IsLittleEndian ? ByteOrder.LittleEndian : ByteOrder.BigEndian;
        }

        private byte[] DoRevertIfRequired(byte[] data) {
            if (!IsUsingSystemByteOrder)
                Array.Reverse(data);

            return data;
        }

        public ByteBuffer PutByte(byte value) {
            _stream.WriteByte(value);
            return this;
        }

        public ByteBuffer PutBytes(byte[] value) {
            _stream.Write(value, 0, value.Length);
            return this;
        }

        public ByteBuffer PutInt32(int value) {
            return PutBytes(DoRevertIfRequired(BitConverter.GetBytes(value)));
        }

        public ByteBuffer PutDouble(double value) {
            return PutBytes(DoRevertIfRequired(BitConverter.GetBytes(value)));
        }

        public void Dispose() {
            _stream.Dispose();
        }
    }
}