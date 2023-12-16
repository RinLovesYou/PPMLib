//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PPMLib
{
    public static class PPMRenderer
    {
        public static readonly Color[] ThumbnailPalette = 
        { 
            Color.FromRgba(0xFF, 0xFF, 0xFF, 0xFF), 
            Color.FromRgba(0x52, 0x52, 0x52, 0xFF), 
            Color.FromRgba(0xFF, 0xFF, 0xFF, 0xFF), 
            Color.FromRgba(0x9C, 0x9C, 0x9C, 0xFF), 
            Color.FromRgba(0x44, 0x48, 0xFF, 0xFF),
            Color.FromRgba(0x4F, 0x51, 0xC8, 0xFF),
            Color.FromRgba(0xAC, 0xAD, 0xFF, 0xFF),
            Color.FromRgba(0x00, 0xFF, 0x00, 0xFF), 
            Color.FromRgba(0xFF, 0x40, 0x48, 0xFF), 
            Color.FromRgba(0xB8, 0x4F, 0x51, 0xFF), 
            Color.FromRgba(0xFF, 0xAB, 0xAD, 0xFF), 
            Color.FromRgba(0x00, 0xFF, 0x00, 0xFF), 
            Color.FromRgba(0xB7, 0x57, 0xB6, 0xFF), 
            Color.FromRgba(0x00, 0xFF, 0x00, 0xFF), 
            Color.FromRgba(0x00, 0xFF, 0x00, 0xFF), 
            Color.FromRgba(0x00, 0xFF, 0x00, 0xFF) 
        };
        // public static Image GetThumbnailImage(byte[] buffer)
        // {
        //     if (buffer.Length != 1536)
        //     {
        //         throw new ArgumentException("Wrong thumbnail buffer size");
        //     }
        //     // Directly set bitmap's 4-bit palette instead of using 32-bit colors 
        //     var bmp = new Image<Byte4>(64, 48)
        //     var palette = bmp.Palette;
        //     var entries = palette.Entries;
        //     for (var i = 0; i <= 15; i++)
        //     {
        //         entries[i] = ThumbnailPalette[i];
        //     }
        //     bmp.Palette = palette;

        //     var rect = new Rectangle(0, 0, 64, 48);
        //     var bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

        //     byte[] bytes = new byte[(32 * 48) + 1];
        //     var IPtr = bmpData.Scan0;
        //     Marshal.Copy(IPtr, bytes, 0, 32 * 48);

        //     int offset = 0;
        //     for (int ty = 0; ty <= 47; ty += 8)
        //     {
        //         for (int tx = 0; tx <= 31; tx += 4)
        //         {
        //             for (int l = 0; l <= 7; l++)
        //             {
        //                 int line = (ty + l) << 5;
        //                 for (int px = 0; px <= 3; px++)
        //                 {
        //                     // Need to reverse nibbles :
        //                     bytes[line + tx + px] = (byte)(((buffer[offset] & 0xF) << 4) | ((buffer[offset] & 0xF0) >> 4));
        //                     offset += 1;
        //                 }
        //             }
        //         }
        //     }

        //     Marshal.Copy(bytes, 0, IPtr, 32 * 48);
        //     bmp.UnlockBits(bmpData);
        //     return bmp;
        // }

        public static readonly Color[] FramePalette = 
        { 
            Color.FromRgba(0x00, 0x00, 0x00, 0xFF), 
            Color.FromRgba(0xFF, 0xFF, 0xFF, 0xFF), 
            Color.FromRgba(0xFF, 0x00, 0x00, 0xFF), //0xFFFF0000
            Color.FromRgba(0x00, 0x00, 0xFF, 0xFF) //0xFF0000FF
        };

        public static Image GetFrameBitmap(PPMFrame frame)
        {
            var bmp = new Image<Rgba32>(256, 192);

            for (var y = 0; y < 192; y++)
            {
                for (var x = 0; x < 256; x++)
                {
                    bmp[x, y] = frame.PaperColor.ToColor();

                    if (frame.Layer1[x, y])
                    {
                        bmp[x, y] = frame.Layer1.PenColor == PenColor.Inverted ? frame.PaperColor.ToInvertedColor() : frame.Layer1.PenColor.ToColor();
                        
                    }

                    if (frame.Layer2[x, y])
                        bmp[x, y] = frame.Layer2.PenColor == PenColor.Inverted ? frame.PaperColor.ToInvertedColor() : frame.Layer2.PenColor.ToColor();
                    
                    

                    // if (frame.Layer1[x, y])
                    // {
                    //     if (frame.Layer1.PenColor != PenColor.Inverted)
                    //     {
                    //         bmp[x, y] = frame.Layer1.PenColor == PenColor.Red ? Color.Red : Color.Blue;
                    //     }
                    //     else
                    //     {
                    //         bmp[x, y] = frame.PaperColor == PaperColor.Black ? Color.Black : Color.White;
                    //     }
                    // }
                    // else if (frame.Layer2[x, y])
                    // {
                    //     if (frame.Layer2.PenColor != PenColor.Inverted)
                    //     {
                    //         bmp[x, y] = frame.Layer2.PenColor == PenColor.Red ? Color.Red : Color.Blue;       
                    //     }
                    //     else
                    //     {
                    //         bmp[x, y] = frame.PaperColor == PaperColor.Black ? Color.Black : Color.White;
                    //     }
                    // }
                }
            }
            return bmp;
        }
    }

}