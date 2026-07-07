namespace CSE325project.Shared;

public class UpcomingReservationsDto
{
    public Guid ReservationId { get; set; }
    public string Email { get; set; } = "";
    public string RoomName { get; set; } = "";
    public DateTime ReservationDate { get; set; }
    public TimeSpan ReservationTime { get; set; }
}