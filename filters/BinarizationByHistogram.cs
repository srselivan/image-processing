using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practiceIp.filters
{
    internal class BinarizationByHistogram : Filter
    {
        public BinarizationByHistogram(string imagePath) : base(imagePath)
        {
        }

        protected override Bitmap ImageProcessing(Bitmap obj)
        {
            Bitmap resultImage = new Bitmap(obj.Width, obj.Height);

            int[] hist = calcIntencity(obj);
            int pixelSum = obj.Width * obj.Height;

            float fivePercentCut = pixelSum * 0.05f;

            for (int i = 0; i < 255; i++)
            {
                if (hist[i] < fivePercentCut)
                {
                    fivePercentCut -= hist[i];
                    hist[i] = 0;
                }
                else
                {
                    hist[i] -= (int)fivePercentCut;
                }
                if (fivePercentCut == 0) break;
            }

            fivePercentCut = pixelSum * 0.05f;

            for (int i = 255; i > 0; i--)
            {
                if (hist[i] < fivePercentCut)
                {
                    fivePercentCut -= hist[i];
                    hist[i] = 0;
                }
                else
                {
                    hist[i] -= (int)fivePercentCut;
                }
                if (fivePercentCut == 0) break;
            }


            int threshold = 0;
            for (int i = 0; i < 255; i++)
            {
                threshold += hist[i] * i;
            }

            threshold = (int)(threshold / hist.Sum());

            for (int i = 0; i < obj.Width; i++)
            {
                for (int j = 0; j < obj.Height; j++)
                {
                    Color sourceColor = obj.GetPixel(i, j);
                    int result;
                    if (sourceColor.R < threshold)
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

        private int[] calcIntencity(Bitmap obj)
        {
            int[] result = new int[256];

            for (int i = 0; i < 256; i++)
            {
                result[i] = 0;
            }

            for (int i = 0; i < obj.Width; ++i)
            {
                for (int j = 0; j < obj.Height; ++j)
                {
                    result[obj.GetPixel(i, j).R]++;
                }
            }

            return result;
        }
    }
}
