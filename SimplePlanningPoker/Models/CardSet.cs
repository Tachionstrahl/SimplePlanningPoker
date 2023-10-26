using System;
namespace SimplePlanningPoker.Models
{
	public class CardSet
	{
		public required string Name { get; set; }
		public required IList<string> Values { get; set; }

		/// <summary>
		/// Checks if a given value is valid for this card set.
		/// </summary>
		/// <param name="value">The value to check.</param>
		/// <returns>True if the value is valid, false otherwise.</returns>
		public bool IsValidValue(string value) {
			if (value == null) {
				throw new ArgumentNullException(nameof(value));
			}
			return Values.Contains(value);
		}
	}

	public static class CardSets
	{
		public static readonly CardSet Default = new CardSet { Name = "Default", Values = new List<string> { "", "" } };
		public static readonly CardSet[] Sets = { Default };
	}
}

