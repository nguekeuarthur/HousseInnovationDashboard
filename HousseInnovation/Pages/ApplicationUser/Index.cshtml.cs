using HousseInnovation.DAL;
using HousseInnovation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HousseInnovation.Pages.ApplicationUser
{  
	public class IndexModel : PageModel
    {
		[BindProperty]
		public LoginModel Input { get; set; }
		[BindProperty]
		public bool message { get; set; }
        private readonly ApplicationDBContext _db;
        private readonly IPasswordHasher<Model.ApplicationUser> _passwordHasher;
        public IndexModel(ApplicationDBContext db, IPasswordHasher<Model.ApplicationUser> passwordHasher)
        {

            _db = db;
			_passwordHasher = passwordHasher;
		}


		public void OnGet()
        {
           
        }

		public async Task<IActionResult> OnPostAsync(string? returnUrl = "/Index")
		{
			returnUrl = Url.Content(returnUrl ?? "/Index");

			if (!ModelState.IsValid)
			{
				return Page();
			}

			try
			{
				
				if (Input.codeuser == "arthur" && Input.password == "1234")
				{
					// Si les identifiants sont corrects, créer les claims
					var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, "arthur"),
				new Claim("UserId", "1") // Vous pouvez mettre n'importe quel identifiant d'utilisateur ici
            };

					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

					return LocalRedirect(returnUrl);
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect.");
				}

				return Page();
			}
			catch (Exception ex)
			{
				ModelState.AddModelError(string.Empty, ex.Message);
				return Page();
			}
		}


	}
}

