using System.Net.Http.Json;
using CSE325project.Shared;

namespace CSE325project.Client.Services;

public class RoomService
{
    private readonly HttpClient _httpClient;

    public RoomService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Room>> GetRoomsAsync()
    {
        var rooms = await _httpClient.GetFromJsonAsync<List<Room>>("api/rooms");
        return rooms ?? new List<Room>();
    }

    public async Task<Room?> GetRoomByIdAsync(long roomId)
    {
        var room = await _httpClient.GetFromJsonAsync<Room>($"api/rooms/{roomId}");
        return room;
    }

    public async Task<Room> CreateRoomAsync(Room room)
    {
        var response = await _httpClient.PostAsJsonAsync("api/rooms", room);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Room>() ?? throw new Exception("Failed to create room.");
    }

    public async Task<Room> UpdateRoomAsync(Room room)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/rooms/{room.RoomId}", room);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Room>() ?? throw new Exception("Failed to update room.");
    }

    public async Task DeleteRoomAsync(long roomId)
    {
        var response = await _httpClient.DeleteAsync($"api/rooms/{roomId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<List<RoomAmenity>> GetRoomAmenitiesByRoomIdAsync(long roomId)
    {
        var roomAmenities = await _httpClient.GetFromJsonAsync<List<RoomAmenity>>($"api/rooms/{roomId}/amenities");
        return roomAmenities ?? new List<RoomAmenity>();
    }

    public async Task<int> GetRoomCountAsync()
    {
        var response = await _httpClient.GetAsync("api/rooms/count");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<List<Room>> GetMostPopularRoomsAsync()
    {
        var rooms = await _httpClient.GetFromJsonAsync<List<Room>>("api/rooms/popular");
        return rooms ?? new List<Room>();
    }
}