using System;

namespace MushROMs.Controls
{
    [Flags]
    public enum SizingDirections
    {
        None = 0,
        Left = 1 << 0,
        Top = 1 << 1,
        Right = 1 << 2,
        Bottom = 1 << 3,
        TopLeft = Top | Left,
        TopRight = Top | Right,
        BottomLeft = Bottom | Left,
        BottomRight = Bottom | Right,
        All = Left | Top | Right | Bottom
    }
}
