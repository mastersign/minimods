using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using NUnit.Framework;

namespace Mastersign.Minimods.BitmapTools.Test
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
                    {{11, 21, 31}, {12, 22, 32},{13, 23, 33}, {14, 24, 34}},
                    {{15, 25, 35}, {16, 26, 36},{17, 27, 37}, {18, 28, 38}},
                };
            var h = rgb.GetLength(0);
            var w = rgb.GetLength(1);

            using (var o = BitmapTools.CreateFromArray(rgb))
            {
                Assert.IsNotNull(o);
                Assert.AreEqual(PixelFormat.Format24bppRgb, o.PixelFormat);
                Assert.AreEqual(w, o.Width);
                Assert.AreEqual(h, o.Height);

                for (var y = 0; y < h; y++)
                {
                    for (var x = 0; x < w; x++)
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

    public class BitmapExtensionTest
    {
        [Test]
        public void SetPaletteToGrayScaleTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => BitmapExtension.SetPaletteToGrayScale(null));

            var o = new Bitmap(1, 1, PixelFormat.Format8bppIndexed);
            o.SetPaletteToGrayScale();
            Assert.IsTrue(o.Palette.Entries.All(c => c.R == c.G && c.G == c.B && c.A == 255));
        }

        [Test]
        public void ToGrayScaleArrayTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => BitmapExtension.ToGrayScaleArray(null));

            using (var rgb = BitmapTools.CreateRgbBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(() => rgb.ToGrayScaleArray());
            }

            using (var argb = BitmapTools.CreateArgbBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(() => argb.ToGrayScaleArray());
            }

            var v = new byte[,]
                {
                    { 1, 2, 3 }, 
                    { 4, 5, 6 }
                };
            var w = v.GetLength(1);
            var h = v.GetLength(0);
            var o = BitmapTools.CreateFromArray(v);

            var v2 = o.ToGrayScaleArray();
            Assert.NotNull(v2);
            Assert.AreEqual(v2.GetLength(0), v.GetLength(0));
            Assert.AreEqual(v2.GetLength(1), v.GetLength(1));
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    Assert.AreEqual(v[y, x], v2[y, x]);
                }
            }

            o.Dispose();
        }

        [Test]
        public void ToRgbArrayTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => BitmapExtension.ToRgbArray(null));

            using (var gray = BitmapTools.CreateGrayScaleBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(() => gray.ToRgbArray());
            }

            using (var argb = BitmapTools.CreateArgbBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(() => argb.ToRgbArray());
            }

            var rgb = new byte[,,]
                {
                    {{11, 21, 31}, {12, 22, 32},{13, 23, 33}, {14, 24, 34}},
                    {{15, 25, 35}, {16, 26, 36},{17, 27, 37}, {18, 28, 38}},
                };
            var w = rgb.GetLength(1);
            var h = rgb.GetLength(0);
            var o = BitmapTools.CreateFromArray(rgb);

            var rgb2 = o.ToRgbArray();
            Assert.NotNull(rgb2);
            Assert.AreEqual(rgb2.GetLength(0), rgb.GetLength(0));
            Assert.AreEqual(rgb2.GetLength(1), rgb.GetLength(1));
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    Assert.AreEqual(rgb[y, x, 0], rgb2[y, x, 0]);
                    Assert.AreEqual(rgb[y, x, 1], rgb2[y, x, 1]);
                    Assert.AreEqual(rgb[y, x, 2], rgb2[y, x, 2]);
                }
            }

            o.Dispose();
        }

        [Test]
        public void ToArgbArrayTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => BitmapExtension.ToArgbArray(null));

            using (var gray = BitmapTools.CreateGrayScaleBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(() => gray.ToArgbArray());
            }

            using (var rgb = BitmapTools.CreateRgbBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(() => rgb.ToArgbArray());
            }

            var argb = new byte[,,]
                {
                    {{01, 11, 21, 31}, {02, 12, 22, 32},{03, 13, 23, 33}},
                    {{04, 14, 24, 34}, {05, 52, 52, 52},{06, 16, 26, 36}},
                };
            var w = argb.GetLength(1);
            var h = argb.GetLength(0);
            var o = BitmapTools.CreateFromArray(argb);

            var argb2 = o.ToArgbArray();
            Assert.NotNull(argb2);
            Assert.AreEqual(argb2.GetLength(0), argb.GetLength(0));
            Assert.AreEqual(argb2.GetLength(1), argb.GetLength(1));
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    Assert.AreEqual(argb[y, x, 0], argb2[y, x, 0]);
                    Assert.AreEqual(argb[y, x, 1], argb2[y, x, 1]);
                    Assert.AreEqual(argb[y, x, 2], argb2[y, x, 2]);
                    Assert.AreEqual(argb[y, x, 3], argb2[y, x, 3]);
                }
            }

            o.Dispose();
        }

        [Test]
        public void CopyToArrayGrayScaleTest()
        {
            var v = new byte[,]
                {
                    { 1, 2, 3 }, 
                    { 4, 5, 6 }
                };
            var w = v.GetLength(1);
            var h = v.GetLength(0);
            var o = BitmapTools.CreateFromArray(v);

            Assert.Throws<ArgumentNullException>(
                () => BitmapExtension.CopyToArray(null, new byte[1, 1]));

            using (var rgb = BitmapTools.CreateRgbBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(
                    () => rgb.CopyToArray(new byte[1, 1]));
            }

            using (var argb = BitmapTools.CreateArgbBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(
                    () => argb.CopyToArray(new byte[1, 1]));
            }

            Assert.Throws<ArgumentNullException>(() => o.CopyToArray((byte[,])null));

            Assert.Throws<ArgumentException>(() => o.CopyToArray(new byte[0, 0]));

            Assert.Throws<ArgumentException>(() => o.CopyToArray(new byte[h - 1, w - 1]));

            Assert.Throws<ArgumentException>(() => o.CopyToArray(new byte[h + 1, w + 1]));

            var v2 = new byte[h, w];
            o.CopyToArray(v2);
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    Assert.AreEqual(v[y, x], v2[y, x]);
                }
            }

            o.Dispose();
        }

        [Test]
        public void CopyToArrayRgbTest()
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
            var w = r.GetLength(1);
            var h = r.GetLength(0);
            var o = BitmapTools.CreateFromArray(r, g, b);

            Assert.Throws<ArgumentNullException>(
                () => BitmapExtension.CopyToArray(null, new byte[1, 1], new byte[1, 1], new byte[1, 1]));

            using (var gray = BitmapTools.CreateGrayScaleBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(
                    () => gray.CopyToArray(new byte[1, 1], new byte[1, 1], new byte[1, 1]));
            }

            using (var argb = BitmapTools.CreateArgbBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(
                    () => argb.CopyToArray(new byte[1, 1], new byte[1, 1], new byte[1, 1]));
            }

            Assert.Throws<ArgumentNullException>(() => o.CopyToArray(null, g, b));
            Assert.Throws<ArgumentNullException>(() => o.CopyToArray(r, null, b));
            Assert.Throws<ArgumentNullException>(() => o.CopyToArray(r, g, null));
            Assert.Throws<ArgumentException>(() => o.CopyToArray(new byte[0, 0], g, b));
            Assert.Throws<ArgumentException>(() => o.CopyToArray(new byte[h - 1, w - 1], g, b));
            Assert.Throws<ArgumentException>(() => o.CopyToArray(new byte[h + 1, w + 1], g, b));

            var r2 = new byte[h, w];
            var g2 = new byte[h, w];
            var b2 = new byte[h, w];
            o.CopyToArray(r2, g2, b2);
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    Assert.AreEqual(r[y, x], r2[y, x]);
                    Assert.AreEqual(g[y, x], g2[y, x]);
                    Assert.AreEqual(b[y, x], b2[y, x]);
                }
            }

            o.Dispose();
        }

        [Test]
        public void CopyToArrayArgbTest()
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
            var w = r.GetLength(1);
            var h = r.GetLength(0);
            var o = BitmapTools.CreateFromArray(a, r, g, b);

            Assert.Throws<ArgumentNullException>(
                () => BitmapExtension.CopyToArray(null, new byte[1, 1], new byte[1, 1], new byte[1, 1], new byte[1, 1]));

            using (var gray = BitmapTools.CreateGrayScaleBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(
                    () => gray.CopyToArray(new byte[1, 1], new byte[1, 1], new byte[1, 1], new byte[1, 1]));
            }

            using (var rgb = BitmapTools.CreateRgbBitmap(1, 1))
            {
                Assert.Throws<ArgumentException>(
                    () => rgb.CopyToArray(new byte[1, 1], new byte[1, 1], new byte[1, 1], new byte[1, 1]));
            }

            Assert.Throws<ArgumentNullException>(() => o.CopyToArray(null, r, g, b));
            Assert.Throws<ArgumentNullException>(() => o.CopyToArray(a, null, g, b));
            Assert.Throws<ArgumentNullException>(() => o.CopyToArray(a, r, null, b));
            Assert.Throws<ArgumentNullException>(() => o.CopyToArray(a, r, g, null));
            Assert.Throws<ArgumentException>(() => o.CopyToArray(new byte[0, 0], r, g, b));
            Assert.Throws<ArgumentException>(() => o.CopyToArray(new byte[h - 1, w - 1], r, g, b));
            Assert.Throws<ArgumentException>(() => o.CopyToArray(new byte[h + 1, w + 1], r, g, b));

            var a2 = new byte[h, w];
            var r2 = new byte[h, w];
            var g2 = new byte[h, w];
            var b2 = new byte[h, w];
            o.CopyToArray(a2, r2, g2, b2);
            for (var y = 0; y < h; y++)
            {
                for (var x = 0; x < w; x++)
                {
                    Assert.AreEqual(a[y, x], a2[y, x]);
                    Assert.AreEqual(r[y, x], r2[y, x]);
                    Assert.AreEqual(g[y, x], g2[y, x]);
                    Assert.AreEqual(b[y, x], b2[y, x]);
                }
            }

            o.Dispose();
        }

        [Test]
        public void CopyToArrayMultiPreconditionTest()
        {
            using (var gray = BitmapTools.CreateGrayScaleBitmap(1, 1))
            {
                Assert.Throws<ArgumentNullException>(() => gray.CopyToArray((byte[, ,])null));
                Assert.Throws<ArgumentOutOfRangeException>(() => gray.CopyToArray(new byte[0, 1, 1]));
                Assert.Throws<ArgumentOutOfRangeException>(() => gray.CopyToArray(new byte[1, 0, 1]));
                Assert.Throws<ArgumentOutOfRangeException>(() => gray.CopyToArray(new byte[1, 1, 0]));
                Assert.Throws<ArgumentOutOfRangeException>(() => gray.CopyToArray(new byte[1, 1, 2]));
            }
            using (var rgb = BitmapTools.CreateRgbBitmap(1, 1))
            {
                Assert.Throws<ArgumentNullException>(() => rgb.CopyToArray((byte[, ,])null));
                Assert.Throws<ArgumentOutOfRangeException>(() => rgb.CopyToArray(new byte[0, 1, 3]));
                Assert.Throws<ArgumentOutOfRangeException>(() => rgb.CopyToArray(new byte[1, 0, 3]));
                Assert.Throws<ArgumentOutOfRangeException>(() => rgb.CopyToArray(new byte[1, 1, 2]));
                Assert.Throws<ArgumentOutOfRangeException>(() => rgb.CopyToArray(new byte[1, 1, 4]));
            }
            using (var argb = BitmapTools.CreateArgbBitmap(1, 1))
            {
                Assert.Throws<ArgumentNullException>(() => argb.CopyToArray((byte[, ,])null));
                Assert.Throws<ArgumentOutOfRangeException>(() => argb.CopyToArray(new byte[0, 1, 4]));
                Assert.Throws<ArgumentOutOfRangeException>(() => argb.CopyToArray(new byte[1, 0, 4]));
                Assert.Throws<ArgumentOutOfRangeException>(() => argb.CopyToArray(new byte[1, 1, 3]));
                Assert.Throws<ArgumentOutOfRangeException>(() => argb.CopyToArray(new byte[1, 1, 5]));
            }
        }

        [Test]
        public void CopyToArrayMultiGrayScaleTest()
        {
            var gray = new byte[,,]
                {
                    {{11}, {12},{13}},
                    {{14}, {52},{16}},
                };
            var w = gray.GetLength(1);
            var h = gray.GetLength(0);

            using (var o = BitmapTools.CreateFromArray(gray))
            {
                var gray2 = new byte[h, w, 1];
                o.CopyToArray(gray2);
                for (var y = 0; y < h; y++)
                {
                    for (var x = 0; x < w; x++)
                    {
                        Assert.AreEqual(gray[y, x, 0], gray2[y, x, 0]);
                    }
                }
            }
        }

        [Test]
        public void CopyToArrayMultiRgbTest()
        {
            var rgb = new byte[,,]
                {
                    {{11, 21, 31}, {12, 22, 32}, {13, 23, 33}, {14, 24, 34}},
                    {{15, 25, 35}, {16, 26, 36}, {17, 27, 37}, {18, 28, 38}},
                };
            var w = rgb.GetLength(1);
            var h = rgb.GetLength(0);

            using (var o = BitmapTools.CreateFromArray(rgb))
            {
                var rgb2 = new byte[h, w, 3];
                o.CopyToArray(rgb2);
                for (var y = 0; y < h; y++)
                {
                    for (var x = 0; x < w; x++)
                    {
                        Assert.AreEqual(rgb[y, x, 0], rgb2[y, x, 0]);
                        Assert.AreEqual(rgb[y, x, 1], rgb2[y, x, 1]);
                        Assert.AreEqual(rgb[y, x, 2], rgb2[y, x, 2]);
                    }
                }
            }
        }

        [Test]
        public void CopyToArrayMultiArgbTest()
        {
            var argb = new byte[,,]
                {
                    {{01, 11, 21, 31}, {02, 12, 22, 32}, {03, 13, 23, 33}},
                    {{04, 14, 24, 34}, {05, 52, 52, 52}, {06, 16, 26, 36}},
                };
            var w = argb.GetLength(1);
            var h = argb.GetLength(0);

            using (var o = BitmapTools.CreateFromArray(argb))
            {
                var argb2 = new byte[h, w, 4];
                o.CopyToArray(argb2);
                for (var y = 0; y < h; y++)
                {
                    for (var x = 0; x < w; x++)
                    {
                        Assert.AreEqual(argb[y, x, 0], argb2[y, x, 0]);
                        Assert.AreEqual(argb[y, x, 1], argb2[y, x, 1]);
                        Assert.AreEqual(argb[y, x, 2], argb2[y, x, 2]);
                        Assert.AreEqual(argb[y, x, 3], argb2[y, x, 3]);
                    }
                }
            }
        }

        [Test]
        public void CopyFromArrayGrayScaleTest()
        {
            var v = new byte[,]
                {
                    { 1, 2, 3 }, 
                    { 4, 5, 6 }
                };
            var w = v.GetLength(1);
            var h = v.GetLength(0);
            using (var o = BitmapTools.CreateGrayScaleBitmap(w, h))
            {
                Assert.Throws<ArgumentNullException>(() => BitmapExtension.CopyFromArray(null, v));
                Assert.Throws<ArgumentNullException>(() => o.CopyFromArray((byte[,])null));

                Assert.Throws<ArgumentOutOfRangeException>(() => o.CopyFromArray(new byte[0, 0]));
                Assert.Throws<ArgumentOutOfRangeException>(() => o.CopyFromArray(new byte[h, 0]));
                Assert.Throws<ArgumentOutOfRangeException>(() => o.CopyFromArray(new byte[0, w]));

                o.CopyFromArray(v);

                for (var y = 0; y < h; y++)
                {
                    for (var x = 0; x < w; x++)
                    {
                        var c = o.GetPixel(x, y);
                        Assert.AreEqual(255, c.A);
                        Assert.AreEqual(v[y, x], c.R);
                        Assert.AreEqual(v[y, x], c.G);
                        Assert.AreEqual(v[y, x], c.B);
                    }
                }
            }

            using (var o = BitmapTools.CreateRgbBitmap(w, h))
            {
                Assert.Throws<ArgumentException>(() => o.CopyFromArray(v));
            }
            using (var o = BitmapTools.CreateArgbBitmap(w, h))
            {
                Assert.Throws<ArgumentException>(() => o.CopyFromArray(v));
            }
        }

        [Test]
        public void CopyFromArrayRgbTest()
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
            var w = r.GetLength(1);
            var h = r.GetLength(0);
            using (var o = BitmapTools.CreateRgbBitmap(w, h))
            {
                Assert.Throws<ArgumentNullException>(() => BitmapExtension.CopyFromArray(null, r, g, b));
                Assert.Throws<ArgumentNullException>(() => o.CopyFromArray(null, g, b));
                Assert.Throws<ArgumentNullException>(() => o.CopyFromArray(r, null, b));
                Assert.Throws<ArgumentNullException>(() => o.CopyFromArray(r, g, null));

                Assert.Throws<ArgumentOutOfRangeException>(() => o.CopyFromArray(new byte[0, 0], g, b));
                Assert.Throws<ArgumentOutOfRangeException>(() => o.CopyFromArray(new byte[h, 0], g, b));
                Assert.Throws<ArgumentOutOfRangeException>(() => o.CopyFromArray(new byte[0, w], g, b));

                o.CopyFromArray(r, g, b);

                for (var y = 0; y < h; y++)
                {
                    for (var x = 0; x < w; x++)
                    {
                        var c = o.GetPixel(x, y);
                        Assert.AreEqual(255, c.A);
                        Assert.AreEqual(r[y, x], c.R);
                        Assert.AreEqual(g[y, x], c.G);
                        Assert.AreEqual(b[y, x], c.B);
                    }
                }
            }

            using (var o = BitmapTools.CreateGrayScaleBitmap(w, h))
            {
                Assert.Throws<ArgumentException>(() => o.CopyFromArray(r, g, b));
            }
            using (var o = BitmapTools.CreateArgbBitmap(w, h))
            {
                Assert.Throws<ArgumentException>(() => o.CopyFromArray(r, g, b));
            }

        }

        [Test]
        public void CopyFromArrayArgbTest()
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
            var w = a.GetLength(1);
            var h = a.GetLength(0);
            using (var o = BitmapTools.CreateArgbBitmap(w, h))
            {
                Assert.Throws<ArgumentNullException>(() => BitmapExtension.CopyFromArray(null, a, r, g, b));
                Assert.Throws<ArgumentNullException>(() => o.CopyFromArray(null, r, g, b));
                Assert.Throws<ArgumentNullException>(() => o.CopyFromArray(a, null, g, b));
                Assert.Throws<ArgumentNullException>(() => o.CopyFromArray(a, r, null, b));
                Assert.Throws<ArgumentNullException>(() => o.CopyFromArray(a, r, g, null));

                Assert.Throws<ArgumentOutOfRangeException>(() => o.CopyFromArray(new byte[0, 0], r, g, b));
                Assert.Throws<ArgumentOutOfRangeException>(() => o.CopyFromArray(new byte[h, 0], r, g, b));
                Assert.Throws<ArgumentOutOfRangeException>(() => o.CopyFromArray(new byte[0, w], r, g, b));

                o.CopyFromArray(a, r, g, b);

                for (var y = 0; y < h; y++)
                {
                    for (var x = 0; x < w; x++)
                    {
                        var c = o.GetPixel(x, y);
                        Assert.AreEqual(a[y, x], c.A);
                        Assert.AreEqual(r[y, x], c.R);
                        Assert.AreEqual(g[y, x], c.G);
                        Assert.AreEqual(b[y, x], c.B);
                    }
                }
            }

            using (var o = BitmapTools.CreateGrayScaleBitmap(w, h))
            {
                Assert.Throws<ArgumentException>(() => o.CopyFromArray(a, r, g, b));
            }
            using (var o = BitmapTools.CreateRgbBitmap(w, h))
            {
                Assert.Throws<ArgumentException>(() => o.CopyFromArray(a, r, g, b));
            }
        }
    }
}