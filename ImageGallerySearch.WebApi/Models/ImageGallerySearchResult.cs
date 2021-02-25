using System.Collections.Generic;

namespace ImageGallerySearch.WebApi.Models
{
    public class ImageGallerySearchResult
    {
        public string Id { get; set; }
        
        public string Author { get; set; }
        
        public string Camera { get; set; }
        
        public string CroppedPicture { get; set; }
        
        public string FullPicture { get; set; }
        
        public List<string> Tags { get; set; }
    }
}