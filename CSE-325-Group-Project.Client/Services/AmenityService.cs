using System.Net.Http.Json;
using CSE325project.Shared;

namespace CSE325project.Client.Services;

public class AmenityService
{
    private readonly HttpClient _httpClient;

    public AmenityService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<AmenitySelectionDto>> GetAmenitiesAsync()
    {
        var amenities = await _httpClient.GetFromJsonAsync<List<AmenitySelectionDto>>("api/amenities");
        return amenities ?? new List<AmenitySelectionDto>();
    }

    public async Task<Amenity?> GetAmenityByIdAsync(long amenityId)
    {
        var amenity = await _httpClient.GetFromJsonAsync<Amenity>($"api/amenities/{amenityId}");
        return amenity;
    }

    public async Task<Amenity> CreateAmenityAsync(Amenity amenity)
    {
        var response = await _httpClient.PostAsJsonAsync("api/amenities", amenity);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Amenity>() ?? throw new Exception("Failed to create amenity.");
    }

    public async Task<Amenity> UpdateAmenityAsync(Amenity amenity)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/amenities/{amenity.AmenityId}", amenity);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Amenity>() ?? throw new Exception("Failed to update amenity.");
    }

    public async Task DeleteAmenityAsync(long amenityId)
    {
        var response = await _httpClient.DeleteAsync($"api/amenities/{amenityId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<int> GetAmenityCountAsync()
    {
        var response = await _httpClient.GetAsync("api/amenities/count");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }
}