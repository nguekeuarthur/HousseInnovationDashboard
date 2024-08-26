using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace HousseInnovation.Models
{
	public class LoginModel
	{
		[BindRequired]
		public string codeuser { get; set; }

		[BindRequired]
		public string password { get; set; }
	}
}
