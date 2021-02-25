namespace ImageGallerySearch.WebApi.Models
{
    public class ImageGallerySettings
    {
        public int CacheLoaderInterval { get; set; }
        
        public string AuthUrl { get; set; }

        public string GetImagesUrl { get; set; }

        public string ApiKey { get; set; }
    }
}