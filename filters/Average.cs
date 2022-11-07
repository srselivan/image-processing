using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practiceIp.filters
{
    internal class Average : Filter
    {
        public Average(string imagePath) : base(imagePath)
        {
        }

        protected override Bitmap ImageProcessing(Bitmap obj)
        {
            Bitmap result = new Bitmap(obj.Width, obj.Height);

            for (int i = 0; i < obj.Width; ++i)
            {
                for (int j = 0; j < obj.Height; ++j)
                {
                    result.SetPixel(i, j, calculateNewPixelColor(obj, i, j));
                }
            }

            return result;
        }

        private Color calculateNewPixelColor(Bitmap obj, int x, int y)
        {
            int radiusX = 1;
            int radiusY = 1;
            int sumR = 0;
            int sumG = 0;
            int sumB = 0;

            for (int l = -radiusY; l <= radiusY; l++)
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = clamp(x + k, 0, obj.Width - 1);
                    int idY = clamp(y + l, 0, obj.Height - 1);
                    Color neighborColor = obj.GetPixel(idX, idY);
                    sumR += neighborColor.R;
                    sumG += neighborColor.G;
                    sumB += neighborColor.B;
                }

            return Color.FromArgb(sumR / 9, sumG / 9, sumB / 9);
        }
    }
}
