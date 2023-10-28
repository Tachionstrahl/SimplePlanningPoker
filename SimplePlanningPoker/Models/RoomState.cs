using System;
namespace SimplePlanningPoker.Models
{
    /// <summary>
    /// Represents the state of a planning poker room.
    /// To be used in the hub and the client.
    /// </summary>
	public abstract class RoomState
    {
        protected readonly Room room;
        public RoomState(Room room)
        {
            this.room = room;

        }
        public abstract RoomStateName RoomStateName { get; }
        /// <summary>
        /// Gets the ID of the room.
        /// </summary>
		public string RoomId => room.RoomId;

        /// <summary>
        /// Gets the card set used in the room.
        /// </summary>
        public CardSet CardSet => room.CardSet;

        /// <summary>
        /// Gets the list of participants in the room.
        /// </summary>
        public IEnumerable<string> Participants => room.GetAllParticipants().Select(p => p.User.Name);
    }

    public enum RoomStateName
    {
        Choose,
        Show
    }

    /// <summary>
    /// Represents the state of a planning poker room during the "choose" phase.
    /// </summary>
    public class ChooseState : RoomState
    {
        public ChooseState(Room room) : base(room)
        {
        }

        public override RoomStateName RoomStateName => RoomStateName.Choose;
    }

    /// <summary>
    /// Represents the state of a planning poker room during the "show" phase.
    /// </summary>
    public class ShowState : RoomState
    {
        public ShowState(Room room) : base(room)
        {
        }

        /// <summary>
        /// Gets the estimates made by the participants in the room.
        /// </summary>
        public IDictionary<string, string?> Estimates =>
        room.GetAllParticipants()
            .Select(p => new { p.User.Name, p.Estimate })
            .ToDictionary(e => e.Name, e => e.Estimate);

        public override RoomStateName RoomStateName => RoomStateName.Show;
    }

}

