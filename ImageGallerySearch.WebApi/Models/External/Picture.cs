using System.Text.Json.Serialization;

namespace ImageGallerySearch.WebApi.Models.External
{
    public class Picture
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        [JsonPropertyName("cropped_picture")]
        public string CroppedPicture { get; set; }
    }
}