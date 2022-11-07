using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practiceIp.metrics
{
    internal class SSIM
    {
        protected static string rootPath = "C:\\Users\\Sergey\\source\\repos\\practiceIp\\resources\\";

        public SSIM(string imageName, string compareImageName)
        {
            Console.Out.WriteLine("SSIM: " + calculateSsim(
                new Bitmap(rootPath + imageName),
                new Bitmap(rootPath + compareImageName)
                ));
        }

        private double calculateSsim(Bitmap image, Bitmap compareImage)
        {
            double result;

            if (image.Size != compareImage.Size)
            {
                result = -1.0f;
                return result;
            }

            double k1 = 0.01f, k2 = 0.03f;
            double c1 = (double)Math.Pow(255f * k1, 2);
            double c2 = (double)Math.Pow(255f * k2, 2);

            double avgBrightX = calculateAverageBrightness(compareImage);
            double avgBrightY = calculateAverageBrightness(image);

            double disX = calculateDis(compareImage, avgBrightX);
            double disY = calculateDis(image, avgBrightY);

            double covXY = calculateCov(compareImage, avgBrightX, image, avgBrightY);

            result = (2 * avgBrightX * avgBrightY + c1) * (2 * covXY + c2) / (double)(Math.Pow(avgBrightX, 2) + Math.Pow(avgBrightY, 2) + c1)
                / (double)(Math.Pow(disX, 2) + Math.Pow(disY, 2) + c2); ;
            return result;
        }

        private double calculateDis(Bitmap image, double AvBr)
        {
            double sum = 0f;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    sum += (double)Math.Pow(GetBrightness(image.GetPixel(i, j)) - AvBr, 2);
                }
            }
            return (double)Math.Sqrt(sum / ((double)(image.Width * image.Height) - 1f));
        }

        private double calculateAverageBrightness(Bitmap image)
        {
            double sum = 0.0f;
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    sum += GetBrightness(image.GetPixel(i, j));
                }
            }
            return sum / (double)(image.Width * image.Height);
        }

        private double calculateCov(Bitmap image1, double m1, Bitmap image2, double m2)
        {
            double sum = 0f;
            for (int i = 0; i < image1.Width; i++)
            {
                for (int j = 0; j < image1.Height; j++)
                {
                    sum += (GetBrightness(image1.GetPixel(i, j)) - m1) *
                        (GetBrightness(image2.GetPixel(i, j)) - m2);
                }
            }
            return sum / ((double)(image1.Width * image1.Height) - 1f);
        }


        private byte GetBrightness(Color color)
        {
            return (byte)(.299 * color.R + .587 * color.G + .114 * color.B);
        }
    }
}
