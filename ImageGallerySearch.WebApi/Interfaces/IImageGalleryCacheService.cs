using System.Collections.Generic;
using System.Threading.Tasks;
using ImageGallerySearch.WebApi.Models;

namespace ImageGallerySearch.WebApi.Interfaces
{
    public interface IImageGalleryCacheService
    {
        Task<List<ImageGallerySearchResult>> Search(string searchTerm);
        
        Task LoadCache();
    }
}