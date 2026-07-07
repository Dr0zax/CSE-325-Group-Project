namespace CSE325project.Shared;
public class ReservationManagerDto
{
    public Guid ReservationId { get; set; }
    public string Email { get; set; } = "";
    public string RoomName { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public String Status { get; set; } = "";
}