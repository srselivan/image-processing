using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practiceIp.filters
{
    internal class MedianFilter : Filter
    {
        public MedianFilter(string imagePath) : base(imagePath)
        {
        }

        protected override Bitmap ImageProcessing(Bitmap obj)
        {
            Bitmap resultImage = new Bitmap(obj.Width, obj.Height);

            int w_size = 3;

            int radiusX = w_size / 2;
            int radiusY = w_size / 2;

            for (int i = 0; i < obj.Width; i++)
            {
                for (int j = 0; j < obj.Height; j++)
                {
                    ArrayList w_colors = new ArrayList();
                    Color resColor = new Color();

                    for (int l = -radiusY; l <= radiusY; l++)
                    {
                        for (int k = -radiusX; k <= radiusX; k++)
                        {
                            int idX = clamp(i + k, 0, obj.Width - 1);
                            int idY = clamp(j + l, 0, obj.Height - 1);

                            w_colors.Add(obj.GetPixel(idX, idY).ToArgb());
                        }
                    }

                    w_colors.Sort();
                    
                    if(w_colors.Count%2 == 0)
                    {
                        resColor = Color.FromArgb(((int)w_colors[(w_colors.Count / 2) - 1] + (int)w_colors[w_colors.Count / 2]) / 2);
                    }
                    else
                    {
                        resColor = Color.FromArgb((int)w_colors[w_colors.Count / 2]);
                    }

                    resultImage.SetPixel(i, j, resColor);
                }
            }

            return resultImage;
        }
    }
}
