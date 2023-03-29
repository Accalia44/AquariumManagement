﻿using System;
namespace Services.Models.Request
{
	public class LoginRequest
	{
		public string Email { get; set; }
		public string Password { get; set; }

		public LoginRequest(string email, string password)
		{
			Email = email;
			Password = password;
		}

	}
}
