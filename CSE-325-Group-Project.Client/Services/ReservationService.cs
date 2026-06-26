using System.Net.Http.Json;
using CSE325project.Shared;

namespace CSE325project.Client.Services;
public class ReservationService
{
    private readonly HttpClient _httpClient;

    public ReservationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Reservation>> GetReservationsAsync()
    {
        var reservations = await _httpClient.GetFromJsonAsync<List<Reservation>>("api/reservations");
        return reservations ?? new List<Reservation>();
    }

    public async Task<Reservation?> GetReservationByIdAsync(long reservationId)
    {
        var reservation = await _httpClient.GetFromJsonAsync<Reservation>($"api/reservations/{reservationId}");
        return reservation;
    }

    public async Task<Reservation> CreateReservationAsync(Reservation reservation)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reservations", reservation);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Reservation>() ?? throw new Exception("Failed to create reservation.");
    }

    public async Task<Reservation> UpdateReservationAsync(Reservation reservation)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/reservations/{reservation.ReservationId}", reservation);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Reservation>() ?? throw new Exception("Failed to update reservation.");
    }

    public async Task DeleteReservationAsync(long reservationId)
    {
        var response = await _httpClient.DeleteAsync($"api/reservations/{reservationId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<int> GetReservationCountAsync()
    {
        var response = await _httpClient.GetAsync("api/reservations/count");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }
}