using System.Text.RegularExpressions;

namespace TestPens.Extensions
{
    public class CommonExtentions
    {
        private static Regex YoutubeRegex = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:watch\?v=|shorts\/)|youtu\.be\/)([\w\-]{11})", RegexOptions.Compiled);
        private static Regex GoogleRegex = new Regex(@"(?:https?:\/\/)?(?:www\.)?drive\.google\.com\/(?:file\/d\/|open\?id=)([\w\-]+)", RegexOptions.Compiled);

        public static bool TryTransformToIframeUrl(string videoUrl, out string result)
        {
            if (string.IsNullOrWhiteSpace(videoUrl))
            {
                throw new NullReferenceException(nameof(videoUrl));
            }

            if (videoUrl.Contains("youtube.com") || videoUrl.Contains("youtu.be"))
            {
                var match = YoutubeRegex.Match(videoUrl);
                if (match.Success)
                {
                    result = $"https://www.youtube.com/embed/{match.Groups[1].Value}";
                    return true;
                }
            }

            if (videoUrl.Contains("drive.google.com"))
            {
                var match = GoogleRegex.Match(videoUrl);
                if (match.Success)
                {
                    result = $"https://drive.google.com/file/d/{match.Groups[1].Value}/preview";
                    return true;
                }
            }

            result = string.Empty;
            return false;
        }
    }
}
