using System.Net.Http.Json;
using StudiBase.Shared.Contracts.DTOs;
using StudiBase.Shared.Contracts.Paging;

namespace StudiBase.Shared.Clients
{
 public class CourseApiClient
 {
 private readonly HttpClient _http;
 public CourseApiClient(HttpClient http) => _http = http;

 public async Task<PagedResult<CourseDto>?> GetAsync(string? q, int page, int pageSize, CancellationToken ct = default)
 => await _http.GetFromJsonAsync<PagedResult<CourseDto>>($"api/courses?q={Uri.EscapeDataString(q ?? string.Empty)}&page={page}&pageSize={pageSize}", ct);

 public async Task<List<CourseDto>?> GetAllAsync(CancellationToken ct = default)
 => await _http.GetFromJsonAsync<List<CourseDto>>("api/courses/all", ct);

 public async Task<CourseDto?> GetByIdAsync(int id, CancellationToken ct = default)
 => await _http.GetFromJsonAsync<CourseDto>($"api/courses/{id}", ct);

 public async Task<CourseDto?> CreateAsync(CourseCreateDto dto, CancellationToken ct = default)
 {
 var res = await _http.PostAsJsonAsync("api/courses", dto, ct);
 res.EnsureSuccessStatusCode();
 return await res.Content.ReadFromJsonAsync<CourseDto>(cancellationToken: ct);
 }

 public async Task UpdateAsync(int id, CourseUpdateDto dto, CancellationToken ct = default)
 {
 var res = await _http.PutAsJsonAsync($"api/courses/{id}", dto, ct);
 res.EnsureSuccessStatusCode();
 }

 public async Task DeleteAsync(int id, CancellationToken ct = default)
 {
 var res = await _http.DeleteAsync($"api/courses/{id}", ct);
 res.EnsureSuccessStatusCode();
 }
 }
}
