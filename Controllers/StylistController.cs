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
    public class StylistController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly string wwwRootPath;

        public StylistController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwRootPath = hostEnvironment.WebRootPath;
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
        // Lägga till en frisör kräver inloggning
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Stylist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,Name,Bio,ImageFile")] StylistModel stylistModel)
        {
            if (ModelState.IsValid)
            {
                // kontrollera om det finns någon bild
                if (stylistModel.ImageFile != null)
                {
                    // Generera unikt filnamn 
                    string fileName = Path.GetFileNameWithoutExtension(stylistModel.ImageFile.FileName);
                    string extension = Path.GetExtension(stylistModel.ImageFile.FileName); // filändelse, såsom jpg, png etc

                    stylistModel.ImageName = fileName = fileName.Replace(" ", String.Empty) + DateTime.Now.ToString("yymmssfff") + extension; // tar bort alla mellanslag om det finns, lägger till datum och filändelse på slutet 

                    string path = Path.Combine(wwwRootPath + "/images", fileName); // sökvägen, lägger i mappen images

                    // sparar i filsystemet
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await stylistModel.ImageFile.CopyToAsync(fileStream); // gör en await för att spara
                    }
                }
                else
                {
                    stylistModel.ImageName = "placeholder.jpg"; // standardvärde om ingen bild laddas upp
                }

                _context.Add(stylistModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stylistModel);
        }

        // GET: Stylist/Edit/5
        // Redigera frisör kräver inloggning
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Bio,ImageName,ImageFile")] StylistModel stylistModel)
        {
            if (id != stylistModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingStylist = await _context.Stylists.FindAsync(id);
                    if (existingStylist == null)
                    {
                        return NotFound();
                    }

                    // om en ny bild laddas upp
                    if (stylistModel.ImageFile != null)
                    {
                        // Ta bort den gamla bilden (om det inte är placeholder.jpg)
                        if (!string.IsNullOrEmpty(existingStylist.ImageName) && existingStylist.ImageName != "placeholder.jpg")
                        {
                            string oldImagePath = Path.Combine(wwwRootPath + "/images", existingStylist.ImageName);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Generera nytt unikt filnamn
                        string fileName = Path.GetFileNameWithoutExtension(stylistModel.ImageFile.FileName);
                        string extension = Path.GetExtension(stylistModel.ImageFile.FileName);
                        fileName = fileName.Replace(" ", string.Empty) + DateTime.Now.ToString("yymmssfff") + extension;
                        string filePath = Path.Combine(wwwRootPath + "/images", fileName);

                        // sparar den nya bilden i filsystemet
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await stylistModel.ImageFile.CopyToAsync(fileStream);
                        }

                        // Uppdaterar bildnamnet i databasen
                        existingStylist.ImageName = fileName;
                    }

                    // Uppdaterar andra fält
                    existingStylist.Name = stylistModel.Name;
                    existingStylist.Bio = stylistModel.Bio;

                    _context.Update(existingStylist);
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
        // Radera frisör kräver inloggning
        [Authorize]
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
        [Authorize]
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