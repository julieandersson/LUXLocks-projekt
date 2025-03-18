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
            if (appointmentModel.AppointmentDate.HasValue)
            {
                var selectedTime = appointmentModel.AppointmentDate.Value;

                // konverterar till serverns lokala tidszon för att undvika problem
                selectedTime = selectedTime.ToLocalTime();

                int hour = selectedTime.Hour;
                int minute = selectedTime.Minute;
                int second = selectedTime.Second;

                // begränsar bokningar till endast mellan 10:00 och 17:00
                if (hour < 10 || hour > 17)  //
                {
                    ModelState.AddModelError("AppointmentDate", "Tiden måste vara mellan 10:00 och 17:00.");
                }

                // endast hela timmar kan väljas, minuter och sekunder är 0
                if (minute != 0 || second != 0)
                {
                    ModelState.AddModelError("AppointmentDate", "Du kan endast välja hela timmar, t.ex. 10:00, 11:00, 12:00...");
                }
            }

            // om modell är giltig efter validering
            if (ModelState.IsValid)
            {
                _context.Add(appointmentModel);
                await _context.SaveChangesAsync();

                // sparar bokningsmeddelandet i TempData för temporär lagring av data så att det kan hämtas på nästa sida
                TempData["BookingMessage"] = "Tack för din bokning! En bokningsbekräftelse har skickats till din e-postadress. Hör av dig 24 timmar innan din bokade tid för eventuella avbokningar eller ändringar.";
                
                return RedirectToAction("Index", "Home"); // skickar tillbaka användaren till startsidan efter bokning
            }

            // laddar om dropdownlistorna så att sidan inte kraschar vid fel
            ViewData["StylistModelId"] = new SelectList(_context.Stylists, "Id", "Name", appointmentModel.StylistModelId);
            ViewData["TreatmentModelId"] = new SelectList(
                _context.Treatments.Select(t => new
                {
                    Id = t.Id,
                    DisplayName = t.Name + " - " + t.Price.ToString("0") + " kr"
                }),
                "Id",
                "DisplayName",
                appointmentModel.TreatmentModelId
            );

            return View(appointmentModel); // återgår till bokningsformuläret om valideringen misslyckas
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
            ViewData["StylistModelId"] = new SelectList(_context.Stylists, "Id", "Name", appointmentModel.StylistModelId);
            ViewData["TreatmentModelId"] = new SelectList(_context.Treatments, "Id", "Name", appointmentModel.TreatmentModelId);
            return View(appointmentModel);
        }

        // POST: Appointment/Edit/5
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