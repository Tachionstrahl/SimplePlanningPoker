using System;
namespace SimplePlanningPoker.Models
{
    /// <summary>
    /// Represents the state of a planning poker room.
    /// To be used in the hub and the client.
    /// </summary>
	public interface IRoomState
    {
        /// <summary>
        /// Gets or sets the ID of the room.
        /// </summary>
		public string RoomId { get; }

        /// <summary>
        /// Gets or sets the card set used in the room.
        /// </summary>
        public CardSet CardSet { get; }

        /// <summary>
        /// Gets or sets the list of participants in the room.
        /// </summary>
        public IEnumerable<string> Participants { get; }
    }

    /// <summary>
    /// Represents the state of a planning poker room during the "choose" phase.
    /// </summary>
    public interface IChooseState : IRoomState
    {
    }

    /// <summary>
    /// Represents the state of a planning poker room during the "show" phase.
    /// </summary>
    public interface IShowState : IRoomState
    {
        /// <summary>
        /// Gets or sets the estimates made by the participants in the room.
        /// </summary>
        public IDictionary<string, string> Estimates { get; }
    }
}

