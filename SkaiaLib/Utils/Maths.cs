
namespace Skaia.Utils
{
    public static class Maths
    {
        public static uint GetRequiredBits(uint number, int size)
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

        public static uint GetRequiredBytes(uint number, int size)
        {
            uint bits = GetRequiredBits(number, size);
            return (bits + 7) >> 3;
        }
    }
}