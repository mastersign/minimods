/*
 * MiniMod "Bitmap Tools"
 * 
 * Author: Tobias Kiertscher <dev@mastersign.de>
 * 
 * Description:
 *   Bitmap tools provides conversion and access methods for
 *   reading and writing data in System.Drawing.Bitmap objects.
 * 
 * Version: 1.0
 * Last Change: 29.01.2013
 * 
 * URL: https://raw.github.com/mastersign/minimods/master/de.mastersign.minimods.bitmaptools.cs
 * 
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;

namespace de.mastersign.minimods.bitmaptools
{
    public static class BitmapTools
    {
        public static Bitmap CreateFromArray(byte[,] m)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// This static class contains a group of extension methods for the
    /// <see cref="Bitmap"/> class.
    /// </summary>
    public static class BitmapExtension
    {
        static BitmapExtension()
        {
            InitializeGrayScalePalette();
        }

        #region gray scale palette

        private static ColorPalette paletteGrayScale;

        private static void InitializeGrayScalePalette()
        {
            var bmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            paletteGrayScale = bmp.Palette;
            bmp.Dispose();
            for (var i = 0; i < 256; i++)
            {
                paletteGrayScale.Entries[i] = Color.FromArgb(255, i, i, i);
            }
        }

        #endregion

        public static byte[,] ToGrayScaleArray(this Bitmap bmp)
        {
            throw new NotImplementedException();
        }

        public static byte[,,] ToRgbArray(this Bitmap bmp)
        {
            throw new NotImplementedException();
        }

        public static byte[,,] ToArgbArray(this Bitmap bmp)
        {
            throw new NotImplementedException();
        }
    }
}
