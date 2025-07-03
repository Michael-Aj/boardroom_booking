namespace boardroombooking1.Models;

public class Venue
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public byte[]? RowVersion { get; set; }
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
