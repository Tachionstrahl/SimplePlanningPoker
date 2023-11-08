using System;
namespace SimplePlanningPoker.Models
{
    public record User(
        string ConnectionId, string Name, string? Estimate)
    {
        public User(string ConnectionId, string Name): this(ConnectionId, Name, null)
		{
			
		}
    }

}

