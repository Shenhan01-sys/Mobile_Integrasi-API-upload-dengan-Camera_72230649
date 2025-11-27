using System.Net.Http.Json;
using StudiBase.Shared.Contracts.DTOs;
using StudiBase.Shared.Contracts.Paging;

namespace StudiBase.Shared.Clients
{
 public class TrainerApiClient
 {
 private readonly HttpClient _http;
 public TrainerApiClient(HttpClient http) => _http = http;

 public async Task<PagedResult<TrainerDto>?> GetAsync(string? q, int page, int pageSize, CancellationToken ct = default)
 => await _http.GetFromJsonAsync<PagedResult<TrainerDto>>($"api/trainers?q={Uri.EscapeDataString(q ?? string.Empty)}&page={page}&pageSize={pageSize}", ct);

 public async Task<List<TrainerDto>?> GetAllAsync(CancellationToken ct = default)
 => await _http.GetFromJsonAsync<List<TrainerDto>>("api/trainers/all", ct);

 public async Task<TrainerDto?> GetByIdAsync(int id, CancellationToken ct = default)
 => await _http.GetFromJsonAsync<TrainerDto>($"api/trainers/{id}", ct);

 public async Task<TrainerDto?> CreateAsync(TrainerCreateDto dto, CancellationToken ct = default)
 {
 var res = await _http.PostAsJsonAsync("api/trainers", dto, ct);
 res.EnsureSuccessStatusCode();
 return await res.Content.ReadFromJsonAsync<TrainerDto>(cancellationToken: ct);
 }

 public async Task UpdateAsync(int id, TrainerUpdateDto dto, CancellationToken ct = default)
 {
 var res = await _http.PutAsJsonAsync($"api/trainers/{id}", dto, ct);
 res.EnsureSuccessStatusCode();
 }

 public async Task DeleteAsync(int id, CancellationToken ct = default)
 {
 var res = await _http.DeleteAsync($"api/trainers/{id}", ct);
 res.EnsureSuccessStatusCode();
 }
 }
}
