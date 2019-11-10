using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using NLog;

namespace DominantColoursSearch
{
    public static class Utility
    {
        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        public static BitmapSource ToBitmapSource(System.Drawing.Bitmap bitmap)
        {
            BitmapSource result = null;

            try
            {
                IntPtr ptr = bitmap.GetHbitmap();

                try
                {
                    result = Imaging.CreateBitmapSourceFromHBitmap(
                                 ptr,
                                 IntPtr.Zero,
                                 Int32Rect.Empty,
                                 BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(ptr);
                    bitmap.Dispose();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("ToBitmapSource");

                throw ex;
            }


            return result;
        }
    }
}
