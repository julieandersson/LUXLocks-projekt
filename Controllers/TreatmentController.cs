using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LUXLocks_projekt.Data;
using LUXLocks_projekt.Models;
using Microsoft.AspNetCore.Authorization;

namespace LUXLocks_projekt.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TreatmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Treatment
        public async Task<IActionResult> Index(string priceRange)
        {
            var treatments = _context.Treatments.AsQueryable();

            // sparar det valda prisintervallet i ViewBag för att behålla valet i vyn
            ViewBag.CurrentPriceRange = priceRange;

            // om användaren har valt ett prisintervall, filtrera resultaten
            if (!string.IsNullOrEmpty(priceRange))
            {
                var priceBounds = priceRange.Split("-");
                if (priceBounds.Length == 2 && int.TryParse(priceBounds[0], out int minPrice) && int.TryParse(priceBounds[1], out int maxPrice))
                {
                    treatments = treatments.Where(t => t.Price >= minPrice && t.Price <= maxPrice); // filtrerar listan med behandlingar baserat på valt prisintervall
                }
            }

            return View(await treatments.ToListAsync());
        }

        // GET: Treatment/Create
        // Lägg till behandling kräver inloggning
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Treatment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price")] TreatmentModel treatmentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(treatmentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(treatmentModel);
        }

        // GET: Treatment/Edit/5
        // Redigera behandling kräver inloggning
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentModel = await _context.Treatments.FindAsync(id);
            if (treatmentModel == null)
            {
                return NotFound();
            }
            return View(treatmentModel);
        }

        // POST: Treatment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price")] TreatmentModel treatmentModel)
        {
            if (id != treatmentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(treatmentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TreatmentModelExists(treatmentModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(treatmentModel);
        }

        // GET: Treatment/Delete/5
        // Radera behandling kräver inloggning
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var treatmentModel = await _context.Treatments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (treatmentModel == null)
            {
                return NotFound();
            }

            return View(treatmentModel);
        }

        // POST: Treatment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treatmentModel = await _context.Treatments.FindAsync(id);
            if (treatmentModel != null)
            {
                _context.Treatments.Remove(treatmentModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TreatmentModelExists(int id)
        {
            return _context.Treatments.Any(e => e.Id == id);
        }
    }
}
