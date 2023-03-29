using System;
using DAL.Entities;
using Services.Authentications;

namespace Services.Models.Response
{
	public class UserResponse
	{
		public User User { get; set; }

		public AuthenticationInformation AuthenticationInformation { get; set; }

	}
}

