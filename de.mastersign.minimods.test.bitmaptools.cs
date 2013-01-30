using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using NUnit.Framework;
using de.mastersign.minimods.bitmaptools;

namespace de.mastersign.minimods.test.bitmaptools
{
    internal class BitmapToolsTest
    {
        [Test]
        public void CreateGrayScaleBitmapTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateGrayScaleBitmap(-1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateGrayScaleBitmap(1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateGrayScaleBitmap(-1, -1));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateGrayScaleBitmap(1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateGrayScaleBitmap(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateGrayScaleBitmap(0, 0));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateGrayScaleBitmap(1, int.MaxValue));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateGrayScaleBitmap(int.MaxValue, 1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateGrayScaleBitmap(int.MaxValue, int.MaxValue));

            using (var o = BitmapTools.CreateGrayScaleBitmap(1, 1))
            {
                Assert.AreEqual(1, o.Width);
                Assert.AreEqual(1, o.Height);
                Assert.AreEqual(PixelFormat.Format8bppIndexed, o.PixelFormat);
                Assert.IsTrue(o.Palette.Entries.All(c => c.R == c.G && c.G == c.B && c.A == 255));
            }
            using (var o = BitmapTools.CreateGrayScaleBitmap(2, 3))
            {
                Assert.AreEqual(2, o.Width);
                Assert.AreEqual(3, o.Height);
            }
        }

        [Test]
        public void CreateRgbBitmapTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateRgbBitmap(-1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateRgbBitmap(1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateRgbBitmap(-1, -1));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateRgbBitmap(1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateRgbBitmap(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateRgbBitmap(0, 0));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateRgbBitmap(1, int.MaxValue));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateRgbBitmap(int.MaxValue, 1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateRgbBitmap(int.MaxValue, int.MaxValue));

            using (var o = BitmapTools.CreateRgbBitmap(1, 2))
            {
                Assert.AreEqual(1, o.Width);
                Assert.AreEqual(2, o.Height);
                Assert.AreEqual(PixelFormat.Format24bppRgb, o.PixelFormat);
            }
        }

