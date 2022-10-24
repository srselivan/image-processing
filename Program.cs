using System.Collections;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

practiceIp.Ip ip = new practiceIp.Ip();

namespace practiceIp
{
    public class Ip
    {
        Bitmap img;
        Bitmap imgGrayscale;
        string path = "C:\\Users\\Sergey\\source\\repos\\practiceIp\\resources\\";

        public Ip()
        {
            img = new Bitmap(path + "example.jpg");
            imgGrayscale = new Bitmap(path + "grayscale.jpg");

            //grayscale(img).Save(path + "grayscale.jpg");
            //average(img).Save(path + "average.jpg");
            //autocontrast(img).Save(path + "autocontrast.jpg");
            //binarization(imgGrayscale, 127).Save(path + "binarization.jpg");
            //adaptiveBinarization(imgGrayscale).Save(path + "adaptiveBinarization.jpg");
            binarizationByHistogram(imgGrayscale).Save(path + "binarizationByHistogram.jpg");
            openImage(path + "binarizationByHistogram.jpg");
        }

        private void openImage(string cmd)
        {
            var proc = new ProcessStartInfo()
            {
                UseShellExecute = true,
                WorkingDirectory = @"C:\Windows\System32",
                FileName = @"C:\Windows\System32\cmd.exe",
                Arguments = "/c " + cmd,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process.Start(proc);
        }

        public Bitmap grayscale(Bitmap obj)
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

        private int clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
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

        public Bitmap average(Bitmap obj)
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

        private int[] calcIntencity(Bitmap obj)
        {
            int[] result = new int[256];

            for(int i = 0; i < 256; i++)
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

       /* public Bitmap GaussianNoise(Bitmap obj)
        {
            Bitmap resultIm = new Bitmap(obj.Width, obj.Height);

            int bytes = obj.Width * obj.Height;
            Random rnd = new Random();
            int[] intensety = new int[256];
            byte[] noise = new byte[bytes];
            double[] gaussian = new double[256];
            intensety = calcIntencity(obj);

            int sigma = 20;
            int mi = 0;
            double sum = 0;

            for(int i = 0; i < 256; i++)
            {
                gaussian[i] = (double)(1 / (sigma * Math.Sqrt(2 * Math.PI)) * Math.Exp(-(Math.Pow(intensety[i] - mi, 2) / (2 * Math.Pow(sigma, 2)))));
                sum += gaussian[i];
            }

            for (int i = 0; i < 256; i++)
            {
                gaussian[i] /= sum;
                gaussian[i] *= bytes;
                gaussian[i] = (int)Math.Floor(gaussian[i]);
            }

            int count = 0;
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < (int)gaussian[i]; j++)
                {
                    noise[j + count] = (byte)i;
                }
                count += (int)gaussian[i];
            }

            for (int i = 0; i < bytes - count; i++)
            {
                noise[count + i] = 0;
            }

            noise = noise.OrderBy(x => rnd.Next()).ToArray();

            for (int i = 0; i < bytes; i++)
            {
                result[i] = (byte)(buffer[i] + noise[i]);
            }

            
            
            return resultIm;
        }*/

        public Bitmap binarization (Bitmap obj, int border)
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

        public Bitmap adaptiveBinarization(Bitmap obj)
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

        public Bitmap binarizationByHistogram(Bitmap obj)
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

        private Color calculatePixelSigma(Bitmap obj, int x, int y)
        {
            Color avColor = calculateNewPixelColor(obj, x, y);

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
                    Color newColor = dec(obj.GetPixel(x, y), avColor);
                    Color resultColor = Color.FromArgb(
                        clamp(newColor.R * newColor.R, 0, 255),
                        clamp(newColor.G * newColor.G, 0, 255),
                        clamp(newColor.B * newColor.B, 0, 255)
                    );

                    sumR += resultColor.R;
                    sumG += resultColor.G;
                    sumB += resultColor.B;
                }

           
            return Color.FromArgb((int)Math.Sqrt((double)sumR), (int)Math.Sqrt((double)sumG), (int)Math.Sqrt((double)sumB));
        }

        private Color dec (Color color1, Color color2)
        {
            return Color.FromArgb(
                    clamp(color1.R - color2.R, 0, 255),
                    clamp(color1.G - color2.G, 0, 255),
                    clamp(color1.B - color2.B, 0, 255)
                    );
        }

        public Bitmap autocontrast(Bitmap obj)
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
