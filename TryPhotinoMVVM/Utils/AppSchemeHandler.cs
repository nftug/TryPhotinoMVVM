using System.IO.Compression;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace TryPhotinoMVVM.Utils;

public static class AppSchemeHandler
{
    private static readonly Assembly _assembly = typeof(Program).Assembly;
    public static readonly EmbeddedFileProvider FileProvider = new(_assembly, $"{_assembly.GetName().Name}.wwwroot");

    public static Stream Handle(object sender, string scheme, string urlString, out string contentType)
    {
        var uri = new Uri(urlString);
        string path = uri.LocalPath;

        var fileInfo = FileProvider.GetFileInfo(path);
        if (!fileInfo.Exists)
        {
            var gzPath = path + ".gz";
            var gzFileInfo = FileProvider.GetFileInfo(gzPath);
            if (gzFileInfo.Exists)
            {
                path = gzPath;
                fileInfo = gzFileInfo;
            }
            else
            {
                if (path == "/index.html")
                {
                    contentType = "text/plain";
                    return new MemoryStream(Encoding.UTF8.GetBytes("404 Not Found"));
                }

                string indexUrl = $"{uri.Scheme}://{uri.Host}/index.html";
                return Handle(sender, scheme, indexUrl, out contentType);
            }
        }

        var extension = Path.GetExtension(path).ToLower();
        var actualExtension = extension == ".gz"
            ? Path.GetExtension(Path.GetFileNameWithoutExtension(path)).ToLower()
            : extension;

        contentType = actualExtension switch
        {
            ".html" => "text/html",
            ".js" => "application/javascript",
            ".css" => "text/css",
            ".json" => "application/json",
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };

        var stream = fileInfo.CreateReadStream();
        return extension == ".gz" ? new GZipStream(stream, CompressionMode.Decompress) : stream;
    }
}
