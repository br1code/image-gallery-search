using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ImageGallerySearch.WebApi.Interfaces;
using ImageGallerySearch.WebApi.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace ImageGallerySearch.WebApi.Services
{
    public class ImageGalleryCacheService : IImageGalleryCacheService
    {
        private readonly string IMAGES_CACHE_KEY = "IMAGES";
        private readonly IMemoryCache _imageGalleryCache;
        private readonly IImageGalleryClient _imageGalleryClient;
        private readonly ILogger<ImageGalleryCacheService> _logger;

        public ImageGalleryCacheService(IMemoryCache imageGalleryCache, IImageGalleryClient imageGalleryClient,
            ILogger<ImageGalleryCacheService> logger)
        {
            _imageGalleryCache = imageGalleryCache;
            _imageGalleryClient = imageGalleryClient;
            _logger = logger;
        }

        public async Task<List<ImageGallerySearchResult>> Search(string searchTerm)
        {
            if (!_imageGalleryCache.TryGetValue(IMAGES_CACHE_KEY, out List<ImageGallerySearchResult> images))
            {
                await LoadCache();

                return await Search(searchTerm);
            }

            return images.Where(GetSearchAlgorithm(searchTerm)).ToList();
        }

        public async Task LoadCache()
        {
            var images = new List<ImageGallerySearchResult>();
            var pageNumber = 1;
            var allPagesWereProcessed = false;

            _logger.LogInformation("Starting to fetch and store images to the Image Gallery Local Cache at " +
                                   DateTime.Now.ToString(CultureInfo.InvariantCulture));

            while (!allPagesWereProcessed)
            {
                _logger.LogInformation($"Processing page number {pageNumber}");
                
                var page = await _imageGalleryClient.GetImagesByPage(pageNumber);

                pageNumber++;

                if (!page.HasMore)
                {
                    allPagesWereProcessed = true;
                }

                foreach (var picture in page.Pictures)
                {
                    var image = await _imageGalleryClient.GetImageById(picture.Id);
                    
                    _logger.LogInformation($"Saving image with id {image.Id} to local cache.");

                    images.Add(new ImageGallerySearchResult
                    {
                        Id = image.Id,
                        Author = image.Author,
                        Camera = image.Camera,
                        CroppedPicture = image.CroppedPicture,
                        FullPicture = image.FullPicture,
                        Tags = image.Tags.Trim().Split(" ").ToList()
                    });
                }

                _imageGalleryCache.Set(IMAGES_CACHE_KEY, images);
            }
            
            _logger.LogInformation("The Image Gallery Local Cache finished updating at " +
                                   DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        // This is the best "search algorithm" I could come up with in this amount of time
        private static Func<ImageGallerySearchResult, bool> GetSearchAlgorithm(string searchTerm)
        {
            var searchTerms = searchTerm.Trim().Split(" ");

            return image =>
                (image.Author?.Split(" ").Intersect(searchTerms, StringComparer.CurrentCultureIgnoreCase).Any() ?? false) ||
                (image.Camera?.Split(" ").Intersect(searchTerms, StringComparer.CurrentCultureIgnoreCase).Any() ?? false) ||
                (image.Tags?.Intersect(searchTerms, StringComparer.CurrentCultureIgnoreCase).Any() ?? false);
        }
    }
}