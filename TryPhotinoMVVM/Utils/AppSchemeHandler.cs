using System.Text;
using Microsoft.Extensions.FileProviders;

namespace TryPhotinoMVVM.Utils;

public static class AppSchemeHandler
{
    public static readonly EmbeddedFileProvider FileProvider = new(typeof(Program).Assembly, "TryPhotinoMVVM.wwwroot");

    public static Stream Handle(object sender, string scheme, string urlString, out string contentType)
    {
        var uri = new Uri(urlString);
        string path = uri.LocalPath;

        var fileInfo = FileProvider.GetFileInfo(path);
        if (!fileInfo.Exists)
        {
            if (path == "/index.html")
            {
                contentType = "text/plain";
                return new MemoryStream(Encoding.UTF8.GetBytes("404 Not Found"));
            }

            string indexUrl = $"{uri.Scheme}://{uri.Host}/index.html";
            return Handle(sender, scheme, indexUrl, out contentType);
        }

        contentType = Path.GetExtension(path).ToLower() switch
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

        return fileInfo.CreateReadStream();
    }
}
