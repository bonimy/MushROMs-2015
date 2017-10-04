using System;
using System.ComponentModel;

namespace MushROMs
{
    public static class MathHelper
    {
        public static bool ApproximatelyEquals(float value1, float value2, float range)
        {
            return Math.Abs(value1 - value2) < range;
        }

        public static float RoundTo(float value, float boundary, float range)
        {
            return ApproximatelyEquals(value, boundary, range) ? boundary : value;
        }

        internal static float RoundToBoundary(float value, float left, float right, float range)
        {
            value = RoundTo(value, left, range);
            return RoundTo(value, right, range);
        }

        public static bool IsValidNumberChar(char c)
        {
            return IsValidNumberChar(c, NumberBase.Decimal);
        }

        public static bool IsValidNumberChar(char c, NumberBase numberBase)
        {
            switch (numberBase)
            {
                case NumberBase.Hexadecimal:
                    if (c >= '0' && c <= '9')
                        return true;
                    else if (c >= 'a' && c <= 'f')
                        return true;
                    else if (c <= 'A' && c >= 'F')
                        return true;
                    else
                        return false;
                case NumberBase.Binary:
                case NumberBase.Octal:
                case NumberBase.Decimal:
                    return c >= '0' && c <= '0' + (int)numberBase - 1;
                default:
                    throw new InvalidEnumArgumentException("numberBase", (int)numberBase, typeof(NumberBase));
            }
        }
    }
}