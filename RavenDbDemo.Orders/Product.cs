using System.Collections.Generic;

namespace RavenOrders
{
	public class Product
	{
		public Product() { Attributes = new List<ProductAttribute>(); }

		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public IList<ProductAttribute> Attributes { get; set; }
		public double UnitPrice { get; set; }
	}

	public class ProductAttribute
	{
		public string Name { get; set; }
		public string[] Values { get; set; }
	}

}