using System.Drawing;

practiceIp.Ip ip = new practiceIp.Ip();

namespace practiceIp
{
    public class Ip
    {
        Bitmap img;
        string path = "C:\\Users\\Sergey\\source\\repos\\practiceIp\\resources\\";

        public Ip()
        {
            img = new Bitmap(path + "example.jpg");

            grayscale(img).Save(path + "grayscale.jpg");
            average(img).Save(path + "average.jpg");
            autocontrast(img).Save(path + "autocontrast.jpg");
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
