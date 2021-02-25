using System.Threading.Tasks;
using ImageGallerySearch.WebApi.Models.External;

namespace ImageGallerySearch.WebApi.Interfaces
{
    public interface IImageGalleryClient
    {
        Task<GetImagesPaginatedResult> GetImagesByPage(int pageNumber);

        Task<GetImageByIdResult> GetImageById(string id);
    }
}