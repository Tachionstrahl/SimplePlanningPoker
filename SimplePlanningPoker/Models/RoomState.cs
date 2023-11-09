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
        public abstract IEnumerable<ParticipantDto> Participants { get; }
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

        public override IEnumerable<ParticipantDto> Participants => room.GetAllParticipants()
        .Select(p => new ParticipantDto(p.User.Name, p.Estimated, null));
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
        /// Gets the list of participants in the room with estimates.
        /// </summary>
        public override IEnumerable<ParticipantDto> Participants => room.GetAllParticipants()
        .Select(p => new ParticipantDto(p.User.Name, p.Estimated, p.Estimate));
        public override RoomStateName RoomStateName => RoomStateName.Show;
    }

}

