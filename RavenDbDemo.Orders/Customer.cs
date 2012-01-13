using Newtonsoft.Json;

namespace RavenOrders
{
	public class Customer
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		[JsonIgnore]
		public string FullName { get { return FirstName + " " + LastName; } }
		public Address Address { get; set; }
		public string EmailAddress { get; set; }
	}
}