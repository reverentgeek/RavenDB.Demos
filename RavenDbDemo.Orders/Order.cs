using System;
using System.Collections.Generic;

namespace RavenOrders
{
	public class BillingInformation
	{
		public string BillToName { get; set; }
		public string PaymentMethod { get; set; }
		public string ConfirmationCode { get; set; }
		public Address BillToAddress { get; set; }
	}

	public class ShippingInformation
	{
		public DateTime? ShipDate { get; set; }
		public string ShipToName { get; set; }
		public string ShippingMethod { get; set; }
		public Address ShipToAddress { get; set; }
	}

	public class OrderLineItem
	{
		public string ProductId { get; set; }
		public string ProductName { get; set; }
		public string ProductDescription { get; set; }
		public double UnitPrice { get; set; }
		public int Quantity { get; set; }
		public double Discount { get; set; }
	}

	public class Order
	{
		public Order()
		{
			OrderDate = DateTime.UtcNow;
			LineItems = new List<OrderLineItem>();
		}

		public string Id { get; set; }
		public string CustomerId { get; set; }
		public DateTime OrderDate { get; set; }
		public ShippingInformation Shipping { get; set; }
		public BillingInformation Billing { get; set; }
		public List<OrderLineItem> LineItems { get; set; } 
		public double Total { get; set; }

		public void CalculateTotal()
		{
			Total = 0;
			foreach (var lineItem in LineItems)
			{
				Total += (lineItem.UnitPrice * lineItem.Quantity - lineItem.Discount);
			}
		}
	}
}