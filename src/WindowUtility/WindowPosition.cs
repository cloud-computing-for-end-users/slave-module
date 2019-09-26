using System;

namespace window_utility
{
    public struct Rect
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
    }

    public class WindowPosition
    {
        public int Left { get; }
        public int Top { get; }
        public int Right { get; }
        public int Bottom { get; }
        public int Width => Right - Left;
        public int Height => Bottom - Top;

        public WindowPosition(Rect rect, float scalingFactor)
        {
            int offset = 10;
            Left = (int)(Convert.ToDouble(rect.Left) * scalingFactor) + offset;
            Right = (int)(Convert.ToDouble(rect.Right) * scalingFactor) - offset;
            Top = (int)(Convert.ToDouble(rect.Top) * scalingFactor);
            Bottom = (int)(Convert.ToDouble(rect.Bottom) * scalingFactor) - offset;
        }

        public override string ToString()
        {
            return Left + " " + Top + " " + Right + " " + Bottom + "(" + Width + ", " + Height+ ")";
        }
    }
}