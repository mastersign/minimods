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
    /// <summary>
    /// This class is a collection of static helper methods to create
    /// <see cref="Bitmap"/> objects.
    /// </summary>
    public static class BitmapTools
    {
        public static Bitmap CreateFromArray(byte[,] m)
        {
            throw new NotImplementedException();
        }

        public static Bitmap CreateFromArray(byte[,] r, byte[,] g, byte[,] b)
        {
            throw new NotImplementedException();
        }

        public static Bitmap CreateFromArray(byte[,] a, byte[,] r, byte[,] g, byte[,] b)
        {
            throw new NotImplementedException();
        }

        public static Bitmap CreateFromRgbArray(byte[, ,] m)
        {
            throw new NotImplementedException();
        }

        public static Bitmap CreateFromArgbArray(byte[, ,] m)
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

        #region conversion

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

        public static void CopyToArray(this Bitmap bmp, byte[,] m)
        {
            throw new NotImplementedException();
        }

        public static void CopyToArray(this Bitmap bmp, byte[,,] m)
        {
            throw new NotImplementedException();
        }

        public static void CopyToArray(this Bitmap bmp, byte[,] r, byte[,] g, byte[,] b)
        {
            throw new NotImplementedException();
        }

        public static void CopyToArray(this Bitmap bmp, byte[,] a, byte[,] r, byte[,] g, byte[,] b)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region filter

        public static void ApplyFilter(this Bitmap bmp, Transfer f)
        {
            throw new NotImplementedException();
        }

        public static void ApplyFilter(this Bitmap bmp, TransferWithPosition f)
        {
            throw new NotImplementedException();
        }

        public static void ApplyFilter(this Bitmap bmp, Transfer fR, Transfer fG, Transfer fB)
        {
            throw new NotImplementedException();
        }

        public static void ApplyFilter(this Bitmap bmp, Transfer fA, Transfer fR, Transfer fG, Transfer fB)
        {
            throw new NotImplementedException();
        }

        public static void ApplyFilter(this Bitmap bmp, TransferWithPosition fR, TransferWithPosition fG, TransferWithPosition fB)
        {
            throw new NotImplementedException();
        }

        public static void ApplyFilter(this Bitmap bmp, TransferWithPosition fA, TransferWithPosition fR, TransferWithPosition fG, TransferWithPosition fB)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public delegate byte Transfer(byte value);

    public delegate byte TransferWithPosition(int x, int y, byte value);
}
