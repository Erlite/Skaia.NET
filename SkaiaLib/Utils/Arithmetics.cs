using System;

namespace Skaia.Utils
{
    internal static class Arithmetics<T>
    {
        internal static uint GetRequiredBits(uint number, int size)
        {
            size = (size * 8) - 1;

            for (int i = size; i >= 0; --i)
            {
                uint b = 1U << i;

                if ((number & b) == b)
                {
                    return (uint)i + 1;
                }
            }

            return 0;
        }

        internal static uint GetRequiredBytes(uint number, int size)
        {
            uint bits = GetRequiredBits(number, size);
            return (bits + 7) >> 3;
        }

        /// <summary>
        /// This was made to substract two generics and give the result for Compressible types. Not pretty, but it's performant.
        /// </summary>
        internal static T Subtract(T value, T deduction)
        {
            if (typeof(T) == typeof(int))
            {
                return (T)(object)((int)(object)value - (int)(object)deduction);
            }
            else if (typeof(T) == typeof(uint))
            {
                return (T)(object)((uint)(object)value - (int)(object)deduction);
            }
            else if (typeof(T) == typeof(long))
            {
                return (T)(object)((long)(object)value - (long)(object)deduction);
            }
            else
            {
                // Should default back to slow path, such as using reflection
                // to check for op_Subtract and calling that
                throw new NotImplementedException();
            }
        }
    }
}