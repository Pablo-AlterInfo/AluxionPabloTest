using System.Collections.Generic;
using System.IO;

namespace AluxionTest.Models.Internal
{
    public class UnsplashResult
    {
        public List<UnsplashImageItem> Images { get; set; }
    }
    public class UnsplashImageItem
    {
        public string id { get; set; }
        public string alt_description { get; set; }
        public UnsplashImageUrls urls { get; set; }
    }
    public class UnsplashImageUrls
    {
        public string raw { get; set; }
        public string full { get; set; }
        public string regular { get; set; }
        public string small { get; set; }
        public string thumb { get; set; }
        public string small_s3 { get; set; }
    }
    public class UnsplashDownloadUrl
    {
        public string url { get; set; }
    }
    public class StreamData
    {
        public string Id { get; set; }
        public Stream StreamInfo { get; set; }
    }
}
