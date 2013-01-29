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
using System.Runtime.InteropServices;
using System.Text;

namespace de.mastersign.minimods.bitmaptools
{
    /// <summary>
    /// This class is a collection of static helper methods to create
    /// <see cref="Bitmap"/> objects.
    /// </summary>
    public static class BitmapTools
    {
        private const int MAX_SIZE = 32 * 1024;

        private static void CheckSize(int width, int height)
        {
            if (width < 1)
            {
                throw new ArgumentException("The width of the bitmap must be at least 1 pixel.", "width");
            }
            if (width > MAX_SIZE)
            {
                throw new ArgumentException(string.Format("The width of the bitmap can not be larger then {0} pixels.", MAX_SIZE), "width");
            }
            if (height < 1)
            {
                throw new ArgumentException("The height of the bitmap must be at least 1 pixel.", "height");
            }
            if (height > MAX_SIZE)
            {
                throw new ArgumentException(string.Format("The height of the bitmap can not be larger then {0} pixels.", MAX_SIZE), "height");
            }
        }

        /// <summary>
        /// Creates a new gray scale <see cref="Bitmap"/> with a bit depth of 8Bits.
        /// </summary>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>The created <see cref="Bitmap"/>.</returns>
        public static Bitmap CreateGrayScaleBitmap(int width, int height)
        {
            CheckSize(width, height);
            var bmp = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
            bmp.SetPaletteToGrayScale();
            return bmp;
        }

        /// <summary>
        /// Creates a new 24Bit RGB <see cref="Bitmap"/> with a bit depth of 8Bits per channel.
        /// </summary>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>The created <see cref="Bitmap"/>.</returns>
        public static Bitmap CreateRgbBitmap(int width, int height)
        {
            CheckSize(width, height);
            return new Bitmap(width, height, PixelFormat.Format24bppRgb);
        }

        /// <summary>
        /// Creates a new 32Bit ARGB <see cref="Bitmap"/> with a bit depth of 8Bits per channel.
        /// </summary>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>The created <see cref="Bitmap"/>.</returns>
        public static Bitmap CreateArgbBitmap(int width, int height)
        {
            CheckSize(width, height);
            return new Bitmap(width, height, PixelFormat.Format32bppArgb);
        }

