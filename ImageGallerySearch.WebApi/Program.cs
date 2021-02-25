using ImageGallerySearch.WebApi.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ImageGallerySearch.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => { logging.AddConsole(); })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureServices(collection =>
                {
                    // we register the hosted service as a singleton first so that there is only one instance, being able to stop it at any time
                    collection.AddSingleton<ImageGalleryCacheLoaderService>();
                    collection.AddHostedService(provider => provider.GetService<ImageGalleryCacheLoaderService>());
                });
    }
}