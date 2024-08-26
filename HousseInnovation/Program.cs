using HousseInnovation.DAL;
using HousseInnovation.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDBContext>(opt =>
	opt.UseSqlServer(builder.Configuration.GetConnectionString("HouseInnovation")));

// Ajouter Razor Pages avec route par défaut vers ApplicationUser/Index
builder.Services.AddRazorPages().AddRazorPagesOptions(options =>
{
	options.Conventions.AddPageRoute("/ApplicationUser/Index", "");
});

// Configurer l'authentification avec cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/ApplicationUser/Index";
	});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
	.AddEntityFrameworkStores<ApplicationDBContext>()
	.AddDefaultTokenProviders();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Rediriger la racine vers ApplicationUser/Index
app.MapGet("/", context =>
{
	context.Response.Redirect("/ApplicationUser/Index");
	return Task.CompletedTask;
});

app.Run();
