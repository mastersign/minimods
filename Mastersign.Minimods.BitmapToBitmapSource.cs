﻿#region MiniMod
// <MiniMod>
//   <Name>Bitmap to BitmapSource</Name>
//   <Author>Micheal Eakins</Author>
//   <LastChanged>2015-01-21</LastChanged>
//   <Version>1.1.0</Version>
//   <Url>https://gist.github.com/mastersign/8d81d5be4f7b31e0cf9a/raw/Mastersign.Minimods.BitmapToBitmapSource.cs</Url>
//   <Description>
//     Conversion from System.Drawing.Bitmap to System.Windows.Media.Imaging.BitmapSource.
//   </Description>
// </MiniMod>
#endregion

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Mastersign.Minimods.BitmapToBitmapSource
{
    /// <summary>
    /// <para>
    /// Contains extension methods for converting a GDI+ <see cref="System.Drawing.Bitmap"/> 
    /// into a <see cref="BitmapSource"/>.
    /// </para>
    /// <para>Author: Micheal Eakins http://stackoverflow.com/users/437301/michael-eakins </para>
    /// <para>Source: http://stackoverflow.com/questions/94456/load-a-wpf-bitmapimage-from-a-system-drawing-bitmap </para>
    /// </summary>
    public static class BitmapToBitmapSourceExtension
    {
        /// <summary>
        /// Converts a <see cref="System.Drawing.Image"/> into a WPF <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="image">The image image.</param>
        /// <returns>A BitmapSource</returns>
        public static BitmapSource ToBitmapSource(this System.Drawing.Image image)
        {
            using (var bitmap = new System.Drawing.Bitmap(image))
            {
                return bitmap.ToBitmapSource();
            }
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Bitmap"/> into a WPF <see cref="BitmapSource"/>.
        /// </summary>
        /// <remarks>Uses GDI to do the conversion. Hence the call to the marshalled DeleteObject.
        /// </remarks>
        /// <param name="bitmap">The bitmap bitmap.</param>
        /// <returns>A BitmapSource</returns>
        public static BitmapSource ToBitmapSource(this System.Drawing.Bitmap bitmap)
        {
            BitmapSource bitSrc = null;

            var hBitmap = bitmap.GetHbitmap();

            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
                NativeMethods.DeleteObject(hBitmap);
            }

            return bitSrc;
        }
    }

    internal static class NativeMethods
    {
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);
    }
}