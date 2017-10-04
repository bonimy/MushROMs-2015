using System;
using System.Collections.Generic;

namespace MushROMs
{
    public static class Helper
    {
        public static bool IsEnumValid(int value, int minValue, int maxValue)
        {
            return value >= minValue && value <= maxValue;
        }
    }
}