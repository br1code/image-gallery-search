using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ImageGallerySearch.WebApi.Interfaces;
using ImageGallerySearch.WebApi.Models;
using ImageGallerySearch.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallerySearch.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageGallerySearchController : ControllerBase
    {
        private readonly IImageGalleryCacheService _imageGalleryCacheService;
        private readonly ImageGalleryCacheLoaderService _imageGalleryCacheLoaderService;

        public ImageGallerySearchController(ImageGalleryCacheLoaderService imageGalleryCacheLoaderService,
            IImageGalleryCacheService imageGalleryCacheService)
        {
            _imageGalleryCacheLoaderService = imageGalleryCacheLoaderService;
            _imageGalleryCacheService = imageGalleryCacheService;
        }

        [HttpGet("/search/{searchTerm}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ImageGallerySearchResult>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Search(string searchTerm)
        {
            try
            {
                return Ok(await _imageGalleryCacheService.Search(searchTerm));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("StopCacheReload")]
        public async Task<IActionResult> StopCacheReload()
        {
            await _imageGalleryCacheLoaderService.StopAsync(new CancellationToken());

            return Ok("Image Gallery Cache Loader stopped");
        }
    }
}