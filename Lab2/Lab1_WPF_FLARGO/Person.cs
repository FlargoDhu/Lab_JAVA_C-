using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Lab2
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public BitmapSource PicturePath { get; set; }
        public string ImageName { get; set; }
        public Bitmap image;
    }
}
