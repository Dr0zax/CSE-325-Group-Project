using System.Net.Http.Json;
using CSE325project.Shared;

namespace CSE325project.Client.Services;
public class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<User>> GetUsersAsync()
    {
        var users = await _httpClient.GetFromJsonAsync<List<User>>("api/users");
        return users ?? new List<User>();
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        var user = await _httpClient.GetFromJsonAsync<User>($"api/users/{userId}");
        return user;
    }

    public async Task<User> AddUserAsync(User user)
    {
        var response = await _httpClient.PostAsJsonAsync("api/users", user);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<User>() ?? throw new Exception("Failed to create user.");
    }

    public async Task UpdateUserAsync(User user)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/users/{user.UserId}", user);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        var response = await _httpClient.DeleteAsync($"api/users/{userId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<int> GetUserCountAsync()
    {
        var response = await _httpClient.GetAsync("api/users/count");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }
}