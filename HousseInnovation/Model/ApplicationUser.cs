using Microsoft.AspNetCore.Identity;

namespace HousseInnovation.Model
{
	public class ApplicationUser: IdentityUser

	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string SureName { get; set; }
		public string IdentityType { get; set; }
		public string IdentityNumber { get; set; }
		public DateTime DOB { get; set; }  // date de naissance

		public string Genre { get; set; }
		public string AgentCode { get; set; }
		public string Msisdn { get; set; }   //+237699947232

		public string Discrimitor { get; set; }  //AGENT, CUSTOMER, 
		public string Adresse { get; set; }

		public bool Bared { get; set; } = false;
		public DateTime CreateAt { get; set; } = DateTime.UtcNow.AddHours(1);
		public long? CompaniesId { get; set; }
		public Boolean IsExist { get; set; }

	}
}
