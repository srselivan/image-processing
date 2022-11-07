using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace practiceIp.metrics
{
    internal class PSNR
    {
        protected static string rootPath = "C:\\Users\\Sergey\\source\\repos\\practiceIp\\resources\\";

        public PSNR(string imageName, string compareImageName)
        {
            Console.Out.WriteLine("PSNR: " + calculatePsnr(
                new Bitmap(rootPath + imageName),
                new Bitmap(rootPath + compareImageName)
                ));
        }

        private double calculatePsnr(Bitmap image, Bitmap compareImage)
        {
            double result;

            if (image.Size != compareImage.Size)
            {
                result = -1.0f;
                return result;
            }

            double mse = calculateMse(image, compareImage);
            result = (double)(20 * Math.Log10(255.0f / Math.Sqrt(mse)));
            return result;
        }

        private double calculateMse(Bitmap image, Bitmap compareImage)
        {
            double sumR = 0f;
            double sumG = 0f;
            double sumB = 0f;

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    sumR += (double)Math.Pow((int)compareImage.GetPixel(i, j).R - (int)image.GetPixel(i, j).R, 2);
                    sumG += (double)Math.Pow((int)compareImage.GetPixel(i, j).G - (int)image.GetPixel(i, j).G, 2);
                    sumB += (double)Math.Pow((int)compareImage.GetPixel(i, j).B - (int)image.GetPixel(i, j).B, 2);
                }
            }

            return (sumR + sumG + sumB) / (image.Width * image.Height) / 3;
        }
    }
}
