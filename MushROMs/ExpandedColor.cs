using System;
using System.ComponentModel;
using System.Drawing;

namespace MushROMs
{
    public struct ExpandedColor
    {
        private const float ColorMax = 255.0f;
        private const float HueMax = 360.0f;
        private const float LumMax = 240.0f;
        private const float SatMax = LumMax;
        private const float PercentMax = 100.0f;
        private const float MinError = 0.0001f;

        public const float LumaRedWeight = 30;
        public const float LumaGreenWeight = 59;
        public const float LumaBlueWeight = 11;

        private float alpha;
        private float red;
        private float green;
        private float blue;

        private float cyan;
        private float magenta;
        private float yellow;

        private float hue;
        private float sat;
        private float lum;

        public float A
        {
            get { return this.alpha; }
        }
        public float R
        {
            get { return this.red; }
        }
        public float G
        {
            get { return this.green; }
        }
        public float B
        {
            get { return this.blue; }
        }

        public float C
        {
            get { return this.cyan; }
        }
        public float M
        {
            get { return this.magenta; }
        }
        public float Y
        {
            get { return this.yellow; }
        }

        public float H
        {
            get { return this.hue; }
        }
        public float S
        {
            get { return this.sat; }
        }
        public float L
        {
            get { return this.lum; }
        }

        private void CalculateRGBtoHSL()
        {
            float r = this.red / ColorMax;
            float g = this.green / ColorMax;
            float b = this.blue / ColorMax;

            float max = Math.Max(r, Math.Max(g, b));
            float min = Math.Min(r, Math.Min(g, b));
            float chroma = max - min;

            float l = (max + min) / 2.0f;
            float s = 0.0f;
            float h = 0.0f;

            if (chroma != 0)
            {
                s = chroma / (1 - Math.Abs((2 * l) - 1));

                if (max == r)
                    h = (g - b) / chroma;
                else if (max == g)
                    h = 2.0f + (b - r) / chroma;
                else
                    h = 4.0f + (r - g) / chroma;
                if (h < 0)
                    h += 6.0f;
            }

            this.hue = h * HueMax / 6.0f;
            this.sat = s * SatMax;
            this.lum = l * LumMax;
        }

        private void CalculateHSLtoRGB()
        {
            float h = this.hue / (HueMax / 6.0f);
            float s = this.sat / SatMax;
            float l = this.lum / LumMax;

            float r = 0;
            float g = 0;
            float b = 0;

            float c = (1 - Math.Abs((2 * l) - 1)) * s;
            float x = c * (1 - Math.Abs((h % 2) - 1));

            if (h >= 0 && h < 1)
            { r = c; g = x; }
            else if (h >= 1 && h < 2)
            { r = x; g = c; }
            else if (h >= 2 && h < 3)
            { g = c; b = x; }
            else if (h >= 3 && h < 4)
            { g = x; b = c; }
            else if (h >= 4 && h < 5)
            { r = x; b = c; }
            else
            { r = c; b = x; }

            float m = l - (c / 2);
            r += m;
            g += m;
            b += m;

            this.red = r * ColorMax;
            this.green = g * ColorMax;
            this.blue = b * ColorMax;
        }

        private void CalculateRGBtoCMY()
        {
            this.cyan = this.magenta = this.yellow = 0;
            float black = ColorMax - Math.Min(this.cyan, Math.Min(this.magenta, this.yellow));
            if (black != 0)
            {
                this.cyan = ColorMax * (black - this.red) / black;
                this.magenta = ColorMax * (black - this.green) / black;
                this.yellow = ColorMax * (black - this.blue) / black;
            }
        }

        private void CMYtoRGB()
        {
            float white = ColorMax - Math.Min(this.cyan, Math.Min(this.magenta, this.yellow));
            this.red = (ColorMax - this.cyan) * white / ColorMax;
            this.green = (ColorMax - this.magenta) * white / ColorMax;
            this.blue = (ColorMax - this.yellow) * white / ColorMax;
        }

