#region MiniMod
// <MiniMod>
//   <Name>Bitmap Tools</Name>
//   <Author>Tobias Kiertscher &lt;dev@mastersign.de&gt;</Author>
//   <LastChanged>2015-01-21</LastChanged>
//   <Version>1.1.0</Version>
//   <Url>https://gist.github.com/mastersign/68bc1c14d77b86e6a4ad/raw/Mastersign.Minimods.BitmapTools.cs</Url>
//   <Description>
//     Bitmap tools provides conversion and access methods for
//    reading and writing data in System.Drawing.Bitmap objects
//   </Description>
// </MiniMod>
#endregion

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Mastersign.Minimods.BitmapTools
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
                throw new ArgumentOutOfRangeException("The width of the bitmap must be at least 1 pixel.", "width");
            }
            if (width > MAX_SIZE)
            {
                throw new ArgumentOutOfRangeException(string.Format("The width of the bitmap can not be larger then {0} pixels.", MAX_SIZE), "width");
            }
            if (height < 1)
            {
                throw new ArgumentOutOfRangeException("The height of the bitmap must be at least 1 pixel.", "height");
            }
            if (height > MAX_SIZE)
            {
                throw new ArgumentOutOfRangeException(string.Format("The height of the bitmap can not be larger then {0} pixels.", MAX_SIZE), "height");
            }
        }

        /// <summary>
        /// Creates a new gray scale <see cref="Bitmap"/> with a bit depth of 8Bits.
        /// </summary>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>The created <see cref="Bitmap"/>.</returns>
        /// <exception cref="ArgumentException">Is thrown,
        /// if an invalid size for the bitmap is given.</exception>
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
        /// <exception cref="ArgumentException">Is thrown,
        /// if an invalid size for the bitmap is given.</exception>
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
        /// <exception cref="ArgumentException">Is thrown,
        /// if an invalid size for the bitmap is given.</exception>
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
        /// <exception cref="ArgumentNullException">Is thrown, 
        /// if <c>null</c> is given for <paramref name="m"/>.</exception>
        public static Bitmap CreateFromArray(byte[,] m)
        {
            if (m == null) throw new ArgumentNullException("m");
            var rows = m.GetLength(0);
            var cols = m.GetLength(1);
            CheckSize(cols, rows);
            var bmp = CreateGrayScaleBitmap(cols, rows);
            bmp.CopyFromArray(m);
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
        /// <exception cref="ArgumentNullException">Is thrown, 
        /// if <c>null</c> is given for <paramref name="r"/>,
        /// <paramref name="g"/>, or <paramref name="b"/>.</exception>
        public static Bitmap CreateFromArray(byte[,] r, byte[,] g, byte[,] b)
        {
            if (r == null) throw new ArgumentNullException("r");
            if (g == null) throw new ArgumentNullException("g");
            if (b == null) throw new ArgumentNullException("b");
            var rows = r.GetLength(0);
            var cols = r.GetLength(1);
            CheckSize(cols, rows);
            var bmp = CreateRgbBitmap(cols, rows);
            bmp.CopyFromArray(r, g, b);
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
        /// <exception cref="ArgumentNullException">Is thrown, 
        /// if <c>null</c> is given for <paramref name="a"/>, <paramref name="r"/>,
        /// <paramref name="g"/>, or <paramref name="b"/>.</exception>
        public static Bitmap CreateFromArray(byte[,] a, byte[,] r, byte[,] g, byte[,] b)
        {
            if (a == null) throw new ArgumentNullException("a");
            if (r == null) throw new ArgumentNullException("r");
            if (g == null) throw new ArgumentNullException("g");
            if (b == null) throw new ArgumentNullException("b");
            var rows = a.GetLength(0);
            var cols = a.GetLength(1);
            CheckSize(cols, rows);
            var bmp = CreateArgbBitmap(cols, rows);
            bmp.CopyFromArray(a, r, g, b);
            return bmp;
        }

        /// <summary>
        /// Creates a new <see cref="Bitmap"/> with a bit depth of 8Bits per channel.
        /// The data is copied from the given <see cref="Byte"/> array.
        /// The pixel format of the result bitmap depends on the size of third dimension
        /// of <paramref name="m"/> (1: 8Bit indexed, 3: 24Bit RGB, 4: 32Bit ARGB).
        /// The first index of an array is the row index (Y),
        /// the second index is the column index (X) 
        /// and the third index is the channel (gray), (r, g, b) or (a, r, g, b).
        /// Every element of an array is interpreted as an intensity value.
        /// A value of <c>0</c> means minimum intensity and a value of <c>255</c> means maximum intensity.
        /// </summary>
        /// <param name="m">The array with the image data.</param>
        /// <returns>The created <see cref="Bitmap"/>.</returns>
        /// <exception cref="ArgumentNullException">Is thrown, 
        /// if <c>null</c> is given for <paramref name="m"/>.</exception>
        public static Bitmap CreateFromArray(byte[, ,] m)
        {
            if (m == null) throw new ArgumentNullException("m");
            var rows = m.GetLength(0);
            var cols = m.GetLength(1);
            CheckSize(cols, rows);
            var channels = m.GetLength(2);
            Bitmap bmp;
            switch (channels)
            {
                case 1:
                    bmp = CreateGrayScaleBitmap(cols, rows);
                    break;
                case 3:
                    bmp = CreateRgbBitmap(cols, rows);
                    break;
                case 4:
                    bmp = CreateArgbBitmap(cols, rows);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("The size of the array in the third dimension is neither 1, nor 3 or 4 (Gray Scale, RGB, ARGB).", "m");
            }
            bmp.CopyFromArray(m);
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

        /// <summary>
        /// Sets the palette of a 8Bit indexed bitmap to a linear gray gradient.
        /// </summary>
        /// <param name="bmp">The bitmap.</param>
        /// <exception cref="ArgumentNullException">
        /// Is thrown if <c>null</c> is given for <paramref name="bmp"/>.</exception>
        /// <exception cref="ArgumentException">
        /// Is thrown if the given bitmap has no 8Bit indexed pixel format.</exception>
        public static void SetPaletteToGrayScale(this Bitmap bmp)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (bmp.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new ArgumentException("The given bitmap is no 8Bit indexed bitmap.", "bmp");
            }
            bmp.Palette = paletteGrayScale;
        }

        #endregion

        #region conversion

        /// <summary>
        /// Reads the image data from the <see cref="Bitmap"/> and copies it into a
        /// new two-dimensional array.
        /// </summary>
        /// <param name="bmp">The <see cref="Bitmap"/>.</param>
        /// <returns>The created array.</returns>
        /// <exception cref="ArgumentNullException">
        /// Is thrown if <c>null</c> is given for <paramref name="bmp"/>.</exception>
        /// <exception cref="ArgumentException">
        /// Is thrown if the bitmap has no 8Bit indexed pixel format.</exception>
        public static byte[,] ToGrayScaleArray(this Bitmap bmp)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (bmp.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new ArgumentException("The given bitmap is no 8Bit indexed bitmap.", "bmp");
            }
            var rows = bmp.Height;
            var cols = bmp.Width;
            var m = new byte[rows, cols];
            InternalCopyToArray(bmp, m, rows, cols);
            return m;
        }

        /// <summary>
        /// Reads the image data from the <see cref="Bitmap"/> and copies it into a
        /// new three-dimensional array.
        /// </summary>
        /// <param name="bmp">The <see cref="Bitmap"/>.</param>
        /// <returns>The created array.</returns>
        /// <exception cref="ArgumentNullException">
        /// Is thrown if <c>null</c> is given for <paramref name="bmp"/>.</exception>
        /// <exception cref="ArgumentException">
        /// Is thrown if the bitmap has no 24Bit RGB pixel format.</exception>
        public static byte[, ,] ToRgbArray(this Bitmap bmp)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (bmp.PixelFormat != PixelFormat.Format24bppRgb)
            {
                throw new ArgumentException("The given bitmap is no 24Bit RGB bitmap.", "bmp");
            }
            var rows = bmp.Height;
            var cols = bmp.Width;
            var m = new byte[rows, cols, 3];
            bmp.CopyToArray(m);
            return m;
        }

        /// <summary>
        /// Reads the image data from the <see cref="Bitmap"/> and copies it into a
        /// new three-dimensional array.
        /// </summary>
        /// <param name="bmp">The <see cref="Bitmap"/>.</param>
        /// <returns>The created array.</returns>
        /// <exception cref="ArgumentNullException">
        /// Is thrown if <c>null</c> is given for <paramref name="bmp"/>.</exception>
        /// <exception cref="ArgumentException">
        /// Is thrown if the bitmap has no 32Bit ARGB pixel format.</exception>
        public static byte[, ,] ToArgbArray(this Bitmap bmp)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (bmp.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("The given bitmap is no 32Bit ARGB bitmap.", "bmp");
            }
            var rows = bmp.Height;
            var cols = bmp.Width;
            var m = new byte[rows, cols, 4];
            bmp.CopyToArray(m);
            return m;
        }

        /// <summary>
        /// Reads the image data from the <see cref="Bitmap"/> and copies it into the
        /// given two-dimensional array.
        /// </summary>
        /// <param name="bmp">The <see cref="Bitmap"/>.</param>
        /// <param name="m">The array for the intensity data.</param>
        /// <returns>The created array.</returns>
        /// <exception cref="ArgumentNullException">
        /// Is thrown if <c>null</c> is given for <paramref name="bmp"/>.</exception>
        /// <exception cref="ArgumentException">
        /// Is thrown if the given bitmap has no 8Bit indexed pixel format or the
        /// the bitmap and the array do not match in size.</exception>
        public static void CopyToArray(this Bitmap bmp, byte[,] m)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (bmp.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new ArgumentException("The given bitmap is no 8Bit indexed bitmap.", "bmp");
            }
            if (m == null) throw new ArgumentNullException("m");
            var rows = bmp.Height;
            var cols = bmp.Width;
            if (rows != m.GetLength(0) || cols != m.GetLength(1))
            {
                throw new ArgumentException("The given bitmap and array do not match in size.");
            }
            InternalCopyToArray(bmp, m, rows, cols);
        }

        private static void InternalCopyToArray(Bitmap bmp, byte[,] m, int rows, int cols)
        {
            var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
            var line = bmpData.Scan0;
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    m[y, x] = Marshal.ReadByte(line, x);
                }
                line += bmpData.Stride;
            }
        }

        /// <summary>
        /// Reads the image data from the <see cref="Bitmap"/> and copies it into the
        /// given two-dimensional arrays.
        /// </summary>
        /// <param name="bmp">The <see cref="Bitmap"/>.</param>
        /// <param name="r">The array for the intensity data of the red channel.</param>
        /// <param name="g">The array for the intensity data of the green channel.</param>
        /// <param name="b">The array for the intensity data of the blue channel.</param>
        /// <returns>The created array.</returns>
        /// <exception cref="ArgumentNullException">
        /// Is thrown if <c>null</c> is given for <paramref name="bmp"/>.</exception>
        /// <exception cref="ArgumentException">
        /// Is thrown if the given bitmap has no 24Bit RGB pixel format or the
        /// the bitmap and the arrays do not match in size.</exception>
        public static void CopyToArray(this Bitmap bmp, byte[,] r, byte[,] g, byte[,] b)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (bmp.PixelFormat != PixelFormat.Format24bppRgb)
            {
                throw new ArgumentException("The given bitmap has no 24Bit RGB pixel format.", "bmp");
            }
            if (r == null) throw new ArgumentNullException("r");
            if (g == null) throw new ArgumentNullException("g");
            if (b == null) throw new ArgumentNullException("b");
            var rows = r.GetLength(0);
            var cols = r.GetLength(1);
            if (rows != bmp.Height || cols != bmp.Width ||
                rows != g.GetLength(0) || cols != g.GetLength(1) ||
                rows != b.GetLength(0) || cols != b.GetLength(1))
            {
                throw new ArgumentException("The given bitmap and arrays do not match in size.");
            }
            InternalCopyToArray(bmp, r, g, b, rows, cols);
        }

        private static void InternalCopyToArray(Bitmap bmp, byte[,] r, byte[,] g, byte[,] b, int rows, int cols)
        {
            var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            var line = bmpData.Scan0;
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    var ofs = x * 3;
                    b[y, x] = Marshal.ReadByte(line, ofs + 0);
                    g[y, x] = Marshal.ReadByte(line, ofs + 1);
                    r[y, x] = Marshal.ReadByte(line, ofs + 2);
                }
                line += bmpData.Stride;
            }
        }

        /// <summary>
        /// Reads the image data from the <see cref="Bitmap"/> and copies it into the
        /// given two-dimensional arrays.
        /// </summary>
        /// <param name="bmp">The <see cref="Bitmap"/>.</param>
        /// <param name="a">The array for the transparency data of the alpha channel.</param>
        /// <param name="r">The array for the intensity data of the red channel.</param>
        /// <param name="g">The array for the intensity data of the green channel.</param>
        /// <param name="b">The array for the intensity data of the blue channel.</param>
        /// <returns>The created array.</returns>
        /// <exception cref="ArgumentNullException">
        /// Is thrown if <c>null</c> is given for <paramref name="bmp"/>.</exception>
        /// <exception cref="ArgumentException">
        /// Is thrown if the given bitmap has no 32Bit ARGB pixel format or the
        /// the bitmap and the arrays do not match in size.</exception>
        public static void CopyToArray(this Bitmap bmp, byte[,] a, byte[,] r, byte[,] g, byte[,] b)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (bmp.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("The given bitmap has no 32Bit ARGB pixel format.", "bmp");
            }
            if (a == null) throw new ArgumentNullException("a");
            if (r == null) throw new ArgumentNullException("r");
            if (g == null) throw new ArgumentNullException("g");
            if (b == null) throw new ArgumentNullException("b");
            var rows = a.GetLength(0);
            var cols = a.GetLength(1);
            if (rows != bmp.Height || cols != bmp.Width ||
                rows != r.GetLength(0) || cols != r.GetLength(1) ||
                rows != g.GetLength(0) || cols != g.GetLength(1) ||
                rows != b.GetLength(0) || cols != b.GetLength(1))
            {
                throw new ArgumentException("The given bitmap and arrays do not match in size.");
            }
            InternalCopyToArray(bmp, a, r, g, b, rows, cols);
        }

        private static void InternalCopyToArray(Bitmap bmp, byte[,] a, byte[,] r, byte[,] g, byte[,] b, int rows, int cols)
        {
            var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var line = bmpData.Scan0;
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    var ofs = x * 4;
                    b[y, x] = Marshal.ReadByte(line, ofs + 0);
                    g[y, x] = Marshal.ReadByte(line, ofs + 1);
                    r[y, x] = Marshal.ReadByte(line, ofs + 2);
                    a[y, x] = Marshal.ReadByte(line, ofs + 3);
                }
                line += bmpData.Stride;
            }
        }

        /// <summary>
        /// Reads the image data from the <see cref="Bitmap"/> and copies it into the
        /// given three-dimensional arrays. 
        /// If the given bitmap is a 8Bit indexed bitmap,
        /// the array <paramref name="m"/> must have a size of 1 in the third dimension.
        /// If the given bitmap is a 24Bit RGB bitmap,
        /// the array <paramref name="m"/> must have a size of 3 in the third dimension.
        /// If the given bitmap is a 32Bit ARGB bitmap,
        /// the array <paramref name="m"/> must have a size of 4 in the third dimension.
        /// </summary>
        /// <param name="bmp">The <see cref="Bitmap"/>.</param>
        /// <param name="m">The array for the image data of the bitmap.</param>
        /// <returns>The created array.</returns>
        /// <exception cref="ArgumentNullException">
        /// Is thrown if <c>null</c> is given for <paramref name="bmp"/>.</exception>
        /// <exception cref="ArgumentException">
        /// Is thrown if the given bitmap has no 32Bit ARGB pixel format or the
        /// the bitmap and the arrays do not match in size.</exception>
        public static void CopyToArray(this Bitmap bmp, byte[, ,] m)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (m == null) throw new ArgumentNullException("m");
            var rows = bmp.Height;
            var cols = bmp.Width;
            if (rows != m.GetLength(0) || cols != m.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("The given bitmap and array do not match in size.");
            }
            if (bmp.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                if (m.GetLength(2) != 1)
                {
                    throw new ArgumentOutOfRangeException("The third dimension of the given array is unequal one (for the one color channel).", "m");
                }
                var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                    ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
                var line = bmpData.Scan0;
                for (var y = 0; y < rows; y++)
                {
                    for (var x = 0; x < cols; x++)
                    {
                        m[y, x, 0] = Marshal.ReadByte(line, x);
                    }
                    line += bmpData.Stride;
                }
            }
            else if (bmp.PixelFormat == PixelFormat.Format24bppRgb)
            {
                if (m.GetLength(2) != 3)
                {
                    throw new ArgumentOutOfRangeException("The third dimension of the given array is unequal three (for the three color channels).", "m");
                }
                var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                    ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                var line = bmpData.Scan0;
                for (var y = 0; y < rows; y++)
                {
                    for (var x = 0; x < cols; x++)
                    {
                        var ofs = x * 3;
                        m[y, x, 2] = Marshal.ReadByte(line, ofs + 0);
                        m[y, x, 1] = Marshal.ReadByte(line, ofs + 1);
                        m[y, x, 0] = Marshal.ReadByte(line, ofs + 2);
                    }
                    line += bmpData.Stride;
                }
            }
            else if (bmp.PixelFormat == PixelFormat.Format32bppArgb)
            {
                if (m.GetLength(2) != 4)
                {
                    throw new ArgumentOutOfRangeException("The third dimension of the given array is unequal four (for the four color channels).", "m");
                }
                var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                var line = bmpData.Scan0;
                for (var y = 0; y < rows; y++)
                {
                    for (var x = 0; x < cols; x++)
                    {
                        var ofs = x * 4;
                        m[y, x, 3] = Marshal.ReadByte(line, ofs + 0);
                        m[y, x, 2] = Marshal.ReadByte(line, ofs + 1);
                        m[y, x, 1] = Marshal.ReadByte(line, ofs + 2);
                        m[y, x, 0] = Marshal.ReadByte(line, ofs + 3);
                    }
                    line += bmpData.Stride;
                }
            }
            else
            {
                throw new ArgumentException("The pixel format of the given bitmap is not supported.", "bmp");
            }
        }

        /// <summary>
        /// Reads the image data from the given array and writes it into the bitmap.
        /// </summary>
        /// <param name="bmp">The bitmap.</param>
        /// <param name="m">The array with the image data.</param>
        public static void CopyFromArray(this Bitmap bmp, byte[,] m)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (bmp.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new ArgumentException("The given bitmap has no 8Bit indexed pixel format.", "bmp");
            }
            if (m == null) throw new ArgumentNullException("m");
            var rows = bmp.Height;
            var cols = bmp.Width;
            if (rows != m.GetLength(0) || cols != m.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("The given bitmap and array do not match in size.");
            }
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
        }

        /// <summary>
        /// Reads the image data from the given arrays and writes it into the bitmap.
        /// </summary>
        /// <param name="bmp">The bitmap.</param>
        /// <param name="r">The array with the intensity data of the red channel.</param>
        /// <param name="g">The array with the intensity data of the green channel.</param>
        /// <param name="b">The array with the intensity data of the blue channel.</param>
        public static void CopyFromArray(this Bitmap bmp, byte[,] r, byte[,] g, byte[,] b)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (bmp.PixelFormat != PixelFormat.Format24bppRgb)
            {
                throw new ArgumentException("The given bitmap has no 24Bit RGB pixel format.", "bmp");
            }
            if (r == null) throw new ArgumentNullException("r");
            if (g == null) throw new ArgumentNullException("g");
            if (b == null) throw new ArgumentNullException("b");
            var rows = r.GetLength(0);
            var cols = r.GetLength(1);
            if (rows != bmp.Height || cols != bmp.Width ||
                rows != g.GetLength(0) || cols != g.GetLength(1) ||
                rows != b.GetLength(0) || cols != b.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("The given bitmap and arrays do not match in size.");
            }
            var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            var line = bmpData.Scan0;
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    var ofs = x * 3;
                    Marshal.WriteByte(line, ofs + 0, b[y, x]);
                    Marshal.WriteByte(line, ofs + 1, g[y, x]);
                    Marshal.WriteByte(line, ofs + 2, r[y, x]);
                }
                line += bmpData.Stride;
            }
            bmp.UnlockBits(bmpData);
        }

        /// <summary>
        /// Reads the image data from the given arrays and writes it into the bitmap.
        /// </summary>
        /// <param name="bmp">The bitmap.</param>
        /// <param name="a">The array with the transparency data of the alpha channel.</param>
        /// <param name="r">The array with the intensity data of the red channel.</param>
        /// <param name="g">The array with the intensity data of the green channel.</param>
        /// <param name="b">The array with the intensity data of the blue channel.</param>
        public static void CopyFromArray(this Bitmap bmp, byte[,] a, byte[,] r, byte[,] g, byte[,] b)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (bmp.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("The given bitmap has no 32Bit ARGB pixel format.", "bmp");
            }
            if (a == null) throw new ArgumentNullException("a");
            if (r == null) throw new ArgumentNullException("r");
            if (g == null) throw new ArgumentNullException("g");
            if (b == null) throw new ArgumentNullException("b");
            var rows = a.GetLength(0);
            var cols = a.GetLength(1);
            if (rows != bmp.Height || cols != bmp.Width ||
                rows != r.GetLength(0) || cols != r.GetLength(1) ||
                rows != g.GetLength(0) || cols != g.GetLength(1) ||
                rows != b.GetLength(0) || cols != b.GetLength(1))
            {
                throw new ArgumentOutOfRangeException("The given bitmap and arrays do not match in size.");
            }
            var bmpData = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            var line = bmpData.Scan0;
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < cols; x++)
                {
                    var ofs = x * 4;
                    Marshal.WriteByte(line, ofs + 0, b[y, x]);
                    Marshal.WriteByte(line, ofs + 1, g[y, x]);
                    Marshal.WriteByte(line, ofs + 2, r[y, x]);
                    Marshal.WriteByte(line, ofs + 3, a[y, x]);
                }
                line += bmpData.Stride;
            }
            bmp.UnlockBits(bmpData);
        }

        /// <summary>
        /// Reads the image data from the given array and writes it into the bitmap.
        /// </summary>
        /// <param name="bmp">The bitmap.</param>
        /// <param name="m">The array with the image data.</param>
        public static void CopyFromArray(this Bitmap bmp, byte[, ,] m)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            var rows = m.GetLength(0);
            var cols = m.GetLength(1);
            var channels = m.GetLength(2);
            switch (channels)
            {
                case 1:
                    var bmpData1 = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                        ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                    var line1 = bmpData1.Scan0;
                    for (var y = 0; y < rows; y++)
                    {
                        for (var x = 0; x < cols; x++)
                        {
                            Marshal.WriteByte(line1, x, m[y, x, 0]);
                        }
                        line1 += bmpData1.Stride;
                    }
                    bmp.UnlockBits(bmpData1);
                    break;
                case 3:
                    var bmpData3 = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                        ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                    var line3 = bmpData3.Scan0;
                    for (var y = 0; y < rows; y++)
                    {
                        for (var x = 0; x < cols; x++)
                        {
                            var ofs = x * 3;
                            Marshal.WriteByte(line3, ofs + 0, m[y, x, 2]);
                            Marshal.WriteByte(line3, ofs + 1, m[y, x, 1]);
                            Marshal.WriteByte(line3, ofs + 2, m[y, x, 0]);
                        }
                        line3 += bmpData3.Stride;
                    }
                    bmp.UnlockBits(bmpData3);
                    break;
                case 4:
                    var bmpData4 = bmp.LockBits(new Rectangle(0, 0, cols, rows),
                           ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    var line4 = bmpData4.Scan0;
                    for (var y = 0; y < rows; y++)
                    {
                        for (var x = 0; x < cols; x++)
                        {
                            var ofs = x * 4;
                            Marshal.WriteByte(line4, ofs + 0, m[y, x, 3]);
                            Marshal.WriteByte(line4, ofs + 1, m[y, x, 2]);
                            Marshal.WriteByte(line4, ofs + 2, m[y, x, 1]);
                            Marshal.WriteByte(line4, ofs + 3, m[y, x, 0]);
                        }
                        line4 += bmpData4.Stride;
                    }
                    bmp.UnlockBits(bmpData4);
                    break;
                default:
                    throw new ArgumentException("The size of the array in the third dimension is neither 1, nor 3 or 4 (Gray Scale, RGB, ARGB).", "m");
            }
        }

        #endregion

        #region filling

        /// <summary>
        /// Fills the image with intensity values, created by calling <paramref name="f"/>.
        /// For a color image <paramref name="f"/> is only called once for each pixel
        /// and the returned intensity value is written to all channels.
        /// If the image has an alpha channel, it remains unchanged.
        /// </summary>
        /// <param name="bmp">The image to fill.</param>
        /// <param name="f">The intensity source function.</param>
        /// <exception cref="ArgumentNullException">Is thrown,
        /// if <c>null</c> is given for <paramref name="bmp"/>, or <paramref name="f"/>.</exception>
        /// <exception cref="ArgumentException">Is thrown,
        /// if the pixel format of <paramref name="bmp"/> is not one of the following:
        /// <see cref="PixelFormat.Format8bppIndexed"/>, <see cref="PixelFormat.Format24bppRgb"/>, 
        /// or <see cref="PixelFormat.Format32bppArgb"/>.
        /// </exception>
        public static void FillWith(this Bitmap bmp, IntensitySource f)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (f == null) throw new ArgumentNullException("f");
            var w = bmp.Width;
            var h = bmp.Height;
            BitmapData bmpData;
            IntPtr line;
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            Marshal.WriteByte(line, x, f(x, y));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format24bppRgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 3;
                            Marshal.WriteByte(line, ofs + 0, f(x, y));
                            Marshal.WriteByte(line, ofs + 1, f(x, y));
                            Marshal.WriteByte(line, ofs + 2, f(x, y));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format32bppArgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 4;
                            Marshal.WriteByte(line, ofs + 1, f(x, y));
                            Marshal.WriteByte(line, ofs + 2, f(x, y));
                            Marshal.WriteByte(line, ofs + 3, f(x, y));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                default:
                    throw new ArgumentException("The pixel format of the given bitmap is not supported.", "bmp");
            }
        }

        /// <summary>
        /// Fills the image with intensity values, created by calling <paramref name="fR"/>, <paramref name="fG"/>, and <paramref name="fB"/> 
        /// for each pixel in the respective channel in the image.
        /// If the image has an alpha channel, it remains unchanged.
        /// If the bitmap is a gray scale image, only <paramref name="fG"/> is used.
        /// </summary>
        /// <param name="bmp">The image to fill.</param>
        /// <param name="fR">The intensity source function for the red channel.</param>
        /// <param name="fG">The intensity source function for the green channel.</param>
        /// <param name="fB">The intensity source function for the blue channel.</param>
        /// <exception cref="ArgumentNullException">Is thrown,
        /// if <c>null</c> is given for <paramref name="bmp"/>, <paramref name="fR"/>, <paramref name="fG"/>, or <paramref name="fB"/>.</exception>
        /// <exception cref="ArgumentException">Is thrown,
        /// if the pixel format of <paramref name="bmp"/> is not one of the following:
        /// <see cref="PixelFormat.Format8bppIndexed"/>, 
        /// <see cref="PixelFormat.Format24bppRgb"/>, 
        /// or <see cref="PixelFormat.Format32bppArgb"/>.
        /// </exception>
        public static void FillWith(this Bitmap bmp, IntensitySource fR, IntensitySource fG, IntensitySource fB)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (fR == null) throw new ArgumentNullException("fR");
            if (fG == null) throw new ArgumentNullException("fG");
            if (fB == null) throw new ArgumentNullException("fB");
            var w = bmp.Width;
            var h = bmp.Height;
            BitmapData bmpData;
            IntPtr line;
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            Marshal.WriteByte(line, x, fG(x, y));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format24bppRgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 3;
                            Marshal.WriteByte(line, ofs + 0, fB(x, y));
                            Marshal.WriteByte(line, ofs + 1, fG(x, y));
                            Marshal.WriteByte(line, ofs + 2, fR(x, y));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format32bppArgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 4;
                            Marshal.WriteByte(line, ofs + 0, fB(x, y));
                            Marshal.WriteByte(line, ofs + 1, fG(x, y));
                            Marshal.WriteByte(line, ofs + 2, fR(x, y));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                default:
                    throw new ArgumentException("The pixel format of the given bitmap is not supported.", "bmp");
            }
        }

        /// <summary>
        /// Fills the image with intensity values, created by calling <paramref name="fA"/>, <paramref name="fR"/>, <paramref name="fG"/>, and <paramref name="fB"/> 
        /// for each pixel in the respective channel in the image.
        /// If the bitmap is a gray scale image, only <paramref name="fG"/> is used.
        /// </summary>
        /// <param name="bmp">The image to fill.</param>
        /// <param name="fA">The transparency source function for the red channel.</param>
        /// <param name="fR">The intensity source function for the red channel.</param>
        /// <param name="fG">The intensity source function for the green channel.</param>
        /// <param name="fB">The intensity source function for the blue channel.</param>
        /// <exception cref="ArgumentNullException">Is thrown,
        /// if <c>null</c> is given for <paramref name="bmp"/>, <paramref name="fR"/>, <paramref name="fG"/>, or <paramref name="fB"/>.</exception>
        /// <exception cref="ArgumentException">Is thrown,
        /// if the pixel format of <paramref name="bmp"/> is not one of the following:
        /// <see cref="PixelFormat.Format8bppIndexed"/>, 
        /// <see cref="PixelFormat.Format24bppRgb"/>, 
        /// or <see cref="PixelFormat.Format32bppArgb"/>.
        /// </exception>
        public static void FillWith(this Bitmap bmp, IntensitySource fA, IntensitySource fR, IntensitySource fG, IntensitySource fB)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (fA == null) throw new ArgumentNullException("fA");
            if (fR == null) throw new ArgumentNullException("fR");
            if (fG == null) throw new ArgumentNullException("fG");
            if (fB == null) throw new ArgumentNullException("fB");
            var w = bmp.Width;
            var h = bmp.Height;
            BitmapData bmpData;
            IntPtr line;
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            Marshal.WriteByte(line, x, fG(x, y));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format24bppRgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 3;
                            Marshal.WriteByte(line, ofs + 0, fB(x, y));
                            Marshal.WriteByte(line, ofs + 1, fG(x, y));
                            Marshal.WriteByte(line, ofs + 2, fR(x, y));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format32bppArgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 4;
                            Marshal.WriteByte(line, ofs + 0, fB(x, y));
                            Marshal.WriteByte(line, ofs + 1, fG(x, y));
                            Marshal.WriteByte(line, ofs + 2, fR(x, y));
                            Marshal.WriteByte(line, ofs + 3, fA(x, y));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                default:
                    throw new ArgumentException("The pixel format of the given bitmap is not supported.", "bmp");
            }
        }

        /// <summary>
        /// Fills the image with intensity values, created by calling <paramref name="f"/>
        /// for each pixel in the image.
        /// If the image is a gray scale image, the intensity is used for all three color channels of the input value
        /// and the intensity of the green output channel is written to the image.
        /// </summary>
        /// <param name="bmp">The bitmap to filter.</param>
        /// <param name="f">The transfer function.</param>
        /// <exception cref="ArgumentNullException">Is thrown,
        /// if <c>null</c> is given for <paramref name="bmp"/>, or <paramref name="f"/>.</exception>
        /// <exception cref="ArgumentException">Is thrown,
        /// if the pixel format of <paramref name="bmp"/> is not on of the following:
        /// <see cref="PixelFormat.Format8bppIndexed"/>, 
        /// <see cref="PixelFormat.Format24bppRgb"/>, 
        /// or <see cref="PixelFormat.Format32bppArgb"/>.
        /// </exception>
        public static void FillWith(this Bitmap bmp, ColorSource f)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (f == null) throw new ArgumentNullException("f");
            var w = bmp.Width;
            var h = bmp.Height;
            BitmapData bmpData;
            IntPtr line;
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            Marshal.WriteByte(line, x, f(x, y).G);
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format24bppRgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 3;
                            var c = f(x, y);
                            Marshal.WriteByte(line, ofs + 0, c.B);
                            Marshal.WriteByte(line, ofs + 1, c.G);
                            Marshal.WriteByte(line, ofs + 2, c.R);
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format32bppArgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 4;
                            var c = f(x, y);
                            Marshal.WriteInt32(line, ofs, c.ToArgb());
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                default:
                    throw new ArgumentException("The pixel format of the given bitmap is not supported.", "bmp");
            }
        }

        #endregion

        #region filter

        /// <summary>
        /// Applies the filter function <paramref name="f"/> to every intensity value
        /// in the image. If the image has an alpha channel, it is not filtered.
        /// </summary>
        /// <param name="bmp">The bitmap to filter.</param>
        /// <param name="f">The transfer function.</param>
        /// <exception cref="ArgumentNullException">Is thrown,
        /// if <c>null</c> is given for <paramref name="bmp"/>, or <paramref name="f"/>.</exception>
        /// <exception cref="ArgumentException">Is thrown,
        /// if the pixel format of <paramref name="bmp"/> is not one of the following:
        /// <see cref="PixelFormat.Format8bppIndexed"/>, 
        /// <see cref="PixelFormat.Format24bppRgb"/>, 
        /// or <see cref="PixelFormat.Format32bppArgb"/>.
        /// </exception>
        public static void ApplyFilter(this Bitmap bmp, IntensityTransfer f)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (f == null) throw new ArgumentNullException("f");
            var w = bmp.Width;
            var h = bmp.Height;
            BitmapData bmpData;
            IntPtr line;
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            Marshal.WriteByte(line, x, f(x, y, Marshal.ReadByte(line, x)));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format24bppRgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 3;
                            Marshal.WriteByte(line, ofs + 0, f(x, y, Marshal.ReadByte(line, ofs + 0)));
                            Marshal.WriteByte(line, ofs + 1, f(x, y, Marshal.ReadByte(line, ofs + 1)));
                            Marshal.WriteByte(line, ofs + 2, f(x, y, Marshal.ReadByte(line, ofs + 2)));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format32bppArgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 4;
                            Marshal.WriteByte(line, ofs + 0, f(x, y, Marshal.ReadByte(line, ofs + 0)));
                            Marshal.WriteByte(line, ofs + 1, f(x, y, Marshal.ReadByte(line, ofs + 1)));
                            Marshal.WriteByte(line, ofs + 2, f(x, y, Marshal.ReadByte(line, ofs + 2)));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                default:
                    throw new ArgumentException("The pixel format of the given bitmap is not supported.", "bmp");
            }
        }

        /// <summary>
        /// Applies the filter functions <paramref name="fR"/>, <paramref name="fG"/>, and <paramref name="fB"/> 
        /// to the intensity values of the respective channel in the image. 
        /// If the image has an alpha channel, it is not filtered.
        /// If the bitmap is a gray scale image, only <paramref name="fG"/> is used for the traansfer.
        /// </summary>
        /// <param name="bmp">The bitmap to filter.</param>
        /// <param name="fR">The transfer function for the red channel.</param>
        /// <param name="fG">The transfer function for the green channel.</param>
        /// <param name="fB">The transfer function for the blue channel.</param>
        /// <exception cref="ArgumentNullException">Is thrown,
        /// if <c>null</c> is given for <paramref name="bmp"/>, <paramref name="fR"/>, <paramref name="fG"/>, or <paramref name="fB"/>.</exception>
        /// <exception cref="ArgumentException">Is thrown,
        /// if the pixel format of <paramref name="bmp"/> is not one of the following:
        /// <see cref="PixelFormat.Format8bppIndexed"/>, 
        /// <see cref="PixelFormat.Format24bppRgb"/>, 
        /// or <see cref="PixelFormat.Format32bppArgb"/>.
        /// </exception>
        public static void ApplyFilter(this Bitmap bmp, IntensityTransfer fR, IntensityTransfer fG, IntensityTransfer fB)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (fR == null) throw new ArgumentNullException("fR");
            if (fG == null) throw new ArgumentNullException("fG");
            if (fB == null) throw new ArgumentNullException("fB");
            var w = bmp.Width;
            var h = bmp.Height;
            BitmapData bmpData;
            IntPtr line;
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            Marshal.WriteByte(line, x, fG(x, y, Marshal.ReadByte(line, x)));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format24bppRgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 3;
                            Marshal.WriteByte(line, ofs + 0, fB(x, y, Marshal.ReadByte(line, ofs + 0)));
                            Marshal.WriteByte(line, ofs + 1, fG(x, y, Marshal.ReadByte(line, ofs + 1)));
                            Marshal.WriteByte(line, ofs + 2, fR(x, y, Marshal.ReadByte(line, ofs + 2)));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format32bppArgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 4;
                            Marshal.WriteByte(line, ofs + 0, fB(x, y, Marshal.ReadByte(line, ofs + 0)));
                            Marshal.WriteByte(line, ofs + 1, fG(x, y, Marshal.ReadByte(line, ofs + 1)));
                            Marshal.WriteByte(line, ofs + 2, fR(x, y, Marshal.ReadByte(line, ofs + 2)));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                default:
                    throw new ArgumentException("The pixel format of the given bitmap is not supported.", "bmp");
            }
        }

        /// <summary>
        /// Applies the filter functions <paramref name="fA"/>, <paramref name="fR"/>, <paramref name="fG"/>, and <paramref name="fB"/> 
        /// to the values of the respective channel in the image.
        /// If the bitmap is a gray scale image, only <paramref name="fG"/> is used for the traansfer.
        /// </summary>
        /// <param name="bmp">The bitmap to filter.</param>
        /// <param name="fA">The transfer function for the alpha channel.</param>
        /// <param name="fR">The transfer function for the red channel.</param>
        /// <param name="fG">The transfer function for the green channel.</param>
        /// <param name="fB">The transfer function for the blue channel.</param>
        /// <exception cref="ArgumentNullException">Is thrown,
        /// if <c>null</c> is given for <paramref name="bmp"/>, <paramref name="fA"/>, <paramref name="fR"/>, <paramref name="fG"/>, or <paramref name="fB"/>.</exception>
        /// <exception cref="ArgumentException">Is thrown,
        /// if the pixel format of <paramref name="bmp"/> is not on of the following:
        /// <see cref="PixelFormat.Format8bppIndexed"/>, 
        /// <see cref="PixelFormat.Format24bppRgb"/>, 
        /// or <see cref="PixelFormat.Format32bppArgb"/>.
        /// </exception>
        public static void ApplyFilter(this Bitmap bmp, IntensityTransfer fA, IntensityTransfer fR, IntensityTransfer fG, IntensityTransfer fB)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (fA == null) throw new ArgumentNullException("fA");
            if (fR == null) throw new ArgumentNullException("fR");
            if (fG == null) throw new ArgumentNullException("fG");
            if (fB == null) throw new ArgumentNullException("fB");
            var w = bmp.Width;
            var h = bmp.Height;
            BitmapData bmpData;
            IntPtr line;
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            Marshal.WriteByte(line, x, fG(x, y, Marshal.ReadByte(line, x)));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format24bppRgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 3;
                            Marshal.WriteByte(line, ofs + 0, fB(x, y, Marshal.ReadByte(line, ofs + 0)));
                            Marshal.WriteByte(line, ofs + 1, fG(x, y, Marshal.ReadByte(line, ofs + 1)));
                            Marshal.WriteByte(line, ofs + 2, fR(x, y, Marshal.ReadByte(line, ofs + 2)));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format32bppArgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 4;
                            Marshal.WriteByte(line, ofs + 0, fB(x, y, Marshal.ReadByte(line, ofs + 0)));
                            Marshal.WriteByte(line, ofs + 1, fG(x, y, Marshal.ReadByte(line, ofs + 1)));
                            Marshal.WriteByte(line, ofs + 2, fR(x, y, Marshal.ReadByte(line, ofs + 2)));
                            Marshal.WriteByte(line, ofs + 3, fA(x, y, Marshal.ReadByte(line, ofs + 3)));
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                default:
                    throw new ArgumentException("The pixel format of the given bitmap is not supported.", "bmp");
            }
        }

        /// <summary>
        /// Applies the filter function <paramref name="f"/> to the values in the image.
        /// If the image is a gray scale image, the intensity is used for all three color channels of the input value
        /// and the intensity of the green output channel is written to the image.
        /// </summary>
        /// <param name="bmp">The bitmap to filter.</param>
        /// <param name="f">The transfer function.</param>
        /// <exception cref="ArgumentNullException">Is thrown,
        /// if <c>null</c> is given for <paramref name="bmp"/>, or <paramref name="f"/>.</exception>
        /// <exception cref="ArgumentException">Is thrown,
        /// if the pixel format of <paramref name="bmp"/> is not on of the following:
        /// <see cref="PixelFormat.Format8bppIndexed"/>, 
        /// <see cref="PixelFormat.Format24bppRgb"/>, 
        /// or <see cref="PixelFormat.Format32bppArgb"/>.
        /// </exception>
        public static void ApplyFilter(this Bitmap bmp, ColorTransfer f)
        {
            if (bmp == null) throw new ArgumentNullException("bmp");
            if (f == null) throw new ArgumentNullException("f");
            var w = bmp.Width;
            var h = bmp.Height;
            BitmapData bmpData;
            IntPtr line;
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format8bppIndexed:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var v = Marshal.ReadByte(line, x);
                            var c = Color.FromArgb(v, v, v);
                            c = f(x, y, c);
                            Marshal.WriteByte(line, x, c.G);
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format24bppRgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 3;
                            var c = Color.FromArgb(
                                Marshal.ReadByte(line, ofs + 2),
                                Marshal.ReadByte(line, ofs + 1),
                                Marshal.ReadByte(line, ofs + 0));
                            c = f(x, y, c);
                            Marshal.WriteByte(line, ofs + 0, c.B);
                            Marshal.WriteByte(line, ofs + 1, c.G);
                            Marshal.WriteByte(line, ofs + 2, c.R);
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                case PixelFormat.Format32bppArgb:
                    bmpData = bmp.LockBits(new Rectangle(0, 0, w, h),
                        ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                    line = bmpData.Scan0;
                    for (var y = 0; y < h; y++)
                    {
                        for (var x = 0; x < w; x++)
                        {
                            var ofs = x * 4;
                            var c = Color.FromArgb(Marshal.ReadInt32(line, ofs));
                            c = f(x, y, c);
                            Marshal.WriteInt32(line, ofs, c.ToArgb());
                        }
                        line += bmpData.Stride;
                    }
                    bmp.UnlockBits(bmpData);
                    break;
                default:
                    throw new ArgumentException("The pixel format of the given bitmap is not supported.", "bmp");
            }
        }

        #endregion
    }

    /// <summary>
    /// A delegate for an intensity source function regarding the pixel position.
    /// </summary>
    /// <param name="x">The position of the pixel on the X axis.</param>
    /// <param name="y">The position of the pixel on the Y axis.</param>
    /// <returns>The resulting intensity value.</returns>
    public delegate byte IntensitySource(int x, int y);

    /// <summary>
    /// A delegate for a color source function regarding the pixel position.
    /// </summary>
    /// <param name="x">The position of the pixel on the X axis.</param>
    /// <param name="y">The position of the pixel on the Y axis.</param>
    /// <returns>The resulting color value.</returns>
    public delegate Color ColorSource(int x, int y);

    /// <summary>
    /// A delegate for an intensity transfer function regarding the pixel position.
    /// </summary>
    /// <param name="x">The position of the pixel on the X axis.</param>
    /// <param name="y">The position of the pixel on the Y axis.</param>
    /// <param name="value">The original intensity value.</param>
    /// <returns>The resulting intensity value.</returns>
    public delegate byte IntensityTransfer(int x, int y, byte value);

    /// <summary>
    /// A delegate for a color transfer function regarding the pixel position.
    /// </summary>
    /// <param name="x">The position of the pixel on the X axis.</param>
    /// <param name="y">The position of the pixel on the Y axis.</param>
    /// <param name="value">The original intensity value.</param>
    /// <returns>The resulting color value.</returns>
    public delegate Color ColorTransfer(int x, int y, Color value);
}