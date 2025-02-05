using System.Runtime.InteropServices;
using DinkToPdf;
using DinkToPdf.Contracts;
using TodoApp.Shared.Helpers;

namespace TodoApp.Infrastructure.Services; // ✅ Adjust namespace based on your project

public static class PdfServiceRegistration
{
    public static IServiceCollection AddPdfService(this IServiceCollection services)
    {
        var osPlatform = RuntimeInformation.OSDescription.ToLower();
        var context = new CustomAssemblyLoadContext();

        if (osPlatform.Contains("windows"))
        {
            // ✅ Load Windows native library
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.dll"));
        }
        else if (osPlatform.Contains("linux"))
        {
            // ✅ Load Linux native library
            context.LoadUnmanagedLibrary(Path.Combine(Directory.GetCurrentDirectory(), "libwkhtmltox.so"));
        }

        // ✅ Register the PDF converter as a singleton service
        services.AddSingleton<IConverter, SynchronizedConverter>(provider =>
            new SynchronizedConverter(new PdfTools()));

        return services;
    }
}
