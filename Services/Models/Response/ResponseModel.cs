using System;
namespace Services.Models.Response
{
	public class ResponseModel
	{
		public Boolean HasError { get; set; }
		public List<String> ErrorMessages { get; set; } = new();

		public ResponseModel()
		{
		}
	}
}
