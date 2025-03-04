using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LUXLocks_projekt.Data;
using LUXLocks_projekt.Models;

namespace LUXLocks_projekt.Controllers
{
    public class StylistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StylistController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stylist
        public async Task<IActionResult> Index()
        {
            return View(await _context.Stylists.ToListAsync());
        }

        // GET: Stylist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stylistModel = await _context.Stylists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stylistModel == null)
            {
                return NotFound();
            }

            return View(stylistModel);
        }

        // GET: Stylist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stylist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Bio")] StylistModel stylistModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stylistModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stylistModel);
        }

        // GET: Stylist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stylistModel = await _context.Stylists.FindAsync(id);
            if (stylistModel == null)
            {
                return NotFound();
            }
            return View(stylistModel);
        }

        // POST: Stylist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Bio")] StylistModel stylistModel)
        {
            if (id != stylistModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stylistModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StylistModelExists(stylistModel.Id))
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
            return View(stylistModel);
        }

        // GET: Stylist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stylistModel = await _context.Stylists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stylistModel == null)
            {
                return NotFound();
            }

            return View(stylistModel);
        }

        // POST: Stylist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stylistModel = await _context.Stylists.FindAsync(id);
            if (stylistModel != null)
            {
                _context.Stylists.Remove(stylistModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StylistModelExists(int id)
        {
            return _context.Stylists.Any(e => e.Id == id);
        }
    }
}
