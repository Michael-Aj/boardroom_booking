namespace boardroombooking1.Models;

public class Booking
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid VenueId { get; set; }
    public Venue? Venue { get; set; }

    public string Title { get; set; } = string.Empty;
    public DateTimeOffset StartUtc { get; set; }
    public DateTimeOffset EndUtc { get; set; }
    public string CreatedBy { get; set; } = "staff";
}
