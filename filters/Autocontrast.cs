using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practiceIp.filters
{
    internal class Autocontrast : Filter
    {
        public Autocontrast(string imagePath) : base(imagePath)
        {
        }

        protected override Bitmap ImageProcessing(Bitmap obj)
        {
            Bitmap result = new Bitmap(obj.Width, obj.Height);

            int minR = 300, maxR = -1;
            int minG = 300, maxG = -1;
            int minB = 300, maxB = -1;

            for (int i = 0; i < obj.Width; i++)
            {
                for (int j = 0; j < obj.Height; j++)
                {
                    Color color = obj.GetPixel(i, j);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;

                    if (R > maxR) maxR = R;
                    else if (R < minR) minR = R;

                    if (G > maxG) maxG = G;
                    else if (G < minG) minG = G;

                    if (B > maxB) maxB = B;
                    else if (B < minB) minB = B;
                }
            }

            for (int i = 0; i < obj.Width; ++i)
            {
                for (int j = 0; j < obj.Height; ++j)
                {
                    Color color = obj.GetPixel(i, j);
                    result.SetPixel(i, j, Color.FromArgb(
                        (int)((color.R - minR) * (255f / (maxR - minR))),
                        (int)((color.G - minG) * (255f / (maxG - minG))),
                        (int)((color.B - minB) * (255f / (maxB - minB)))
                        ));
                }
            }

            return result;
        }
    }
}
