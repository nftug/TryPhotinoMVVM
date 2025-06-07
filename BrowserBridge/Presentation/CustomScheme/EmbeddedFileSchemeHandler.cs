using System.IO.Compression;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.FileProviders;

namespace BrowserBridge;

public static class EmbeddedFileSchemeHandler
{
    private static readonly Assembly _assembly = Assembly.GetEntryAssembly()!;
    public static readonly EmbeddedFileProvider FileProvider = new(_assembly, $"{_assembly.GetName().Name}");

    public static CustomSchemeHandlerResult GetEmbeddedFile(Uri uri, string rootPath)
    {
        var path = Path.Combine(rootPath, uri.LocalPath.TrimStart('/'));

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
                    return new(new MemoryStream(Encoding.UTF8.GetBytes("404 Not Found")), "text/plain");
                }

                var indexUri = new Uri($"{uri.Scheme}://{uri.Host}/index.html");
                return GetEmbeddedFile(indexUri, rootPath);
            }
        }

        var extension = Path.GetExtension(path);
        var actualExtension = extension == ".gz"
            ? Path.GetExtension(Path.GetFileNameWithoutExtension(path))
            : extension;
        var contentType = CustomSchemeUtils.GetContentType(actualExtension);

        var stream = fileInfo.CreateReadStream();
        return new(extension == ".gz" ? new GZipStream(stream, CompressionMode.Decompress) : stream, contentType);
    }
}
