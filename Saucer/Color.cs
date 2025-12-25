using System;

namespace Saucer
{
    /// <summary>
    /// Represents an RGBA color.
    /// </summary>
    public struct Color : IEquatable<Color>
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(int hex) : this()
        {
            A = (byte)((hex >> 24) & 0xFF);
            R = (byte)((hex >> 16) & 0xFF);
            G = (byte)((hex >> 8) & 0xFF);
            B = (byte)(hex & 0xFF);
        }

        public override bool Equals(object? obj) => obj is Color color && Equals(color);
        public bool Equals(Color other) => R == other.R && G == other.G && B == other.B && A == other.A;
        public override int GetHashCode() => unchecked((R << 24) | (G << 16) | (B << 8) | A);
        public override string ToString() => $"#{A:X2}{R:X2}{G:X2}{B:X2}";

        public static Color Black => new(0, 0, 0, 255);
        public static Color White => new(255, 255, 255, 255);
        public static Color Red => new(255, 0, 0, 255);
        public static Color Green => new(0, 255, 0, 255);
        public static Color Blue => new(0, 0, 255, 255);
        public static Color Transparent => new(0, 0, 0, 0);

        public static bool operator ==(Color left, Color right) => left.Equals(right);
        public static bool operator !=(Color left, Color right) => !left.Equals(right);
    }
}