        public ExpandedColor Modify(float hDegrees, float sPercent, float lPercent)
        {
            hDegrees = MathHelper.RoundToBoundary(hDegrees, -HueMax / 2, HueMax / 2, MinError);
            sPercent = MathHelper.RoundToBoundary(sPercent, -PercentMax, PercentMax, MinError);
            lPercent = MathHelper.RoundToBoundary(lPercent, -PercentMax, PercentMax, MinError);

            if (hDegrees < -HueMax / 2 || hDegrees > HueMax / 2)
                throw new ArgumentOutOfRangeException("hDegrees");
            if (sPercent < -PercentMax || sPercent > PercentMax)
                throw new ArgumentOutOfRangeException("sPercent");
            if (lPercent < -PercentMax || lPercent > PercentMax)
                throw new ArgumentOutOfRangeException("lPercent");

            float h = this.hue + hDegrees;
            if (h < 0)
                h += HueMax;
            if (h > HueMax)
                h -= HueMax;

            float s = this.sat + ((sPercent > 0 ? SatMax - this.sat : this.sat) * sPercent / PercentMax);
            float l = this.lum + ((lPercent > 0 ? LumMax - this.lum : this.lum) * lPercent / PercentMax);
            return ExpandedColor.FromAhsl(this.alpha, h, s, l);
        }

        public ExpandedColor Colorize(float hDegrees, float sPercent, float lPercent)
        {
            return Colorize(hDegrees, sPercent, lPercent, PercentMax);
        }

        public ExpandedColor Colorize(float hDegrees, float sPercent, float lPercent, float eff)
        {
            hDegrees = MathHelper.RoundToBoundary(hDegrees, 0, HueMax, MinError);
            sPercent = MathHelper.RoundToBoundary(sPercent, 0, PercentMax, MinError);
            lPercent = MathHelper.RoundToBoundary(lPercent, 0, PercentMax, MinError);
            eff = MathHelper.RoundToBoundary(eff, 0, PercentMax, MinError);

            if (hDegrees < 0 || hDegrees > HueMax)
                throw new ArgumentOutOfRangeException("hDegrees");
            if (sPercent < 0 || sPercent > PercentMax)
                throw new ArgumentOutOfRangeException("sPercent");
            if (lPercent < 0 || lPercent > PercentMax)
                throw new ArgumentOutOfRangeException("lPercent");
            if (eff < 0 || eff > PercentMax)
                throw new ArgumentOutOfRangeException("eff");

            const float half = PercentMax / 2.0f;
            float l = this.lum + ((lPercent > half ? LumMax - this.lum : this.lum) * (lPercent - half) / half);
            ExpandedColor c = ExpandedColor.FromAhsl(hDegrees, SatMax * (sPercent / PercentMax), l);
            return ExpandedColor.FromArgb(this.alpha, (eff * c.red + (PercentMax - eff) * this.red) / PercentMax,
                                                      (eff * c.green + (PercentMax - eff) * this.green) / PercentMax,
                                                      (eff * c.blue + (PercentMax - eff) * this.blue) / PercentMax);
        }

        public ExpandedColor Invert()
        {
            return ExpandedColor.FromArgb(this.alpha, ColorMax - this.red, ColorMax - this.green, ColorMax - this.blue);
        }

        public ExpandedColor LumaGrayscale()
        {
            return Grayscale(LumaRedWeight, LumaGreenWeight, LumaBlueWeight);
        }

        public ExpandedColor Grayscale(float red, float green, float blue)
        {
            red = MathHelper.RoundTo(red, 0, MinError);
            green = MathHelper.RoundTo(green, 0, MinError);
            blue = MathHelper.RoundTo(blue, 0, MinError);

            if (red < 0)
                throw new ArgumentOutOfRangeException("red");
            if (green < 0)
                throw new ArgumentOutOfRangeException("green");
            if (blue < 0)
                throw new ArgumentOutOfRangeException("blue");

            float total = red + green + blue;
            if (total == 0)
                return Color.Empty;

            red /= total;
            green /= total;
            blue /= total;

            float lum = red * (this.red / ColorMax) + green * (this.green / ColorMax) + blue * (this.blue / ColorMax);
            return ExpandedColor.FromAhsl(this.alpha, this.hue, 0, lum * LumMax);
        }

