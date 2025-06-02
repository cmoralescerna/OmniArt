using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using OmniArt.EF_Data;
using OmniArt.Services;
using OmniArt.ViewModels;

namespace OmniArt
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp(sp => new App(sp)) // Use lambda to inject services into app
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Configure EF for an In-Memory Database
            // With the In-Memory Database, data is lost when the application shuts down.
            // There is no need for an external database (not allowed on this assignemnt!)
            // An In-Memory Database is designed for temporary usage!
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("OmniArtDb")); // Use In-Memory DB

            // Register Services
            builder.Services.AddSingleton<GalleryService>();
            builder.Services.AddSingleton<ArtService>();

            // Register ViewModels
            builder.Services.AddTransient<GalleryFormViewModel>();
            builder.Services.AddTransient<UploadArtViewModel>();
            builder.Services.AddTransient<ArtViewModel>();
            builder.Services.AddTransient<GalleriesViewModel>();
            builder.Services.AddTransient<GalleryViewModel>();
            builder.Services.AddTransient<HostViewModel>();
            builder.Services.AddTransient<ParticipantViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
