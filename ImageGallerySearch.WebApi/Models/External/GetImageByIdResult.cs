using System.Text.Json.Serialization;

namespace ImageGallerySearch.WebApi.Models.External
{
    public class GetImageByIdResult
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("author")]
        public string Author { get; set; }

        [JsonPropertyName("camera")]
        public string Camera { get; set; }

        [JsonPropertyName("tags")]
        public string Tags { get; set; }
        
        [JsonPropertyName("cropped_picture")]
        public string CroppedPicture { get; set; }

        [JsonPropertyName("full_picture")]
        public string FullPicture { get; set; }
    }
}