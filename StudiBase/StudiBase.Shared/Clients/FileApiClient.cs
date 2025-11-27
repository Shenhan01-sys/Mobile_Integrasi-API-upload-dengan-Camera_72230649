using System.Net.Http.Headers;
using System.Net.Http.Json;
using StudiBase.Shared.Contracts.DTOs;

namespace StudiBase.Shared.Clients;

public class FileApiClient
{
    private readonly HttpClient _http;
    public FileApiClient(HttpClient http) => _http = http;

    public async Task<string?> UploadCourseThumbnailAsync(int courseId, Stream fileStream, string fileName, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", fileName);
        var res = await _http.PostAsync($"api/courses/{courseId}/thumbnail", content, ct);
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadFromJsonAsync<Dictionary<string, object>>(cancellationToken: ct);
        return json != null && json.TryGetValue("path", out var p) ? p?.ToString() : null;
    }

    public async Task<CourseMaterialDto?> UploadCourseMaterialAsync(int courseId, int trainerId, Stream fileStream, string fileName, string title, string? description, string fileType, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        content.Add(fileContent, "file", fileName);
        content.Add(new StringContent(title), "Title");
        if (!string.IsNullOrWhiteSpace(description)) content.Add(new StringContent(description), "Description");
        content.Add(new StringContent(trainerId.ToString()), "UploadedByTrainerId");
        content.Add(new StringContent(fileType), "fileType");
        var res = await _http.PostAsync($"api/courses/{courseId}/materials", content, ct);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<CourseMaterialDto>(cancellationToken: ct);
    }

    public async Task DeleteCourseMaterialAsync(int courseId, int materialId, CancellationToken ct = default)
    {
        var res = await _http.DeleteAsync($"api/courses/{courseId}/materials/{materialId}", ct);
        res.EnsureSuccessStatusCode();
    }

    // Tambahkan method ini di dalam class FileApiClient
    public async Task<string?> UploadTrainerPhotoAsync(int trainerId, Stream fileStream, string fileName, CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // Asumsi hasil kamera JPEG

        // Parameter "file" ini harus sesuai dengan [FromForm] IFormFile file di Controller
        content.Add(fileContent, "file", fileName);

        // Panggil endpoint yang sudah ada di TrainerController
        var res = await _http.PostAsync($"api/trainers/{trainerId}/photo", content, ct);
        res.EnsureSuccessStatusCode();

        var json = await res.Content.ReadFromJsonAsync<Dictionary<string, object>>(cancellationToken: ct);
        return json != null && json.TryGetValue("path", out var p) ? p?.ToString() : null;
    }

    public async Task<List<CourseMaterialDto>?> GetCourseMaterialsAsync(int courseId, CancellationToken ct = default)
    => await _http.GetFromJsonAsync<List<CourseMaterialDto>>($"api/courses/{courseId}/materials", ct);
}
