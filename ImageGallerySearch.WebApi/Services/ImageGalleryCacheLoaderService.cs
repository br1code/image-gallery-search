using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using ImageGallerySearch.WebApi.Interfaces;
using ImageGallerySearch.WebApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ImageGallerySearch.WebApi.Services
{
    public class ImageGalleryCacheLoaderService : IHostedService
    {
        private readonly IImageGalleryCacheService _imageGalleryCacheService;
        private readonly ILogger<ImageGalleryCacheLoaderService> _logger;
        private readonly int _cacheLoaderInterval;
        private bool _keepRunning = true;
        private DateTime _lastTimeCacheUpdated;

        public ImageGalleryCacheLoaderService(IImageGalleryCacheService imageGalleryCacheService,
            ILogger<ImageGalleryCacheLoaderService> logger, IConfiguration configuration)
        {
            _imageGalleryCacheService = imageGalleryCacheService;
            _logger = logger;

            var settings = configuration.GetSection("ImageGallerySettings").Get<ImageGallerySettings>();
            _cacheLoaderInterval = settings.CacheLoaderInterval;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (_keepRunning)
            {
                await _imageGalleryCacheService.LoadCache();

                _lastTimeCacheUpdated = DateTime.Now;

                await Task.Delay(TimeSpan.FromMinutes(_cacheLoaderInterval), cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var stopTime = DateTime.Now.ToString(CultureInfo.CurrentCulture);
            var lastTime = _lastTimeCacheUpdated.ToString(CultureInfo.CurrentCulture);
            
            _logger.LogInformation($"Stopping Image Gallery Cache Load Background Task at {stopTime}. Last time updated: {lastTime}");

            _keepRunning = false;

            return Task.CompletedTask;
        }
    }
}