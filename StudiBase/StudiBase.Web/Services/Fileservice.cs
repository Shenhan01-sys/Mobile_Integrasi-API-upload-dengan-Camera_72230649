using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudiBase.Web.Services
{
    public interface IFileService
    {
        Task<FileSaveResult> SaveAsync(IFormFile file, IEnumerable<string> segments, CancellationToken ct = default);
        Task<bool> DeleteAsync(string? relativePath);
        bool TryGetAbsolutePath(string? relativePath, out string absolutePath);
        string GetContentType(string fileName);
    }

    public sealed record FileSaveResult(string RelativePath, string FileName, long Size, string ContentType);

    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly FileExtensionContentTypeProvider _contentTypeProvider = new();
        private const string UploadsFolder = "uploads";

        public FileService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<FileSaveResult> SaveAsync(IFormFile file, IEnumerable<string> segments, CancellationToken ct = default)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty", nameof(file));

            var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var directory = Path.Combine(new[] { root, UploadsFolder }.Concat(segments).ToArray());
            Directory.CreateDirectory(directory);

            var ext = Path.GetExtension(file.FileName);
            var safeName = $"{Guid.NewGuid():N}{ext}";
            var absolutePath = Path.Combine(directory, safeName);
            await using (var stream = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 64 * 1024, useAsync: true))
            {
                await file.CopyToAsync(stream, ct);
            }

            var relativePath = "/" + Path.Combine(new[] { UploadsFolder }.Concat(segments).Append(safeName).ToArray())
                .Replace("\\", "/");

            var contentType = GetContentType(safeName);
            return new FileSaveResult(relativePath, safeName, file.Length, contentType);
        }

        public async Task<bool> DeleteAsync(string? relativePath)
        {
            if (!TryGetAbsolutePath(relativePath, out var absolutePathDel)) return false;
            if (!File.Exists(absolutePathDel)) return false;
            try
            {
                await Task.Run(() => File.Delete(absolutePathDel));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TryGetAbsolutePath(string? relativePath, out string absolutePathGet)
        {
            absolutePathGet = string.Empty;
            if (string.IsNullOrWhiteSpace(relativePath)) return false;
            var root = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");

            var rel = relativePath.Replace("\\", "/");
            if (rel.StartsWith("~/")) rel = rel[1..];
            if (rel.StartsWith("/")) rel = rel[1..];

            // Ensure path stays within wwwroot
            var combined = Path.GetFullPath(Path.Combine(root, rel));
            if (!combined.StartsWith(root, StringComparison.OrdinalIgnoreCase)) return false;
            absolutePathGet = combined;
            return true;
        }

        public string GetContentType(string fileName)
        {
            if (_contentTypeProvider.TryGetContentType(fileName, out var contentType))
                return contentType;
            return "application/octet-stream";
        }
    }
}
