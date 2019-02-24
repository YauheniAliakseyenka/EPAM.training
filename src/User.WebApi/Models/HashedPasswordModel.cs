namespace User.WebApi.Models
{
	internal class HashedPasswordModel
	{
		public string Hash { get; set; }
		public string Salt { get; set; }
	}
}
