using Petecat.Formatter.Attribute;

namespace Petecat.App.Url
{
    public class ReplacementConfiguration
    {
        [JsonProperty(Alias = "ignoreFolders")]
        public string[] IgnoreFolders { get; set; }

        [JsonProperty(Alias = "fileExtensions")]
        public string[] FileExtensions { get; set; }
    }
}