        /// <summary>
        /// Creates a new gray scale <see cref="Bitmap"/> with a bit depth of 8Bits.
        /// The data is copied from the given <see cref="Byte"/> array.
        /// The first index of the array is the row index (Y) 
        /// and the second index is the column index (X).
        /// Every element of the array is interpreted as an intensity value.
        /// A value of <c>0</c> means black and a value of <c>255</c> means white.
        /// </summary>
        /// <param name="m">The array with the intensity data.</param>
        /// <returns>The created <see cref="Bitmap"/>.</returns>
        public static Bitmap CreateFromArray(byte[,] m)
        {
            if (m == null) throw new ArgumentNullException("m");
            var rows = m.GetLength(0);
            var cols = m.GetLength(1);
            CheckSize(cols, rows);
            var bmp = CreateGrayScaleBitmap(cols, rows);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
            var line = bmpData.Scan0;
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    Marshal.WriteByte(line, x, m[y, x]);
                }
                line += bmpData.Stride;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// Creates a new 24Bit RGB <see cref="Bitmap"/> with a bit depth of 8Bits per channel.
        /// The data is copied from the given <see cref="Byte"/> arrays.
        /// The first index of an array is the row index (Y) 
        /// and the second index is the column index (X).
        /// Every element of an array is interpreted as an intensity value.
        /// A value of <c>0</c> means minimum intensity and a value of <c>255</c> means maximum intensity.
        /// </summary>
        /// <param name="r">The array with the intensity data for the red channel.</param>
        /// <param name="g">The array with the intensity data for the green channel.</param>
        /// <param name="b">The array with the intensity data for the blue channel.</param>
        /// <returns>The created <see cref="Bitmap"/>.</returns>
        public static Bitmap CreateFromArray(byte[,] r, byte[,] g, byte[,] b)
        {
            if (r == null) throw new ArgumentNullException("r");
            if (g == null) throw new ArgumentNullException("g");
            if (b == null) throw new ArgumentNullException("b");
            var rows = Math.Min(r.GetLength(0), Math.Min(g.GetLength(0), b.GetLength(0)));
            var cols = Math.Min(r.GetLength(1), Math.Min(g.GetLength(1), b.GetLength(1)));
            CheckSize(cols, rows);
            var bmp = CreateRgbBitmap(cols, rows);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            var line = bmpData.Scan0;
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    var ofs = x * 3;
                    Marshal.WriteByte(line, ofs + 0, r[y, x]);
                    Marshal.WriteByte(line, ofs + 1, g[y, x]);
                    Marshal.WriteByte(line, ofs + 2, b[y, x]);
                }
                line += bmpData.Stride;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// Creates a new 32Bit ARGB <see cref="Bitmap"/> with a bit depth of 8Bits per channel.
        /// The data is copied from the given <see cref="Byte"/> arrays.
        /// The first index of an array is the row index (Y) 
        /// and the second index is the column index (X).
        /// Every element of an array is interpreted as an intensity value.
        /// A value of <c>0</c> means minimum intensity and a value of <c>255</c> means maximum intensity.
        /// For the alpha channel a value of <c>0</c> means transparent and a value of <c>255</c> means opaque.
        /// </summary>
        /// <param name="a">The array with the transparency data for the alpha channel.</param>
        /// <param name="r">The array with the intensity data for the red channel.</param>
        /// <param name="g">The array with the intensity data for the green channel.</param>
        /// <param name="b">The array with the intensity data for the blue channel.</param>
        /// <returns>The created <see cref="Bitmap"/>.</returns>
        public static Bitmap CreateFromArray(byte[,] a, byte[,] r, byte[,] g, byte[,] b)
        {
            if (a == null) throw new ArgumentNullException("a");
            if (r == null) throw new ArgumentNullException("r");
            if (g == null) throw new ArgumentNullException("g");
            if (b == null) throw new ArgumentNullException("b");
            var rows = Math.Min(a.GetLength(0), Math.Min(r.GetLength(0), Math.Min(g.GetLength(0), b.GetLength(0))));
            var cols = Math.Min(a.GetLength(1), Math.Min(r.GetLength(1), Math.Min(g.GetLength(1), b.GetLength(1))));
            CheckSize(cols, rows);
            var bmp = CreateArgbBitmap(cols, rows);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            var line = bmpData.Scan0;
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    var ofs = x * 4;
                    Marshal.WriteByte(line, ofs + 0, a[y, x]);
                    Marshal.WriteByte(line, ofs + 1, r[y, x]);
                    Marshal.WriteByte(line, ofs + 2, g[y, x]);
                    Marshal.WriteByte(line, ofs + 3, b[y, x]);
                }
                line += bmpData.Stride;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// Creates a new 24Bit RGB <see cref="Bitmap"/> with a bit depth of 8Bits per channel.
        /// The data is copied from the given <see cref="Byte"/> arrays.
        /// The first index of an array is the row index (Y),
        /// the second index is the column index (X) 
        /// and the third index is the channel (r, g, b).
        /// Every element of an array is interpreted as an intensity value.
        /// A value of <c>0</c> means minimum intensity and a value of <c>255</c> means maximum intensity.
        /// </summary>
        /// <param name="m">The array with the image data.</param>
        /// <returns>The created <see cref="Bitmap"/>.</returns>
        public static Bitmap CreateFromRgbArray(byte[, ,] m)
        {
            if (m == null) throw new ArgumentNullException("m");
            var rows = m.GetLength(0);
            var cols = m.GetLength(1);
            CheckSize(cols, rows);
            var bmp = CreateRgbBitmap(cols, rows);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            var line = bmpData.Scan0;
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    var ofs = x * 3;
                    Marshal.WriteByte(line, ofs + 0, m[y, x, 0]);
                    Marshal.WriteByte(line, ofs + 1, m[y, x, 1]);
                    Marshal.WriteByte(line, ofs + 2, m[y, x, 2]);
                }
                line += bmpData.Stride;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        /// <summary>
        /// Creates a new 32Bit ARGB <see cref="Bitmap"/> with a bit depth of 8Bits per channel.
        /// The data is copied from the given <see cref="Byte"/> arrays.
        /// The first index of an array is the row index (Y),
        /// the second index is the column index (X) 
        /// and the third index is the channel (a, r, g, b).
        /// Every element of an array is interpreted as an intensity value.
        /// A value of <c>0</c> means minimum intensity and a value of <c>255</c> means maximum intensity.
        /// For the alpha channel a value of <c>0</c> means transparent and a value of <c>255</c> means opaque.
        /// </summary>
        /// <param name="m">The array with the image data.</param>
        /// <returns>The created <see cref="Bitmap"/>.</returns>
        public static Bitmap CreateFromArgbArray(byte[, ,] m)
        {
            if (m == null) throw new ArgumentNullException("m");
            var rows = m.GetLength(0);
            var cols = m.GetLength(1);
            CheckSize(cols, rows);
            var bmp = CreateRgbBitmap(cols, rows);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            var line = bmpData.Scan0;
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    var ofs = x * 4;
                    Marshal.WriteByte(line, ofs + 0, m[y, x, 0]);
                    Marshal.WriteByte(line, ofs + 1, m[y, x, 1]);
                    Marshal.WriteByte(line, ofs + 2, m[y, x, 2]);
                    Marshal.WriteByte(line, ofs + 3, m[y, x, 32]);
                }
                line += bmpData.Stride;
            }
            bmp.UnlockBits(bmpData);
            return bmp;
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

