using System;
using System.Drawing;

namespace MushROMs
{
    public class View
    {
        private Map map;
        private Rectangle region;

        public Map Map
        {
            get { return this.map; }
        }

        public Point ZeroPoint
        {
            get { return this.region.Location; }
        }
        public int ZeroX
        {
            get { return this.ZeroPoint.X; }
        }
        public int ZeroY
        {
            get { return this.ZeroPoint.Y; }
        }

        public Size ViewSize
        {
            get { return this.region.Size; }
        }
        public int ViewWidth
        {
            get { return this.ViewSize.Width; }
        }
        public int ViewHeight
        {
            get { return this.ViewSize.Height; }
        }

        public View(Map map, Rectangle region)
        {
            this.map = map;
            this.region = region;
        }
    }
}