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
        var reservations = await _httpClient.GetFromJsonAsync<List<Reservation>>("api/reservation");
        return reservations ?? new List<Reservation>();
    }

    public async Task<Reservation?> GetReservationByIdAsync(Guid reservationId)
    {
        var reservation = await _httpClient.GetFromJsonAsync<Reservation>($"api/reservation/{reservationId}");
        return reservation;
    }

    public async Task<Reservation> CreateReservationAsync(Reservation reservation)
    {
        var response = await _httpClient.PostAsJsonAsync("api/reservation", reservation);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Reservation>() ?? throw new Exception("Failed to create reservation.");
    }

    public async Task<Reservation> UpdateReservationAsync(Reservation reservation)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/reservation/{reservation.ReservationId}", reservation);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<Reservation>() ?? throw new Exception("Failed to update reservation.");
    }

    public async Task DeleteReservationAsync(Guid reservationId)
    {
        var response = await _httpClient.DeleteAsync($"api/reservation/{reservationId}");
        response.EnsureSuccessStatusCode();
    }

    public async Task<int> GetReservationCountAsync()
    {
        var response = await _httpClient.GetAsync("api/reservation/count");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<int>();
    }

    public async Task<List<RoomReservationCountDto>> GetReservationCountsAsync()
    {
        var response = await _httpClient.GetAsync("api/reservation/counts");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<RoomReservationCountDto>>() ?? new List<RoomReservationCountDto>();
    }

    public async Task<List<UpcomingReservationsDto>> GetUpcomingReservationsAsync()
    {
        var reservations = await _httpClient.GetFromJsonAsync<List<UpcomingReservationsDto>>("api/reservation/upcoming");
        return reservations ?? new List<UpcomingReservationsDto>();
    }
}