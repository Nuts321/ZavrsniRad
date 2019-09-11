using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PolovniAutomobiliZavrsniRad.Data;
using PolovniAutomobiliZavrsniRad.Models;

namespace PolovniAutomobiliZavrsniRad.Controllers
{
    public class HomeController : Controller
    {
        private readonly RoleManager<IdentityRole> rm;
        private readonly UserManager<ApplicationUser> um;
        private readonly AutoContext db;

        public HomeController(RoleManager<IdentityRole> _rm, UserManager<ApplicationUser> _um, AutoContext _db)
        {
            rm = _rm;
            um = _um;
            db = _db;
        }

        //administracija
        public async Task<int> KreirajRolu(string rola)
        {
            bool rolaPostoji = await rm.RoleExistsAsync(rola);

            if (rolaPostoji)
            {
                return 0;
            }

            else
            {
                IdentityRole rolaAdmin = new IdentityRole(rola);
                var rezultat = await rm.CreateAsync(rolaAdmin);
                if (rezultat.Succeeded)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        //Kreiraj administratora
        private async Task<ApplicationUser> KreirajAdministratora()
        {
            ApplicationUser admin = await um.FindByEmailAsync("admin@gmail.com");
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName="Admin",
                    Email = "admin@gmail.com",
                    Ime = "Admin",
                    Prezime = "Admin",
                    Adresa = "Admin",
                    Grad = "Admin",
                    Telefon = "123456"
                };
                string lozinka = "123";

                var rezultat = await um.CreateAsync(admin, lozinka);
                if (rezultat.Succeeded)
                {
                    return admin;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return admin;
            }
        }

        //AdminSistema
        [Authorize(Roles ="Admin")]
        //[AllowAnonymous]
        public async Task<IActionResult> AdminSistema()
        {
            int rolaPostoji = await KreirajRolu("admin");
            ApplicationUser admin = await KreirajAdministratora();

            if (rolaPostoji == -1)
            {
                ViewBag.Poruka = "Greska pri kreiranju role";
                return View();
            }
            if (admin == null)
            {
                ViewBag.Poruka = "Greska pri kreiranju admina";
                return View();
            }

            bool rezultat1 = await um.IsInRoleAsync(admin, "admin");

            if (rezultat1)
            {
                ViewBag.Poruka = "Korisnik je vec u roli Admina";
                return View();
            }

            var rezultat = await um.AddToRoleAsync(admin, "admin");
            if (rezultat.Succeeded)
            {
                ViewBag.Poruka = "Kreiran Administrator sajta";
            }
            else
            {
                ViewBag.Poruka = "Greska pri dodavanju korisnika u rolu";
            }
            return View();
        }


        //public IActionResult Index()
        //{
        //    IEnumerable<Vozilo> listavozila = db.Vozila;
        //    ViewBag.Marka = new SelectList(db.Marke, "MarkaId", "Naziv");
        //    return View(listavozila);
        //}
        public async Task<IActionResult> Index()
        {

            //UPDATE
            //VRATITI MARKE SAMO ONIH VOZILA KOJI SU UBACENI
            
            ViewBag.Marka = new SelectList(db.Marke, "MarkaId", "Naziv");
            var autoContext = db.Vozila.Include(v => v.Marka).Include(v => v.Modeli).Include(v => v.TipVozila);
            return View(await autoContext.ToListAsync());
        }

        public PartialViewResult PrikaziModel(int MarkaId = 0)
        {
            if (MarkaId != 0)
            {
                ViewBag.Model = new SelectList(db.Models.Where(m => m.MarkaId == MarkaId), "ModelId", "Naziv");
            }
            return PartialView();
        }

        /// <summary>
        /// Mozda je bolje da se filtiranje odradi cim se udje u Index stranu
        /// AKO parametri nisu postavljeni prikazi sve oglase
        /// U SUPROTNOM prikaze filtrirane podatke
        /// </summary>
        /// <param name="min"> minimalna cena</param>
        /// <param name="max"> maksimalna cena</param>
        /// <param name="MarkaId"> ID Marke</param>
        /// <param name="ModelId"> ID modela</param>
        /// <returns></returns>
        ///
        public PartialViewResult Filtriraj(string korisnikId, decimal? min, decimal? max, int MarkaId = 0, int ModelId = 0)
        {
            IEnumerable<Vozilo> listaVozila = db.Vozila.Include(v => v.Marka)
                .Include(v => v.Modeli);

            if (MarkaId!=0)
            {
                listaVozila = listaVozila.Where(id => id.MarkaId == MarkaId);
            }
            if (ModelId != 0)
            {
                listaVozila = listaVozila.Where(id => id.ModelId == ModelId);
            }
            if (min == null)
            {
                try
                {
                    min = listaVozila.Min(p => p.Cena);
                }
                catch (Exception)
                {

                    return PartialView();
                }

                //min = listaVozila.Min(p => p.Cena);
            }
            if (max == null)
            {
                try
                {
                    max = listaVozila.Max(p => p.Cena);
                }
                catch (Exception)
                {

                    return PartialView();
                }
                //max = listaVozila.Max(p => p.Cena);
            }
            if (korisnikId != null) 
            {
                try
                {
                    listaVozila = listaVozila.Where(k => k.KorisnikId == korisnikId);
                }
                catch (Exception)
                {

                    return PartialView();
                }
            }
            listaVozila = listaVozila.Where(p => p.Cena >= min && p.Cena <= max);
            return PartialView(listaVozila);
        }

        public IActionResult Posalji()
        {
            return View();
        }

        [ValidateAntiForgeryToken]

        [HttpPost]
        public IActionResult Posalji(string ime, string prezime, string email, string poruka)
        {

            MailAddress admin = new MailAddress("tripledoublemachine23@gmail.com");
            MailAddress posiljaoc = new MailAddress(email, ime + " " + prezime);
            MailMessage msg = new MailMessage();
            msg.To.Add(admin);
            msg.From = posiljaoc;

            msg.Subject = "Poruka sa web sajta";
            msg.IsBodyHtml = true;
            msg.Body = poruka;

            SmtpClient klijent = new SmtpClient("smtp.gmail.com");
            klijent.Credentials = new NetworkCredential("itageneracija2018@gmail.com", "link2019a");
            klijent.EnableSsl = true;
            klijent.Port = 587;

            try
            {
                klijent.Send(msg);
                ViewBag.Poruka = "Email poslat! Odgovoricemo Vam u najkracem mogucem roku.";
                return View();
            }
            catch (System.Exception)
            {
                ViewBag.Poruka = "Greska pri slanju emaila!";
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
