using SixLabors.ImageSharp;

namespace PPMLib
{
    public enum PenColor
    {
        Inverted = 1,
        Red = 2,
        Blue = 3
    }

    public static class PenColorExt
    {
        public static Color ToColor(this PenColor color) => color switch
        {
            PenColor.Red => Color.FromRgba(0xFF, 0x00, 0x00, 0xFF),
            PenColor.Blue => Color.FromRgba(0x00, 0x00, 0xFF, 0xFF),
            _ => Color.FromRgba(0x00, 0x00, 0x00, 0xFF)
        };
    }
}