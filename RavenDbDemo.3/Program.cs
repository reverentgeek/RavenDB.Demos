using System;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
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
					IndexCreation.CreateIndexes(typeof(Posts_ByTitle).Assembly, documentStore);

					var postsBySearch = session.Query<Post, Posts_ByTitle>()
						.Search(x => x.Title, "Code*")
						.OrderByDescending(x => x.PostDate).ToList();

					foreach(var post in postsBySearch)
						Console.WriteLine("Post ID: {0}, Title: {1}", post.Id, post.Title);

					Console.ReadKey(true);
				}
			}
		}

	}

	public class Posts_ByTitle : AbstractIndexCreationTask<Post>
	{
		public Posts_ByTitle()
		{
			Map = posts => from post in posts
							  select new { post.Title, post.PostDate };
			Index(x => x.Title, FieldIndexing.Analyzed);
		}
	}
}
