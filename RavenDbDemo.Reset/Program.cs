using Raven.Client.Document;
using Raven.Client.Shard;
using Raven.Client.Shard.ShardStrategy;
using Raven.Client.Shard.ShardStrategy.ShardAccess;
using RavenDB.Demo.Core;
using RavenDB.Demo.Core.Shard;

namespace RavenDbDemo.Reset
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
				var posts = shards[0].DatabaseCommands.StartsWith("post", 0, 50);
				foreach (var post in posts) shards[0].DatabaseCommands.Delete(post.Key, post.Etag);

				var users = shards[1].DatabaseCommands.StartsWith("user", 0, 50);
				foreach (var user in users) shards[1].DatabaseCommands.Delete(user.Key, user.Etag);
			}
		}
	}
}
