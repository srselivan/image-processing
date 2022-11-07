using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practiceIp.filters
{
    internal class Grayscale : Filter
    {
        public Grayscale(string imagePath) : base(imagePath)
        {
        }

        protected override Bitmap ImageProcessing(Bitmap obj)
        {
            Bitmap result = new Bitmap(obj.Width, obj.Height);

            for (int i = 0; i < obj.Width; i++)
            {
                for (int j = 0; j < obj.Height; j++)
                {
                    Color color = obj.GetPixel(i, j);
                    byte intensity = (byte)(0.299f * color.R + 0.587f * color.G + 0.114f * color.B);
                    result.SetPixel(i, j, Color.FromArgb(intensity, intensity, intensity));
                }
            }
            return result;
        }
    }
}