        [Test]
        public void CreateArgbBitmapTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateArgbBitmap(-1, 1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateArgbBitmap(1, -1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateArgbBitmap(-1, -1));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateArgbBitmap(1, 0));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateArgbBitmap(0, 1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateArgbBitmap(0, 0));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateArgbBitmap(1, int.MaxValue));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateArgbBitmap(int.MaxValue, 1));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateArgbBitmap(int.MaxValue, int.MaxValue));

            using (var o = BitmapTools.CreateArgbBitmap(1, 2))
            {
                Assert.AreEqual(1, o.Width);
                Assert.AreEqual(2, o.Height);
                Assert.AreEqual(PixelFormat.Format32bppArgb, o.PixelFormat);
            }
        }

        [Test]
        public void CreateFromArrayGrayScaleTest()
        {
            var v = new byte[,]
                {
                    { 1, 2, 3 }, 
                    { 4, 5, 6 }
                };

            Assert.Throws<ArgumentNullException>(
                () => BitmapTools.CreateFromArray((byte[,])null));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(new byte[0, 0]));

            using (var o = BitmapTools.CreateFromArray(v))
            {
                Assert.IsNotNull(o);
                Assert.AreEqual(PixelFormat.Format8bppIndexed, o.PixelFormat);
                Assert.AreEqual(3, o.Width);
                Assert.AreEqual(2, o.Height);
                var tl = v[0, 0];
                var tr = v[0, 2];
                var bl = v[1, 0];
                var br = v[1, 2];
                Assert.AreEqual(Color.FromArgb(tl, tl, tl), o.GetPixel(0, 0));
                Assert.AreEqual(Color.FromArgb(tr, tr, tr), o.GetPixel(2, 0));
                Assert.AreEqual(Color.FromArgb(br, br, br), o.GetPixel(2, 1));
                Assert.AreEqual(Color.FromArgb(bl, bl, bl), o.GetPixel(0, 1));
            }
        }

        [Test]
        public void CreateFromArrayRgbTest()
        {
            var r = new byte[,]
                {
                    { 11, 12, 13 }, 
                    { 14, 15, 16 }
                };
            var g = new byte[,]
                {
                    { 21, 22, 23 }, 
                    { 24, 25, 26 }
                };
            var b = new byte[,]
                {
                    { 31, 32, 33 }, 
                    { 34, 35, 36 }
                };

            Assert.Throws<ArgumentNullException>(
                () => BitmapTools.CreateFromArray(null, g, b));
            Assert.Throws<ArgumentNullException>(
                () => BitmapTools.CreateFromArray(r, null, b));
            Assert.Throws<ArgumentNullException>(
                () => BitmapTools.CreateFromArray(r, g, null));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(new byte[0, 0], g, b));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(r, new byte[0, 0], b));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(r, g, new byte[0, 0]));

            using (var o = BitmapTools.CreateFromArray(r, g, b))
            {
                Assert.IsNotNull(o);
                Assert.AreEqual(PixelFormat.Format24bppRgb, o.PixelFormat);
                Assert.AreEqual(3, o.Width);
                Assert.AreEqual(2, o.Height);

                for (var y = 0; y < 2; y++)
                {
                    for (var x = 0; x < 3; x++)
                    {
                        var c = o.GetPixel(x, y);
                        Assert.AreEqual(r[y, x], c.R);
                        Assert.AreEqual(g[y, x], c.G);
                        Assert.AreEqual(b[y, x], c.B);
                    }
                }
            }
        }

        [Test]
        public void CreateFromArrayArgbTest()
        {
            var a = new byte[,]
                {
                    { 01, 02, 03 }, 
                    { 04, 05, 06 }
                };
            var r = new byte[,]
                {
                    { 11, 12, 13 }, 
                    { 14, 15, 16 }
                };
            var g = new byte[,]
                {
                    { 21, 22, 23 }, 
                    { 24, 25, 26 }
                };
            var b = new byte[,]
                {
                    { 31, 32, 33 }, 
                    { 34, 35, 36 }
                };

            Assert.Throws<ArgumentNullException>(
                () => BitmapTools.CreateFromArray(null, r, g, b));
            Assert.Throws<ArgumentNullException>(
                () => BitmapTools.CreateFromArray(a, null, g, b));
            Assert.Throws<ArgumentNullException>(
                () => BitmapTools.CreateFromArray(a, r, null, b));
            Assert.Throws<ArgumentNullException>(
                () => BitmapTools.CreateFromArray(a, r, g, null));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(new byte[0, 0], r, g, b));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(a, new byte[0, 0], g, b));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(a, r, new byte[0, 0], b));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(a, r, g, new byte[0, 0]));

            using (var o = BitmapTools.CreateFromArray(a, r, g, b))
            {
                Assert.IsNotNull(o);
                Assert.AreEqual(PixelFormat.Format32bppArgb, o.PixelFormat);
                Assert.AreEqual(3, o.Width);
                Assert.AreEqual(2, o.Height);

                for (var y = 0; y < 2; y++)
                {
                    for (var x = 0; x < 3; x++)
                    {
                        var c = o.GetPixel(x, y);
                        Assert.AreEqual(a[y, x], c.A);
                        Assert.AreEqual(r[y, x], c.R);
                        Assert.AreEqual(g[y, x], c.G);
                        Assert.AreEqual(b[y, x], c.B);
                    }
                }
            }
        }

        [Test]
        public void CreateFromArrayMultiPreconditionTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => BitmapTools.CreateFromArray((byte[, ,])null));

            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(new byte[0, 1, 1]));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(new byte[1, 0, 1]));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(new byte[1, 1, 0]));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(new byte[1, 1, 2]));
            Assert.Throws<ArgumentOutOfRangeException>(
                () => BitmapTools.CreateFromArray(new byte[1, 1, 5]));
        }

        [Test]
        public void CreateFromArrayMultiGrayScaleTest()
        {
            var gray = new byte[,,]
                {
                    {{11}, {12},{13}},
                    {{14}, {52},{16}},
                };

            using (var o = BitmapTools.CreateFromArray(gray))
            {
                Assert.IsNotNull(o);
                Assert.AreEqual(PixelFormat.Format8bppIndexed, o.PixelFormat);
                Assert.AreEqual(3, o.Width);
                Assert.AreEqual(2, o.Height);

                for (var y = 0; y < 2; y++)
                {
                    for (var x = 0; x < 3; x++)
                    {
                        var c = o.GetPixel(x, y);
                        Assert.AreEqual(255, c.A);
                        Assert.AreEqual(gray[y, x, 0], c.R);
                        Assert.AreEqual(gray[y, x, 0], c.G);
                        Assert.AreEqual(gray[y, x, 0], c.B);
                    }
                }
            }
        }

        [Test]
        public void CreateFromArrayMultiRgbTest()
        {
            var rgb = new byte[,,]
                {
                    {{11, 21, 31}, {12, 22, 32},{13, 23, 33}},
                    {{14, 24, 34}, {52, 52, 52},{16, 26, 36}},
                };

            using (var o = BitmapTools.CreateFromArray(rgb))
            {
                Assert.IsNotNull(o);
                Assert.AreEqual(PixelFormat.Format24bppRgb, o.PixelFormat);
                Assert.AreEqual(3, o.Width);
                Assert.AreEqual(2, o.Height);

                for (var y = 0; y < 2; y++)
                {
                    for (var x = 0; x < 3; x++)
                    {
                        var c = o.GetPixel(x, y);
                        Assert.AreEqual(255, c.A);
                        Assert.AreEqual(rgb[y, x, 0], c.R);
                        Assert.AreEqual(rgb[y, x, 1], c.G);
                        Assert.AreEqual(rgb[y, x, 2], c.B);
                    }
                }
            }
        }

        [Test]
        public void CreateFromArrayMultiArgbTest()
        {
            var argb = new byte[,,]
                {
                    {{01, 11, 21, 31}, {02, 12, 22, 32},{03, 13, 23, 33}},
                    {{04, 14, 24, 34}, {05, 52, 52, 52},{06, 16, 26, 36}},
                };

            using (var o = BitmapTools.CreateFromArray(argb))
            {
                Assert.IsNotNull(o);
                Assert.AreEqual(PixelFormat.Format32bppArgb, o.PixelFormat);
                Assert.AreEqual(3, o.Width);
                Assert.AreEqual(2, o.Height);

                for (var y = 0; y < 2; y++)
                {
                    for (var x = 0; x < 3; x++)
                    {
                        var c = o.GetPixel(x, y);
                        Assert.AreEqual(argb[y, x, 0], c.A);
                        Assert.AreEqual(argb[y, x, 1], c.R);
                        Assert.AreEqual(argb[y, x, 2], c.G);
                        Assert.AreEqual(argb[y, x, 3], c.B);
                    }
                }
            }
        }
    }
}
