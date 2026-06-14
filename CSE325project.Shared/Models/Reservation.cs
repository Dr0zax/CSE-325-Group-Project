public class Reservation
{
    public long ReservationId { get; set; }

    public long RoomId { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string? Description { get; set; }
}