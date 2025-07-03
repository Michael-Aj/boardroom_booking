using boardroombooking1.Data;
using boardroombooking1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace boardroombooking1.Controllers;

public class VenuesController : Controller
{
    private readonly AppDbContext _db;
    public VenuesController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index() =>
        View(await _db.Venues.AsNoTracking().ToListAsync());

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Venue v)
    {
        if (!ModelState.IsValid) return View(v);
        _db.Add(v);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var v = await _db.Venues.FindAsync(id);
        return v is null ? NotFound() : View(v);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, byte[] rowVersion)
    {
        var v = await _db.Venues.FindAsync(id);
        if (v == null) return NotFound();

        if (await TryUpdateModelAsync(v))
        {
            _db.Entry(v).Property("RowVersion").OriginalValue = rowVersion;
            try
            {
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Someone else modified this venue. Reload and retry.");
            }
        }
        return View(v);
    }
}
