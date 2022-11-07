using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practiceIp.filters
{
    internal class Binarization : Filter
    {
        private int border = 127;
        
        public Binarization(string imagePath) : base(imagePath)
        {
        }

        protected override Bitmap ImageProcessing(Bitmap obj)
        {
            Bitmap result = new Bitmap(obj.Width, obj.Height);

            Color bColor = Color.Black;
            Color lColor = Color.White;

            for (int i = 0; i < obj.Width; i++)
            {
                for (int j = 0; j < obj.Height; j++)
                {
                    Color color = obj.GetPixel(i, j);
                    if (color.R > border)
                    {
                        result.SetPixel(i, j, lColor);
                    }
                    else
                    {
                        result.SetPixel(i, j, bColor);
                    }

                }
            }

            return result;
        }
    }
}
