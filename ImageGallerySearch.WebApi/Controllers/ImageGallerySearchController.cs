using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ImageGallerySearch.WebApi.Interfaces;
using ImageGallerySearch.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ImageGallerySearch.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageGallerySearchController : ControllerBase
    {
        private readonly IImageGalleryCacheService _imageGalleryCacheService;

        public ImageGallerySearchController(IImageGalleryCacheService imageGalleryCacheService)
        {
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
    }
}