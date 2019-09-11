using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PolovniAutomobiliZavrsniRad.Models;

namespace PolovniAutomobiliZavrsniRad.Controllers
{
    [Authorize]
    public class KomentarController : Controller
    {
        private readonly AutoContext db;

        public KomentarController(AutoContext context)
        {
            db = context;
        }

        // GET: Komentar
        [AllowAnonymous]
        public IActionResult Index(int? id)
        {
            IEnumerable<Komentar> listaKomentara = db.Kometari;
            if (id != null)
            {
                ViewBag.VoziloId = id;
                listaKomentara = listaKomentara.Where(k => k.VoziloId == id);
            }
            else
            {
                ViewBag.Komentar = "Nema komentara";
            }
            return View(listaKomentara);
        }

        // GET: Komentar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await db.Kometari
                .Include(k => k.Vozilo)
                .FirstOrDefaultAsync(m => m.KomentarId == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // GET: Komentar/Create
        //public IActionResult Create()
        //{
        //    ViewData["VoziloId"] = new SelectList(db.Vozila, "VoziloId", "Kilometraza");
        //    return View();
        //}

        // POST: Komentar/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int VoziloId,string KorisnikId,string Korisnik,string Opis)
        {
            if (ModelState.IsValid)
            {
                Komentar k1 = new Komentar
                {
                    VoziloId = VoziloId,
                    KorisnikId = KorisnikId,
                    Korisnik = Korisnik,
                    Opis = Opis
                };
                db.Kometari.Add(k1);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
            //if (ModelState.IsValid)
            //{
            //    db.Add(komentar);
            //    await db.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return RedirectToAction(nameof(Index));
        }

        // GET: Komentar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var komentar = await db.Kometari.FindAsync(id);
            if (komentar == null)
            {
                return NotFound();
            }
            ViewBag.Opis = komentar.Opis;
            ViewData["VoziloId"] = new SelectList(db.Vozila, "VoziloId", "Kilometraza", komentar.VoziloId);
            return View(komentar);
        }

        // POST: Komentar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KomentarId,VoziloId,KorisnikId,Korisnik,Opis")] Komentar komentar)
        {
            if (id != komentar.KomentarId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(komentar);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KomentarExists(komentar.KomentarId))
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
            ViewData["VoziloId"] = new SelectList(db.Vozila, "VoziloId", "Kilometraza", komentar.VoziloId);
            return View(komentar);
        }

        // GET: Komentar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var komentar = await db.Kometari
                .Include(k => k.Vozilo)
                .FirstOrDefaultAsync(m => m.KomentarId == id);
            if (komentar == null)
            {
                return NotFound();
            }

            return View(komentar);
        }

        // POST: Komentar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var komentar = await db.Kometari.FindAsync(id);
            db.Kometari.Remove(komentar);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KomentarExists(int id)
        {
            return db.Kometari.Any(e => e.KomentarId == id);
        }
    }
}
