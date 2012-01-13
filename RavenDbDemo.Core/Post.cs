using System;
using System.Collections.Generic;

namespace RavenDB.Demo.Core
{
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
}
