using System;
using System.Drawing;

namespace STGCLauncher
{
    public class DownloadResult
    {
        public bool Success { get; set; }
        public Exception Error { get; set; }
        public bool WasCancelled { get; set; }
    }

    public class NewsData
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public Image Image { get; set; }
    }
}