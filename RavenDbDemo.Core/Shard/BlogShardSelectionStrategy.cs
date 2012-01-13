using System;
using System.Collections.Generic;
using Raven.Client.Shard.ShardStrategy.ShardResolution;
using Raven.Client.Shard.ShardStrategy.ShardSelection;

namespace RavenDB.Demo.Core.Shard
{
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