        public ExpandedColor Blend(ExpandedColor bottom, BlendMode mode)
        {
            float[] c1 = { this.red / ColorMax, this.green / ColorMax, this.blue / ColorMax };
            float[] c2 = { bottom.red / ColorMax, bottom.green / ColorMax, bottom.blue / ColorMax };
            float[] c3 = new float[c1.Length];

            float l1 = this.lum / LumMax;
            switch (mode)
            {
                case BlendMode.Multiply:
                    for (int i = c1.Length; --i >= 0; )
                        c3[i] = c1[i] * c2[i];
                    break;
                case BlendMode.Screen:
                    for (int i = c1.Length; --i >= 0; )
                        c3[i] = 1 - (1 - c1[i]) * (1 - c2[i]);
                    break;
                case BlendMode.Overlay:
                    for (int i = c1.Length; --i >= 0; )
                    {
                        if (c1[i] < 0.5)
                            c3[i] = 2 * c1[i] * c2[i];
                        else
                            c3[i] = 1 - 2 * (1 - c1[i]) * (1 - c2[i]);
                    }
                    break;
                case BlendMode.HardLight:
                    for (int i = c1.Length; --i >= 0; )
                    {
                        if (c2[i] < 0.5)
                            c3[i] = 2 * c1[i] * c2[i];
                        else
                            c3[i] = 1 - 2 * (1 - c1[i]) * (1 - c2[i]);
                    }
                    break;
                case BlendMode.SoftLight:
                    for (int i = c1.Length; --i >= 0; )
                    {
                        if (c2[i] < 0.5)
                            c3[i] = 2 * c1[i] * c2[i] + c1[i] * c1[i] * (1 - 2 * c2[i]);
                        else
                            c3[i] = 2 * c1[i] * (1 - c2[i]) + (float)Math.Sqrt(c1[i]) * (2 * c2[i] - 1);
                    }
                    break;
                case BlendMode.ColorDodge:
                    for (int i = c1.Length; --i >= 0; )
                    {
                        if (c1[i] < 1)
                        {
                            c3[i] = c2[i] / (1 - c1[i]);
                            if (c3[i] > 1)
                                c3[i] = 1;
                        }
                        else
                            c3[i] = 1;
                    }
                    break;
                case BlendMode.LinearDodge:
                    for (int i = c1.Length; --i >= 0; )
                    {
                        c3[i] = c1[i] + c2[i];
                        if (c3[i] > 1)
                            c3[i] = 1;
                    }
                    break;
                case BlendMode.ColorBurn:
                    for (int i = c1.Length; --i >= 0; )
                    {
                        if (c1[i] > 0)
                        {
                            c3[i] = (1 - c2[i]) / c1[i];
                            if (c3[i] > 1)
                                c3[i] = 0;
                            else
                                c3[i] = 1 - c3[i];
                        }
                        else
                            c3[i] = 0;
                    }
                    break;
                case BlendMode.LinearBurn:
                    for (int i = c1.Length; --i >= 0; )
                    {
                        c3[i] = c1[i] + c2[i] - 1;
                        if (c3[i] < 0)
                            c3[i] = 0;
                    }
                    break;
                case BlendMode.VividLight:
                    for (int i = c1.Length; --i >= 0; )
                    {
                        if (l1 > 0.5)
                        {
                            if (c1[i] < 1)
                            {
                                c3[i] = c2[i] / (1 - c1[i]);
                                if (c3[i] > 1)
                                    c3[i] = 1;
                            }
                            else
                                c3[i] = 1;
                        }
                        else
                        {
                            if (c1[i] > 0)
                            {
                                c3[i] = (1 - c2[i]) / c1[i];
                                if (c3[i] > 1)
                                    c3[i] = 0;
                                else
                                    c3[i] = 1 - c3[i];
                            }
                            else
                                c3[i] = 0;
                        }
                    }
                    break;
                case BlendMode.LinearLight:
                    for (int i = c1.Length; --i >= 0; )
                    {
                        if (l1 > 0.5)
                        {
                            c3[i] = c1[i] + c2[i];
                            if (c3[i] > 1)
                                c3[i] = 1;
                        }
                        else
                        {
                            c3[i] = c1[i] + c2[i] - 1;
                            if (c3[i] < 0)
                                c3[i] = 0;
                        }
                    }
                    break;
                case BlendMode.Difference:
                    for (int i = c1.Length; --i >= 0; )
                        c3[i] = Math.Max(c1[i], c2[i]) - Math.Min(c1[i], c2[i]);
                    break;
                case BlendMode.Darken:
                    for (int i = c1.Length; --i >= 0; )
                        c3[i] = Math.Min(c1[i], c2[i]);
                    break;
                case BlendMode.Lighten:
                    for (int i = c1.Length; --i >= 0; )
                        c3[i] = Math.Max(c1[i], c2[i]);
                    break;
                default:
                    throw new InvalidEnumArgumentException("mode", (int)mode, typeof(BlendMode));
            }
            return ExpandedColor.FromArgb(c3[0] * ColorMax, c3[1] * ColorMax, c3[2] * ColorMax);
        }

        public static ExpandedColor FromArgb(float red, float green, float blue)
        {
            return ExpandedColor.FromArgb(ColorMax, red, green, blue);
        }

