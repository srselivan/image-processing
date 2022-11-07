using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practiceIp.filters
{
    internal class AdaptiveBinarization : Filter
    {
        public AdaptiveBinarization(string imagePath) : base(imagePath)
        {
        }

        protected override Bitmap ImageProcessing(Bitmap obj)
        {
            Bitmap resultImage = new Bitmap(obj.Width, obj.Height);

            int w_size = 3;

            int radiusX = w_size / 2;
            int radiusY = w_size / 2;

            double res2 = 0;
            double sum = 0;

            for (int i = 0; i < obj.Width; i++)
            {
                for (int j = 0; j < obj.Height; j++)
                {
                    double resColor1 = 0;
                    for (int l = -radiusY; l <= radiusY; l++)
                    {
                        for (int k = -radiusX; k <= radiusX; k++)
                        {
                            int idX = clamp(i + k, 0, obj.Width - 1);
                            int idY = clamp(j + l, 0, obj.Height - 1);

                            resColor1 += obj.GetPixel(idX, idY).R;
                        }
                    }
                    double res = resColor1 / (w_size * w_size);

                    for (int l = -radiusY; l <= radiusY; l++)
                    {
                        for (int k = -radiusX; k <= radiusX; k++)
                        {
                            int idX = clamp(i + k, 0, obj.Width - 1);
                            int idY = clamp(j + l, 0, obj.Height - 1);
                            sum += (obj.GetPixel(idX, idY).R - res) * (obj.GetPixel(idX, idY).R - res);

                        }
                    }
                    res2 = Math.Sqrt(sum / (w_size * w_size));

                    int T = (int)(res + (-0.2) * res2);
                    sum = 0;
                    Color sourceColor = obj.GetPixel(i, j);
                    int result;
                    if (sourceColor.R < T)
                    {
                        result = 0;
                    }
                    else
                    {
                        result = 255;
                    }
                    Color resultColor = Color.FromArgb(result, result, result);
                    resultImage.SetPixel(i, j, resultColor);
                }
            }

            return resultImage;
        }
    }
}
