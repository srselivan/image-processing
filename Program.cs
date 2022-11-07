using practiceIp.filters;

practiceIp.Ip ip = new practiceIp.Ip();
ip.StartProcessing();

namespace practiceIp
{
    public class Ip
    {
        private IProcesseble filter;

        public Ip()
        {
            filter = new MedianFilter("practiceIp.filters.SoltPapper.jpg");
        }

        public void StartProcessing()
        {
            filter.Processing();
            StartMetrics();
        }

        private void StartMetrics()
        {
            new metrics.PSNR("practiceIp.filters.MedianFilter.jpg", "example.jpg");
            new metrics.SSIM("practiceIp.filters.MedianFilter.jpg", "example.jpg");
        }
    }
}