        public static ExpandedColor FromArgb(float alpha, float red, float green, float blue)
        {
            alpha = MathHelper.RoundToBoundary(alpha, 0, ColorMax, MinError);
            red = MathHelper.RoundToBoundary(red, 0, ColorMax, MinError);
            green = MathHelper.RoundToBoundary(green, 0, ColorMax, MinError);
            blue = MathHelper.RoundToBoundary(blue, 0, ColorMax, MinError);

            if (alpha < 0 || alpha > ColorMax)
                throw new ArgumentOutOfRangeException("alpha");
            if (red < 0 || red > ColorMax)
                throw new ArgumentOutOfRangeException("red");
            if (green < 0 || green > ColorMax)
                throw new ArgumentOutOfRangeException("green");
            if (blue < 0 || blue > ColorMax)
                throw new ArgumentOutOfRangeException("blue");

            ExpandedColor x = new ExpandedColor();
            x.alpha = alpha;
            x.red = red;
            x.green = green;
            x.blue = blue;
            x.CalculateRGBtoCMY();
            x.CalculateRGBtoHSL();
            return x;
        }

        public static ExpandedColor FromAcmy(float cyan, float magenta, float yellow)
        {
            return ExpandedColor.FromAcmy(ColorMax, cyan, magenta, yellow);
        }

        public static ExpandedColor FromAcmy(float alpha, float cyan, float magenta, float yellow)
        {
            alpha = MathHelper.RoundToBoundary(alpha, 0, ColorMax, MinError);
            cyan = MathHelper.RoundToBoundary(cyan, 0, ColorMax, MinError);
            magenta = MathHelper.RoundToBoundary(magenta, 0, ColorMax, MinError);
            yellow = MathHelper.RoundToBoundary(yellow, 0, ColorMax, MinError);

            if (alpha < 0 || alpha > ColorMax)
                throw new ArgumentOutOfRangeException("alpha");
            if (cyan < 0 || cyan > ColorMax)
                throw new ArgumentOutOfRangeException("cyan");
            if (magenta < 0 || magenta > ColorMax)
                throw new ArgumentOutOfRangeException("magenta");
            if (yellow < 0 || yellow > ColorMax)
                throw new ArgumentOutOfRangeException("yellow");

            ExpandedColor x = new ExpandedColor();
            x.alpha = alpha;
            x.cyan = cyan;
            x.magenta = magenta;
            x.yellow = yellow;
            x.CMYtoRGB();
            x.CalculateRGBtoHSL();
            return x;
        }

        public static ExpandedColor FromAhsl(float hue, float sat, float lum)
        {
            return ExpandedColor.FromAhsl(ColorMax, hue, sat, lum);
        }

        public static ExpandedColor FromAhsl(float alpha, float hue, float sat, float lum)
        {
            alpha = MathHelper.RoundToBoundary(alpha, 0, ColorMax, MinError);
            hue = MathHelper.RoundToBoundary(hue, 0, HueMax, MinError);
            sat = MathHelper.RoundToBoundary(sat, 0, SatMax, MinError);
            lum = MathHelper.RoundToBoundary(lum, 0, LumMax, MinError);

            if (alpha < 0 || alpha > ColorMax)
                throw new ArgumentOutOfRangeException("alpha");
            if (hue < 0 || hue > HueMax)
                throw new ArgumentOutOfRangeException("hue");
            if (sat < 0 || sat > SatMax)
                throw new ArgumentOutOfRangeException("sat");
            if (lum < 0 || lum > LumMax)
                throw new ArgumentOutOfRangeException("lum");

            ExpandedColor x = new ExpandedColor();
            x.alpha = alpha;
            x.hue = hue;
            x.sat = sat;
            x.lum = lum;
            x.CalculateHSLtoRGB();
            x.CalculateRGBtoCMY();
            return x;
        }

        public override string ToString()
        {
            return ((Color)this).ToString();
        }

        public static implicit operator ExpandedColor(Color color)
        {
            return ExpandedColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static explicit operator Color(ExpandedColor color)
        {
            return Color.FromArgb((int)(color.alpha + 0.5f), (int)(color.red + 0.5f), (int)(color.green + 0.5f), (int)(color.blue + 0.5f));
        }

        public override bool Equals(object obj)
        {
            return this == (ExpandedColor)obj;
        }

        public override int GetHashCode()
        {
            return ((Color)this).GetHashCode();
        }

        public static bool operator ==(ExpandedColor left, ExpandedColor right)
        {
            return (Color)left == (Color)right;
        }

        public static bool operator !=(ExpandedColor left, ExpandedColor right)
        {
            return (Color)left != (Color)right;
        }
    }
}