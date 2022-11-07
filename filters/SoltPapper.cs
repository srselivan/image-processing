using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace practiceIp.filters
{
    internal class SoltPapper : Filter
    {
        public SoltPapper(string imagePath) : base(imagePath)
        {
        }

        protected override Bitmap ImageProcessing(Bitmap obj)
        {
            Bitmap result = new Bitmap(obj.Width, obj.Height);
            Random rnd = new Random();

            for (int i = 0; i < obj.Width; i++)
            {
                for (int j = 0; j < obj.Height; j++)
                {
                    double tmp = rnd.NextDouble();

                    if (tmp < 0.9)
                    {
                        result.SetPixel(i, j, obj.GetPixel(i, j));
                    }
                    else if (tmp > 0.95)
                    {
                        result.SetPixel(i, j, Color.Black);
                    }
                    else
                    {
                        result.SetPixel(i, j, Color.White);
                    }
                }
            }

            return result;
        }
    }
}
