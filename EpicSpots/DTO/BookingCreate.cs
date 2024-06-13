using EpicSpots.DTO;

public class BookingCreateDTO
{
    public int CampsiteId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public string BookingStatus { get; set; } = "Confirmed"; // Default to Pending
}
