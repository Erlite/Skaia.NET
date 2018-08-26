using Skaia.Utils;
using System;

namespace Skaia.Serialization
{
    public abstract class NumericCompressible<T> : ICompressible<T>
    {
        public abstract bool CompressionEnabled { get; set; }

        public abstract T MinValue { get; set; }
        public abstract T MaxValue { get; set; }
        public abstract T Value { get; set; }

        protected abstract T GetCompressedValue();
        protected abstract T GetDefaultDeserialization(byte[] data);
        protected abstract byte[] GetDefaultSerialization();
        protected abstract int GetSizeOf();
        protected abstract uint GetValueRange();
        protected abstract T SetValueFromDecompressed(T decompressed);

        /// <summary>
        /// Compress the value into a byte array.
        /// </summary>
        /// <returns></returns>
        public virtual byte[] Compress()
        {
            if (!CompressionEnabled)
            {
                return GetDefaultSerialization();
            }

            uint range = GetValueRange();
            uint requiredBytes = Arithmetics<T>.GetRequiredBytes(range, GetSizeOf());

            T compressed = GetCompressedValue();

            byte[] source = GetDefaultSerialization();
            byte[] destination = new byte[requiredBytes];

            Array.Copy(source, destination, requiredBytes);
            return destination;
        }

        /// <summary>
        /// Decompresses the value from a byte array.
        /// </summary>
        /// <param name="data"></param>
        public virtual T Decompress(byte[] data)
        {
            if (!CompressionEnabled)
            {
                Value = GetDefaultDeserialization(data);
                return Value;
            }

            byte[] decompressed = new byte[GetSizeOf()];
            Array.Copy(data, decompressed, data.Length);
            T decompressedValue = GetDefaultDeserialization(data);

            return SetValueFromDecompressed(decompressedValue);
        }
    }
}