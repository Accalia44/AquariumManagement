using System;
using Microsoft.AspNetCore.Http;

namespace Services.Models.Request
{
	public class PictureRequest
	{
		public string Description { get; set; }

		public IFormFile FormFile {get; set;}

	}
}

