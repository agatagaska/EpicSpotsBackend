using EpicSpots.DTO;

public class BookingDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CampsiteId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NumberOfGuests { get; set; }
    public decimal TotalPrice { get; set; }
    public string BookingStatus { get; set; }
    public CampsiteDTO Campsite { get; set; }
}