using static System.Net.WebRequestMethods;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Security.Principal;
using System.Security;
using System.Transactions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HousseInnovation.Model;

namespace HousseInnovation.DAL
{
	public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			// Global turn off delete behaviour on foreign keys


			//Configuration des relations entre table
		

			base.OnModelCreating(builder);
            // Personnalisez le modèle Identity ASP.NET et remplacez les paramètres par défaut si nécessaire.
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers");
        }


		public DbSet<Client> Client { get; set; }

		public DbSet<ApplicationUser> ApplicationUser { get; set; }

	}
}