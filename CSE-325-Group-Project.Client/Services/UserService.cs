using CSE325project.Shared;
using System.Net.Http.Json;

namespace CSE325project.Client.Services;

public class UserService
{
    private readonly HttpClient _http;

    public UserService(HttpClient http)
    {
        _http = http;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var response = await _http.PostAsJsonAsync("api/users", user);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>() ?? throw new InvalidOperationException("Failed to read created user.");
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        try
        {
            return await _http.GetFromJsonAsync<User>($"api/users/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<UserListDto>> GetAllUsersAsync()
    {
        return await _http.GetFromJsonAsync<List<UserListDto>>("api/users")
            ?? new List<UserListDto>();
    }

    public async Task UpdateUserAsync(Guid id, User user)
    {
        var response = await _http.PutAsJsonAsync($"api/users/{id}", user);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var response = await _http.DeleteAsync($"api/users/{id}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<int> GetUserCountAsync()
    {
        var response = await _http.GetAsync("api/users/count");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }
}