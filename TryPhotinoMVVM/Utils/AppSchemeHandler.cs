using System.Reflection;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace TryPhotinoMVVM.Utils;

public static class AppSchemeHandler
{
    public static readonly EmbeddedFileProvider FileProvider = new(typeof(Program).Assembly, "TryPhotinoMVVM.wwwroot");

    public static readonly string BuildDateHash = typeof(Program).Assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
        .InformationalVersion?.Split('-').Last() ?? "dev";

    public static Stream Handle(object sender, string scheme, string url, out string contentType)
    {
        var path = url.Replace("app://", "").Split('?')[0].TrimStart('/');

        var fileInfo = FileProvider.GetFileInfo(path);
        if (!fileInfo.Exists)
        {
            if (path == "index.html")
            {
                contentType = "text/plain";
                return new MemoryStream(Encoding.UTF8.GetBytes("404 Not Found"));
            }

            return Handle(sender, scheme, $"app://index.html?hash={BuildDateHash}", out contentType);
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
