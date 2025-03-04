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
    [Authorize]
    public class CustomerProfileController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustomerProfile
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CustomerProfiles.Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CustomerProfile/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerProfileModel = await _context.CustomerProfiles
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerProfileModel == null)
            {
                return NotFound();
            }

            return View(customerProfileModel);
        }

        // GET: CustomerProfile/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: CustomerProfile/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,HairLength,HairType,AdditionalInfo")] CustomerProfileModel customerProfileModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customerProfileModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customerProfileModel.UserId);
            return View(customerProfileModel);
        }

        // GET: CustomerProfile/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerProfileModel = await _context.CustomerProfiles.FindAsync(id);
            if (customerProfileModel == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customerProfileModel.UserId);
            return View(customerProfileModel);
        }

        // POST: CustomerProfile/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,HairLength,HairType,AdditionalInfo")] CustomerProfileModel customerProfileModel)
        {
            if (id != customerProfileModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customerProfileModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerProfileModelExists(customerProfileModel.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", customerProfileModel.UserId);
            return View(customerProfileModel);
        }

        // GET: CustomerProfile/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customerProfileModel = await _context.CustomerProfiles
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customerProfileModel == null)
            {
                return NotFound();
            }

            return View(customerProfileModel);
        }

        // POST: CustomerProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customerProfileModel = await _context.CustomerProfiles.FindAsync(id);
            if (customerProfileModel != null)
            {
                _context.CustomerProfiles.Remove(customerProfileModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerProfileModelExists(int id)
        {
            return _context.CustomerProfiles.Any(e => e.Id == id);
        }
    }
}
