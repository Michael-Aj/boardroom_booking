using boardroombooking1.Data;
using boardroombooking1.Models;
using boardroombooking1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace boardroombooking1.Controllers;

public class BookingsController : Controller
{
    private readonly BookingService _svc;
    private readonly AppDbContext _db;
    public BookingsController(BookingService svc, AppDbContext db) => (_svc, _db) = (svc, db);

    // Controllers/BookingsController.cs
    public async Task<IActionResult> Index()
    {
        // 1️⃣  Fetch from DB (no ordering)
        var list = await _db.Bookings
            .Include(b => b.Venue)
            .AsNoTracking()
            .ToListAsync();

        // 2️⃣  Order in C# to avoid SQLite’s limitation
        var ordered = list.OrderBy(b => b.StartUtc).ToList();

        return View(ordered);
    }


    public IActionResult Create() { LoadVenues(); return View(); }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Booking b)
    {
        if (!ModelState.IsValid) { LoadVenues(b.VenueId); return View(b); }

        var (ok, err) = await _svc.CreateAsync(b);
        if (!ok) { ModelState.AddModelError("", err!); LoadVenues(b.VenueId); return View(b); }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var b = await _db.Bookings.Include(x => x.Venue).FirstOrDefaultAsync(x => x.Id == id);
        return b is null ? NotFound() : View(b);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var b = await _db.Bookings.FindAsync(id);
        if (b != null) { _db.Remove(b); await _db.SaveChangesAsync(); }
        return RedirectToAction(nameof(Index));
    }

    private void LoadVenues(Guid? id = null) =>
        ViewBag.Venues = new SelectList(_db.Venues.Where(v => v.IsActive), "Id", "Name", id);
}
