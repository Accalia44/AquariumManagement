using System;
namespace Services.Models.Response
{
	public class ItemResponseModel<T> : ResponseModel where T : class
	{
		public T Data { get; set; }

	}
}

