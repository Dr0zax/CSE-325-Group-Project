using System.Net.Http.Json;
using CSE325project.Shared;

namespace CSE325project.Client.Services;

public class RoomAmenityService
{
    private readonly HttpClient _httpClient;

    public RoomAmenityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task<List<RoomAmenity>> GetRoomAmenitiesAsync()
    {
        var roomAmenities = await _httpClient.GetFromJsonAsync<List<RoomAmenity>>("api/roomamenities");
        return roomAmenities ?? new List<RoomAmenity>();
    }

    public async Task<RoomAmenity?> GetRoomAmenityByIdAsync(long roomAmenityId)
    {
        var roomAmenity = await _httpClient.GetFromJsonAsync<RoomAmenity>($"api/roomamenities/{roomAmenityId}");
        return roomAmenity;
    }

    public async Task<RoomAmenity> CreateRoomAmenityAsync(RoomAmenity roomAmenity)
    {
        var response = await _httpClient.PostAsJsonAsync("api/roomamenities", roomAmenity);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<RoomAmenity>() ?? throw new Exception("Failed to create room amenity.");
    }

    public async Task<RoomAmenity> UpdateRoomAmenityAsync(RoomAmenity roomAmenity)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/roomamenities/{roomAmenity.RoomAmenityId}", roomAmenity);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<RoomAmenity>() ?? throw new Exception("Failed to update room amenity.");
    }

    public async Task<List<long>> GetAmenitiesByRoomIdAsync(long roomId)
    {
        var amenities = await _httpClient.GetFromJsonAsync<List<long>>($"api/rooms/{roomId}/amenities");
        return amenities ?? new List<long>();
    }
}