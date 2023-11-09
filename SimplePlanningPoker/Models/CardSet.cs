using System;
namespace SimplePlanningPoker.Models
{
	/// <summary>
	/// A defined set of planning poker cards
	/// </summary>
	public class CardSet
	{
		/// <summary>
		/// The name of the card set.
		/// </summary>
		public required string Name { get; set; }
		/// <summary>
		/// The available values in the card set.
		/// </summary>
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

	/// <summary>
	/// Static defined <see cref="CardSet"/>s, ready to use.
	/// </summary>
	public static class CardSets
	{
		/// <summary>
		/// Gets the default card set.
		/// </summary>
		public static readonly CardSet Default = new() { Name = "Default", Values = new List<string> { "0", "0.5", "1", "2", "3", "5", "8", "13", "20", "40", "100", "?", "☕", "∞" } };
		/// <summary>
		/// Gets all defined card sets.
		/// </summary>
		public static readonly CardSet[] Sets = { Default };
	}
}

