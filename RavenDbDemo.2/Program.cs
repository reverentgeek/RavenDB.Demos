using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client.Document;
using RavenDB.Demo.Core;

namespace RavenDbDemo
{
	class Program
	{
		static void Main()
		{
			using (var documentStore = new DocumentStore { Url = "http://localhost:8080" }.Initialize())
			{
				using (var session = documentStore.OpenSession())
				{
					for(var i = 0; i < 10; i++)
					{
						var post = new Post
						{
							BlogId = "blogs/1",
							AuthorId = "users/PastyGeek",
							Name = "PastyGeek",
							PostDate = DateTime.Now.AddHours(-1).AddMinutes(i),
							Title = "Glutonous Gloating CodeMash Post " + i,
							Body = "You wouldn't believe how much more fun I'm having than you!",
							Tags = new List<string> { "tag" + i, "codemash" }
						};
						session.Store(post);
						session.SaveChanges();
					}

					Console.WriteLine("Query for all posts.");
		
					var posts = session.Query<Post>().OrderByDescending(p => p.PostDate).ToList();

					foreach(var post in posts)
						Console.WriteLine("Post ID: {0}, Title: {1}", post.Id, post.Title);

					Console.WriteLine("Query posts by tag.");
					
					var postsByTag = session.Query<Post>()
						.Where(p => p.Tags.Any(t => t == "tag1"));

					foreach (var post in postsByTag)
						Console.WriteLine("Post ID: {0}, Title: {1}, Tags: {2}",
							post.Id, post.Title, string.Join(", ", post.Tags.ToArray()));

					Console.ReadKey(true);
				}
			}
		}

	}
}
