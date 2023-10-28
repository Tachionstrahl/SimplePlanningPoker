using System;
namespace SimplePlanningPoker.Models
{
	public class CardSet
	{
		public required string Name { get; set; }
		public required  IEnumerable<string> Values { get; set; }

		/// <summary>
		/// Checks if a given value is part for this card set.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>True if the value is valid, false otherwise.</returns>
		public bool Contains(string value) {
			if (value == null) {
				throw new ArgumentNullException(nameof(value));
			}
			return Values.Contains(value);
		}
	}

	public static class CardSets
	{
		public static readonly CardSet Default = new() { Name = "Default", Values = new List<string> { "0", "0.5", "1", "2", "3", "5", "8", "13", "20", "40", "100", "?", "☕", "∞" } };
		public static readonly CardSet[] Sets = { Default };
	}
}

