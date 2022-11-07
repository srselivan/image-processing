using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practiceIp.filters
{
    internal abstract class Filter : IProcesseble
    {
        protected static string rootPath = "C:\\Users\\Sergey\\source\\repos\\practiceIp\\resources\\";
        protected string imageName;

        public Filter(string imagePath)
        {
            imageName = imagePath;
        }

        public void Processing()
        {
            Bitmap image = new Bitmap(rootPath + imageName);
            ImageProcessing(image).Save(rootPath + $"{TypeDescriptor.GetClassName(this)}.jpg");
            SeeImage(rootPath + $"{TypeDescriptor.GetClassName(this)}.jpg");
        }

        protected abstract Bitmap ImageProcessing(Bitmap obj);

        protected void SeeImage(string cmd)
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

        protected static int clamp(int value, int min, int max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }
    }
}
