using System;
using System.Collections.Generic;
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
					var post = new Post
								{
									BlogId = "blogs/1",
									AuthorId = "users/PastyGeek",
									Name = "PastyGeek",
									PostDate = DateTime.Now.AddHours(-1),
									Title = "Having a blast at CodeMash!",
									Body = "Hanselman! Bacon! Swimming! You missed the 20 minute ticket window! Ha ha!",
									Tags = new List<string> { "bacon", "codemash", "conferences", "gloating" }
								};

					session.Store(post);
					session.SaveChanges();

					Console.WriteLine("Created initial blog post. Press any key...");
					Console.ReadKey(true);

					var existingPost = session.Load<Post>(post.Id);

					existingPost.Comments.Add(new Comment
					{
						CommentDate = DateTime.Now.AddMinutes(-30),
						Email = "bitter@work.com",
						Name = "Grumpy Slow Guy",
						Text = "Thanks for rubbing salt into my wound."
					});

					existingPost.Comments.Add(new Comment
					{
						CommentDate = DateTime.Now.AddMinutes(-15),
						Email = "bragger@baconparty.com",
						Name = "PastyGeek",
						Text = "LOL!"
					});

					session.Store(existingPost);
					session.SaveChanges();
				}
			}
		}
	}
}
