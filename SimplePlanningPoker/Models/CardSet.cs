using System;
namespace SimplePlanningPoker.Models
{
	public class CardSet
	{
		public required string Name { get; set; }
		public required IList<string> Values { get; set; }
	}

	public static class CardSets
	{
		public static readonly CardSet Default = new CardSet { Name = "Default", Values = new List<string> { "", "" } };
		public static readonly CardSet[] Sets = { Default };
	}
}

