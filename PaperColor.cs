using SixLabors.ImageSharp;

namespace PPMLib
{
    public enum PaperColor
    {
        White = 1,
        Black = 0
    }
    
    public static class PaperColorExt
    {
        public static Color ToColor(this PaperColor color) => color switch
        {
            PaperColor.White => Color.FromRgba(0xFF, 0xFF, 0xFF, 0xFF),
            PaperColor.Black => Color.FromRgba(0x00, 0x00, 0x00, 0xFF),
            _ => Color.FromRgba(0x00, 0x00, 0x00, 0xFF)
        };
        
        public static Color ToInvertedColor(this PaperColor color) => color switch
        {
            PaperColor.White => Color.FromRgba(0x00, 0x00, 0x00, 0xFF),
            PaperColor.Black => Color.FromRgba(0xFF, 0xFF, 0xFF, 0xFF),
            _ => Color.FromRgba(0x00, 0x00, 0x00, 0xFF)
        };
    }
}