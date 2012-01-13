using System;
using System.Collections.Generic;
using Raven.Client.Document;

namespace RavenDbDemo
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var documentStore = new DocumentStore { Url = "http://localhost:8080" }.Initialize())
			{
				using (var session = documentStore.OpenSession())
				{
					//var post = new Post
					//            {
					//                BlogId = "blogs/1",
					//                AuthorId = "users/3",
					//                Name = "Pasty Geek",
					//                PostDate = DateTime.Now.AddHours(-1),
					//                Title = "Having a blast at CodeMash!",
					//                Body = "Hanselman! Bacon! Swimming! You missed the 20 minute ticket window! Ha ha!",
					//                Tags = new List<string> {"bacon", "codemash", "conferences", "gloating"}
					//            };

					//post.Comments.Add(new Comment
					//{
					//    CommentDate = DateTime.Now.AddMinutes(-30),
					//    Email = "bitter@work.com",
					//    Name = "Grumply Slow Guy",
					//    Text = "Thanks for rubbing salt into my wound."
					//});

					//post.Comments.Add(new Comment
					//{
					//    CommentDate = DateTime.Now.AddMinutes(-15),
					//    Email = "bragger@baconparty.com",
					//    Name = "Pasty Geek",
					//    Text = "LOL!"
					//});

					//session.Store(post);

					//session.SaveChanges();

					//Console.ReadKey(true);
				}
			}
		}
	}

	public class Post
	{
		public string Id { get; set; }
		public string BlogId { get; set; }
		public string AuthorId { get; set; }
		public string Name { get; set; }
		public DateTime PostDate { get; set; }
		public string Title { get; set; }
		public string Body { get; set; }
		public IList<string> Tags { get; set; }
		public IList<Comment> Comments { get; set; }

		public Post()
		{
			Tags = new List<string>();
			Comments = new List<Comment>();
		}
	}

	public class Comment
	{
		public DateTime CommentDate { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string Text { get; set; }
	}
}
