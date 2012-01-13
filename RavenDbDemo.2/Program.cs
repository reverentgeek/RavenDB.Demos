using System;
using System.Collections.Generic;
using Raven.Client.Document;
using Raven.Client.Shard;
using Raven.Client.Shard.ShardStrategy;
using Raven.Client.Shard.ShardStrategy.ShardAccess;
using Raven.Client.Shard.ShardStrategy.ShardResolution;
using Raven.Client.Shard.ShardStrategy.ShardSelection;
using RavenDB.Demo.Core;

namespace RavenDbDemo
{
	class Program
	{
		static void Main()
		{
			var shards = new Shards
			             	{
			             		new DocumentStore {Url = "http://localhost:8080", Identifier = "Posts"},
			             		new DocumentStore
			             			{
			             				Url = "http://localhost:8081",
			             				Identifier = "Users",
			             				Conventions = {DocumentKeyGenerator = user => "users/" + ((User) user).Name}
			             			}
			             	};

			var shardStrategy = new ShardStrategy
			                    	{
			                    		ShardAccessStrategy = new ParallelShardAccessStrategy(),
			                    		ShardSelectionStrategy = new BlogShardSelectionStrategy(),
			                    		ShardResolutionStrategy = new BlogShardResolutionStrategy()
			                    	};

			using (var documentStore = new ShardedDocumentStore(shardStrategy, shards).Initialize())
			{
				using (var session = documentStore.OpenSession())
				{
					var user = new User {Name = "PastyGeek"};
					session.Store(user);
					session.SaveChanges();

					var post = new Post
					           	{
					           		AuthorId = user.Id,
									Name = user.Name,
					           		BlogId = "blogs/1",
					           		Title = "More CodeMash Gloating!",
									Body = "You wouldn't believe how much more fun I'm having than you!",
									PostDate = DateTime.Now,
									Tags = new List<string> { "codemash", "gloating" }
					           	};
					session.Store(post);
					session.SaveChanges();
				}
			}
		}
	}

	public class BlogShardSelectionStrategy : IShardSelectionStrategy
	{
		public string ShardIdForNewObject(object obj)
		{
			return GetShardIdFromObjectType(obj);
		}

		public string ShardIdForExistingObject(object obj)
		{
			return GetShardIdFromObjectType(obj);
		}

		private static string GetShardIdFromObjectType(object instance)
		{
			if (instance is User)
				return "Users";
			if (instance is Post)
				return "Posts";
			throw new ArgumentException("Cannot get shard id for '" + instance + "' because it is not a User or Post");
		}
	}

	public class BlogShardResolutionStrategy : IShardResolutionStrategy
	{
		public IList<string> SelectShardIds(ShardResolutionStrategyData srsd)
		{
			if (srsd.EntityType == typeof(User))
				return new[] { "Users" };
			if (srsd.EntityType == typeof(Post))
				return new[] { "Post" };

			throw new ArgumentException("Cannot get shard id for '" + srsd.EntityType + "' because it is not a User or Post");
		}
	}
}
