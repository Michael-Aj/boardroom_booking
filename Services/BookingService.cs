using boardroombooking1.Data;
using boardroombooking1.Models;
using Microsoft.EntityFrameworkCore;

namespace boardroombooking1.Services;

public class BookingService
{
    private readonly AppDbContext _db;
    public BookingService(AppDbContext db) => _db = db;

    public async Task<(bool ok, string? err)> CreateAsync(Booking b, CancellationToken ct = default)
    {
        var overlap = await _db.Bookings
            .Where(x => x.VenueId == b.VenueId)
            .Select(x => new { x.StartUtc, x.EndUtc })
            .AsNoTracking()
            .ToListAsync(ct)
            .ContinueWith(t => t.Result.Any(x =>
                    x.StartUtc < b.EndUtc && b.StartUtc < x.EndUtc), ct);

        if (overlap) return (false, "Time slot already taken.");

        _db.Bookings.Add(b);
        await _db.SaveChangesAsync(ct);
        return (true, null);
    }
}
