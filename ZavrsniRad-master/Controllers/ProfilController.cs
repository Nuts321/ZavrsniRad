using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PolovniAutomobiliZavrsniRad.Models;

namespace PolovniAutomobiliZavrsniRad.Controllers
{
    [Authorize]
    public class ProfilController : Controller
    {
        private readonly AutoContext db;
        public ProfilController(AutoContext _db)
        {
            db = _db;
        }
        [AllowAnonymous]
        public FileContentResult CitajSliku(int? id)
        {
            if (id == null)
            {
                return null;
            }
            Vozilo v1 = db.Vozila.Find(id);
            if (v1 == null)
            {
                return null;
            }
            return File(v1.Slika, v1.SlikaTip);
        }

        // GET: Profil
        public ActionResult Index()
        {
            return View();
        }

        // GET: Profil/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vozilo = await db.Vozila
                .Include(v => v.Marka)
                .Include(v => v.Modeli)
                .Include(v => v.TipVozila)
                .Include(v => v.Komentars)
                .FirstOrDefaultAsync(m => m.VoziloId == id);
            string korisnikId = vozilo.KorisnikId;           
            try
            {
                var korisnik = await db.AspNetUsers.FindAsync(korisnikId);
                ViewBag.Ime = korisnik.Ime;
                ViewBag.Prezime = korisnik.Prezime;
                ViewBag.Telefon = korisnik.Telefon;
                ViewBag.KorisnikId = korisnikId;
            }
            catch (Exception)
            {
                ViewBag.Greska="Doslo je do greske";
            }
            if (vozilo == null)
            {
                return NotFound();
            }

            return View(vozilo);
        }

        // GET: Profil/Create
        public ActionResult Create()
        {
            ViewBag.Marka = new SelectList(db.Marke, "MarkaId", "Naziv");
            ViewBag.Tip = new SelectList(db.TipoviVozila, "TipVozilaId", "Naziv");
            return View();
        }

        // POST: Profil/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VoziloId,MarkaId,ModelId,TipVozilaId,KorisnikId,Kubikaza,Snaga,Kilometraza,Pogon,Menjac,BrojBrzina,Cena,Slika,SlikaTip,Opis")]
        Vozilo vozilo, IFormFile odabranaSlika)
        {
            if (odabranaSlika == null)
            {
                ModelState.AddModelError("FajlSlike", "Niste");
            }
            if (ModelState.IsValid)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    await odabranaSlika.CopyToAsync(ms);
                    vozilo.Slika = ms.ToArray();
                }
                vozilo.SlikaTip = odabranaSlika.ContentType;
                db.Add(vozilo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            return View(vozilo);
        }
        
        // GET: Profil/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vozilo = await db.Vozila.FindAsync(id);
            if (vozilo == null)
            {
                return NotFound();
            }
            ViewData["MarkaId"] = new SelectList(db.Marke, "MarkaId", "Naziv", vozilo.MarkaId);
            ViewData["ModelId"] = new SelectList(db.Models, "ModelId", "Naziv", vozilo.ModelId);
            ViewData["TipVozilaId"] = new SelectList(db.TipoviVozila, "TipVozilaId", "Naziv", vozilo.TipVozilaId);
            return View(vozilo);
        }

        // POST: Profil/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VoziloId,MarkaId,ModelId,TipVozilaId,KorisnikId,Kubikaza,Snaga,Kilometraza,Pogon,Menjac,BrojBrzina,Cena,Slika,SlikaTip,Opis")] Vozilo vozilo, IFormFile odabranaSlika)
        {

            if (id != vozilo.VoziloId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await odabranaSlika.CopyToAsync(ms);
                        vozilo.Slika = ms.ToArray();
                    }
                    vozilo.SlikaTip = odabranaSlika.ContentType;
                    db.Update(vozilo);
                    await db.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoziloExists(vozilo.VoziloId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            ViewData["MarkaId"] = new SelectList(db.Marke, "MarkaId", "Naziv", vozilo.MarkaId);
            ViewData["ModelId"] = new SelectList(db.Models, "ModelId", "Naziv", vozilo.ModelId);
            ViewData["TipVozilaId"] = new SelectList(db.TipoviVozila, "TipVozilaId", "Naziv", vozilo.TipVozilaId);
            return View(vozilo);
        }

        private bool VoziloExists(int id)
        {
            return db.Vozila.Any(e => e.VoziloId == id);
        }

        //GET: Profil/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vozilo = await db.Vozila
                .Include(v => v.Marka)
                .Include(v => v.Modeli)
                .Include(v => v.TipVozila)
                .FirstOrDefaultAsync(m => m.VoziloId == id);
            if (vozilo == null)
            {
                return NotFound();
            }

            return View(vozilo);
        }

        // POST: Profil/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var vozilo = await db.Vozila.FindAsync(id);
            db.Vozila.Remove(vozilo);
            await db.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }
    }
}