        public static void SetPaletteToGrayScale(this Bitmap bmp)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region conversion

        public static byte[,] ToGrayScaleArray(this Bitmap bmp)
        {
            throw new NotImplementedException();
        }

        public static byte[, ,] ToRgbArray(this Bitmap bmp)
        {
            throw new NotImplementedException();
        }

        public static byte[, ,] ToArgbArray(this Bitmap bmp)
        {
            throw new NotImplementedException();
        }

        public static void CopyToArray(this Bitmap bmp, byte[,] m)
        {
            throw new NotImplementedException();
        }

        public static void CopyToArray(this Bitmap bmp, byte[, ,] m)
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

        public static void CopyFromArray(this Bitmap bmp, byte[,] m)
        {
            throw new NotImplementedException();
        }

        public static void CopyFromArray(this Bitmap bmp, byte[, ,] m)
        {
            throw new NotImplementedException();
        }

        public static void CopyFromArray(this Bitmap bmp, byte[,] r, byte[,] g, byte[,] b)
        {
            throw new NotImplementedException();
        }

        public static void CopyFromArray(this Bitmap bmp, byte[,] a, byte[,] r, byte[,] g, byte[,] b)
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

    /// <summary>
    /// A delegate for an intensity transfer function.
    /// </summary>
    /// <param name="value">The original intensity value.</param>
    /// <returns>The result intensity value.</returns>
    public delegate byte Transfer(byte value);

    /// <summary>
    /// A delegate for an intensity transfer function regarding the pixel position.
    /// </summary>
    /// <param name="x">The position of the pixel on the X axis.</param>
    /// <param name="y">The position of the pixel on the Y axis.</param>
    /// <param name="value">The original intensity value.</param>
    /// <returns>The resulting intensity value.</returns>
    public delegate byte TransferWithPosition(int x, int y, byte value);
}
