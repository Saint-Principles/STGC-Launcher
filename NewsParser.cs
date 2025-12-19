using System.Drawing;

namespace STGCLauncher
{
    public static class NewsParser
    {
        public static NewsData Parse(string text, Image image)
        {
            var news = new NewsData { Image = image };

            if (string.IsNullOrEmpty(text))
            {
                news.Title = "News";
                news.Body = "No news available.";
                return news;
            }

            int newLineIndex = text.IndexOf('\n');

            if (newLineIndex <= 0)
            {
                news.Title = text;
                news.Body = string.Empty;
            }
            else
            {
                news.Title = text.Substring(0, newLineIndex).TrimEnd('\n');
                news.Body = text.Substring(newLineIndex + 1).Trim();
            }

            return news;
        }
    }
}
