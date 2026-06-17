namespace CSE325project.Shared;

public class Reservation
{
    public long ReservationId { get; set; }

    public long RoomId { get; set; }

    public long UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public string? Status { get; set; }
}