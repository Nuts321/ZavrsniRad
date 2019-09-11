using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PolovniAutomobiliZavrsniRad.Data;

namespace PolovniAutomobiliZavrsniRad.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage ="Morate uneti Ime")]
            [StringLength(30,ErrorMessage ="Maksimalno 30 a minimalno 3 karaktera",MinimumLength =3)]
            public string Ime { get; set; }

            [Required(ErrorMessage = "Morate uneti Prezime")]
            [StringLength(30, ErrorMessage = "Maksimalno 30 a minimalno 3 karaktera", MinimumLength = 3)]
            public string Prezime { get; set; }

            [Required(ErrorMessage = "Morate uneti Adresu")]
            [StringLength(50, ErrorMessage = "Maksimalno 50 a minimalno 3 karaktera", MinimumLength = 3)]
            public string Adresa { get; set; }

            [Required(ErrorMessage = "Morate uneti Grad")]
            [StringLength(30, ErrorMessage = "Maksimalno 30 a minimalno 3 karaktera", MinimumLength = 3)]
            public string Grad { get; set; }

            [Required(ErrorMessage = "Morate uneti Telefon")]
            [StringLength(30, ErrorMessage = "Maksimalno 30 karaktera")]
            public string Telefon { get; set; }

            [Required(ErrorMessage ="Unesite email adresu.")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage ="Morate uneti lozinku")]
            [StringLength(100, ErrorMessage = "Lozinka mora da sadrzi minimum 3 karaktera", MinimumLength = 3)]
            [DataType(DataType.Password)]
            [Display(Name = "Lozinka")]
            public string Lozinka { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Potvrdi lozinku")]
            [Compare("Lozinka", ErrorMessage = "Lozinke se ne podudaraju.")]
            public string PotvrdiLozinku { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Ime = Input.Ime,
                    Prezime = Input.Prezime,
                    Adresa = Input.Adresa,
                    Grad = Input.Grad,
                    Telefon = Input.Telefon
                };
                var result = await _userManager.CreateAsync(user, Input.Lozinka);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
