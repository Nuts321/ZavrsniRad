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
    public class AspNetUsersController : Controller
    {
        private readonly AutoContext db;

        public AspNetUsersController(AutoContext context)
        {
            db = context;
        }

        // GET: AspNetUsers
        public async Task<IActionResult> Index()
        {
            return View(await db.AspNetUsers.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> DodajMarkuModelTip(int ModelId = 0, int MarkaId = 0, string NoviTip = "", string NovaMarkaModel = "")
        {
            if (NovaMarkaModel!=" ")
            {
                NovaMarkaModel = NovaMarkaModel.First().ToString().ToUpper() + String.Join("", NovaMarkaModel.Skip(1)).ToLower();
                if (MarkaId == 0)
                {
                    try
                    {
                        Marka m1 = new Marka
                        {
                            Naziv = NovaMarkaModel
                        };
                        db.Marke.Add(m1);
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception)
                    {

                        ViewBag.Greska="Doslo je do greske prilikom cuvanja Marke";
                    }
                }
                if (MarkaId!=0 && ModelId==0)
                {
                    try
                    {
                        Modeli m1 = new Modeli
                        {
                            MarkaId=MarkaId,
                            Naziv = NovaMarkaModel
                        };
                        db.Models.Add(m1);
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception)
                    {
                        ViewBag.Greska = "Doslo je do greske prilikom cuvanja Modela";
                    }
                }
            }
            if (NoviTip!="")
            {
                NoviTip = NoviTip.First().ToString().ToUpper() + String.Join("", NoviTip.Skip(1)).ToLower();
                try
                {
                    TipVozila t1 = new TipVozila
                    {
                        Naziv = NoviTip
                    };
                    db.TipoviVozila.Add(t1);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception)
                {
                    ViewBag.Greska = "Doslo je do greske prilikom cuvanja Tipa vozila";                    
                }
                
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: AspNetUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            //var aspNetUser = await db.AspNetUsers
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (aspNetUser == null)
            //{
            //    return NotFound();
            //}
            //ViewBag.Ime = aspNetUser.Ime;
            //ViewBag.Prezime = aspNetUser.Prezime;
            //ViewBag.Telefon = aspNetUser.Telefon;
            //ViewBag.Grad = aspNetUser.Grad;
            //ViewBag.Adresa = aspNetUser.Adresa;
            //return View();
            ViewBag.Marka = new SelectList(db.Marke, "MarkaId", "Naziv");
            ViewBag.Tip = new SelectList(db.TipoviVozila, "TipVozilaId", "Naziv");

            if (id == null)
            {
                return NotFound();
            }

            var aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AspNetUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,Adresa,Grad,Ime,Prezime,Telefon")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Add(aspNetUser);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUser = await db.AspNetUsers.FindAsync(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,Adresa,Grad,Ime,Prezime,Telefon")] AspNetUser aspNetUser)
        {
            if (id != aspNetUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(aspNetUser);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUserExists(aspNetUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View("Details",id);
            }
            return View(aspNetUser);
        }

        // GET: AspNetUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUser = await db.AspNetUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            return View(aspNetUser);
        }

        // POST: AspNetUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUser = await db.AspNetUsers.FindAsync(id);
            db.AspNetUsers.Remove(aspNetUser);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUserExists(string id)
        {
            return db.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
