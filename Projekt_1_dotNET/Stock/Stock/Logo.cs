using StockApp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace StockApp
{
    class Logo
    {

        
        public Image randomImage;
        public String Topic { get; set; }

        public ImageSource GetLogo()
        {
            var bitmap = new Bitmap(randomImage);
            return ImageSourceForBitmap(bitmap);
        }

        private void FindSomeImage(string rand)
        {

            while (true)
            {
                try
                {
                    var _img = new RandomImage(rand).ToBitmap();
                    if (_img != null)
                    {
                        randomImage = _img;
                        break;
                    }

                }
                catch (Exception) { }

           }
        }
        
        public  void FindLogo()
        {

            try
            {
 
                 FindSomeImage(Topic+"+ company logo");       

            }
            catch (Exception ex)
            {
                MessageBox.Show("RandomImage asyns error:" + ex.Message);
            }

        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

    }
}
