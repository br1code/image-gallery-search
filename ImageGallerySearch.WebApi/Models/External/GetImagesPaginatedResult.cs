using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ImageGallerySearch.WebApi.Models.External
{
    public class GetImagesPaginatedResult
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pageCount")]
        public int PageCount { get; set; }

        [JsonPropertyName("hasMore")]
        public bool HasMore { get; set; }
        
        [JsonPropertyName("pictures")]
        public List<Picture> Pictures { get; set; }
    }
}