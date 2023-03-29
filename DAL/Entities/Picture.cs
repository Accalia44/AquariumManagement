using System;
namespace DAL.Entities
{
	public class Picture : Entity
	{
		public string Aquarium { get; set; }
		public string Description { get; set; }
		public string ContentType { get; set; }
		public string PictureID { get; set; }
		public DateTime Uploaded { get; set; } = DateTime.Now;

		public Picture(){}

		public Picture(string id, string aqurium, string description, string contentType)
		{
			PictureID = id;
			Aquarium = aqurium;
			Description = description;
			ContentType = contentType;
		}
	}
}

