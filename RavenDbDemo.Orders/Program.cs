using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;

namespace RavenOrders
{
	class Program
	{
		static void Main()
		{
			using (var documentStore = new DocumentStore { Url = "http://localhost:8080" }.Initialize())
			{
				//Console.WriteLine("Initializing database...");
				// InitializeDatabase(documentStore);
				
				using(var session = documentStore.OpenSession())
				{
					Console.WriteLine("Load by document ID...");
					var order = session.Load<Order>("orders/225");
					Console.WriteLine("{0} - {1} - {2:c}", order.Id, order.OrderDate, order.Total);

					//Console.WriteLine("Query by field...");
					//var products = session.Query<Product>().Where(x => x.Name.Contains("RavenDB")).OrderByDescending(x => x.Name).ToList();
					//foreach (var product in products)
					//    Console.WriteLine(product.Name);

					//Console.WriteLine("Index Search (Lucene)...");
					//var productsBySearch = session.Query<Product, Products_ByName>().Search(x => x.Name, "*ug").ToList();
					//foreach (var product in productsBySearch)
					//    Console.WriteLine(product.Name);
				}
				Console.WriteLine("");
				Console.WriteLine("Press any key...");
				Console.ReadKey(true);
			}
		}

		private static void InitializeDatabase(IDocumentStore documentStore)
		{
			ClearDatabase(documentStore);
			var products = CreateProducts(documentStore);
			var customer = CreateCustomer(documentStore);
			var order = CreateOrder(documentStore, customer, products);
			IndexCreation.CreateIndexes(typeof(Products_ByName).Assembly, documentStore);
		}

		private static void ClearDatabase(IDocumentStore documentStore)
		{
			using(var session = documentStore.OpenSession())
			{
				var orders = session.Advanced.DatabaseCommands.StartsWith("orders", 0, 50);
				foreach(var order in orders)
					session.Advanced.DatabaseCommands.Delete(order.Key, order.Etag);

				var products = session.Advanced.DatabaseCommands.StartsWith("products", 0, 50);
				foreach (var product in products)
					session.Advanced.DatabaseCommands.Delete(product.Key, product.Etag);

				var customers = session.Advanced.DatabaseCommands.StartsWith("customers", 0, 50);
				foreach (var customer in customers)
					session.Advanced.DatabaseCommands.Delete(customer.Key, customer.Etag);

				//var products = from p in session.Query<Product>() select p;
				//foreach(var product in products)
				//    session.Delete(product);

				//var orders = session.Query<Order>();
				//foreach(var order in orders)
				//    session.Delete(order);

				//var customers = session.Query<Customer>();
				//foreach(var customer in customers)
				//    session.Delete(customer);

				session.SaveChanges();
			}
		}

		private static List<Product> CreateProducts(IDocumentStore documentStore)
		{
			var products = new List<Product>();
			products.Add(new Product
			             	{
			             		Name = "RavenDB Shirt",
			             		UnitPrice = 15.0d,
								Attributes = new List<ProductAttribute> { 
									new ProductAttribute { Name = "Color", Values = new [] { "Black", "White" } },
									new ProductAttribute { Name = "Size", Values = new [] { "S", "M", "L", "XL", "XXL" }}
								}
			             	});
			products.Add(new Product 
							{
								Name = "RavenDB Mug",
								UnitPrice = 10.0d,
								Attributes = new List<ProductAttribute> { 
									new ProductAttribute { Name = "Color", Values = new [] { "Black", "White", "Red" } }
								}
							});
	
			using (var session = documentStore.OpenSession())
			{
				foreach(var product in products)
					session.Store(product);
				session.SaveChanges();
			}

			return products;
		}

		private static Order CreateOrder(IDocumentStore documentStore, Customer customer, IList<Product> products)
		{
			var order = new Order
			            	{
			            		CustomerId = customer.Id,
			            		Billing =
			            			new BillingInformation
			            				{
			            					BillToAddress = customer.Address,
			            					BillToName = customer.FullName,
			            					PaymentMethod = "VISA",
			            					ConfirmationCode = "123456"
			            				},
			            		Shipping = new ShippingInformation
			            		           	{
			            		           		ShippingMethod = "Fedex 2-day",
			            		           		ShipToAddress = customer.Address,
												ShipToName = customer.FullName
			            		           	}
			            	};

			foreach(var product in products)
			{
				order.Total += product.UnitPrice;
				order.LineItems.Add(new OrderLineItem
				                    	{
				                    		ProductId = product.Id,
											ProductName = product.Name + " - " + product.Attributes[0].Values[1],
											Quantity = 1,
											UnitPrice = product.UnitPrice
				                    	});
			}

			order.CalculateTotal();

			using (var session = documentStore.OpenSession())
			{
				session.Store(order);
				session.SaveChanges();
			}

			return order;
		}

		private static Customer CreateCustomer(IDocumentStore documentStore)
		{
			var customer = new Customer
			               	{
			               		FirstName = "John",
			               		LastName = "Smith",
			               		EmailAddress = "john.smith@test.com",
			               		Address =
			               			new Address
			               				{
			               					Address1 = "123 Elm Street",
			               					City = "Nashville",
			               					State = "TN",
			               					PostalCode = "37214",
			               					Country = "USA"
			               				}
			               	};
			using (var session = documentStore.OpenSession())
			{
				session.Store(customer);
				session.SaveChanges();
			}
			return customer;
		}
	}

	public class Products_ByName : AbstractIndexCreationTask<Product>
	{
		public Products_ByName()
		{
			Map = products => from product in products
			                  select new {product.Name};
			Index(x => x.Name, FieldIndexing.Analyzed);
		}
	}
}
