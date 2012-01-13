using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RavenDB.Demo.Core
{
	public class Comment
	{
		public DateTime CommentDate { get; set; }
		public string Email { get; set; }
		public string Name { get; set; }
		public string Text { get; set; }
	}
}
