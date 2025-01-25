using System.Text.RegularExpressions;

namespace TestPens.Extensions
{
    public static class CommonExtentions
    {
        private static readonly Regex YoutubeRegex = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:watch\?v=|shorts\/)|youtu\.be\/)([\w\-]{11})", RegexOptions.Compiled);
        private static readonly Regex GoogleRegex = new Regex(@"(?:https?:\/\/)?(?:www\.)?drive\.google\.com\/(?:file\/d\/|open\?id=)([\w\-]+)", RegexOptions.Compiled);

        public static string TransformToIframeUrl(this string videoUrl)
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
                    return $"https://www.youtube.com/embed/{match.Groups[1].Value}";
                }
            }

            if (videoUrl.Contains("drive.google.com"))
            {
                var match = GoogleRegex.Match(videoUrl);
                if (match.Success)
                {
                    return $"https://drive.google.com/file/d/{match.Groups[1].Value}/preview";
                }
            }

            return videoUrl;
        }
    }
}
