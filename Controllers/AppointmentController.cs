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
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointment
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Appointments.Include(a => a.Stylist).Include(a => a.Treatment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Appointment/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentModel = await _context.Appointments
                .Include(a => a.Stylist)
                .Include(a => a.Treatment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentModel == null)
            {
                return NotFound();
            }

            return View(appointmentModel);
        }

        // GET: Appointment/Create
        public IActionResult Create()
        {

            ViewData["StylistModelId"] = new SelectList(_context.Stylists, "Id", "Name");
            
            // kombinerar namn och pris i en sträng så att båda visas i dropdownmenyn vid bokning
            ViewData["TreatmentModelId"] = new SelectList(
                _context.Treatments.Select(t => new 
                { 
                    Id = t.Id, 
                    DisplayName = t.Name + " - " + t.Price.ToString("0") + " kr" 
                }), 
                "Id", 
                "DisplayName"
            );

            return View();
        }

        // POST: Appointment/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AppointmentDate,CustomerName,PhoneNumber,Email,StylistModelId,TreatmentModelId,HairLength,HairType,AdditionalInfo,SilentTreatment")] AppointmentModel appointmentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointmentModel);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ViewData["StylistModelId"] = new SelectList(_context.Stylists, "Id", "Id", appointmentModel.StylistModelId);
            ViewData["TreatmentModelId"] = new SelectList(_context.Treatments, "Id", "Id", appointmentModel.TreatmentModelId);
            return View(appointmentModel);
        }

        // GET: Appointment/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentModel = await _context.Appointments.FindAsync(id);
            if (appointmentModel == null)
            {
                return NotFound();
            }
            ViewData["StylistModelId"] = new SelectList(_context.Stylists, "Id", "Id", appointmentModel.StylistModelId);
            ViewData["TreatmentModelId"] = new SelectList(_context.Treatments, "Id", "Id", appointmentModel.TreatmentModelId);
            return View(appointmentModel);
        }

        // POST: Appointment/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AppointmentDate,CustomerName,PhoneNumber,Email,StylistModelId,TreatmentModelId,HairLength,HairType,AdditionalInfo,SilentTreatment")] AppointmentModel appointmentModel)
        {
            if (id != appointmentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointmentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentModelExists(appointmentModel.Id))
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
            ViewData["StylistModelId"] = new SelectList(_context.Stylists, "Id", "Id", appointmentModel.StylistModelId);
            ViewData["TreatmentModelId"] = new SelectList(_context.Treatments, "Id", "Id", appointmentModel.TreatmentModelId);
            return View(appointmentModel);
        }

        // GET: Appointment/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointmentModel = await _context.Appointments
                .Include(a => a.Stylist)
                .Include(a => a.Treatment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appointmentModel == null)
            {
                return NotFound();
            }

            return View(appointmentModel);
        }

        // POST: Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointmentModel = await _context.Appointments.FindAsync(id);
            if (appointmentModel != null)
            {
                _context.Appointments.Remove(appointmentModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppointmentModelExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}