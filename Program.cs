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
            filter = new GaussianNoise("grayscale.jpg");
        }

        public void StartProcessing()
        {
            filter.Processing();
        }

    }
}
