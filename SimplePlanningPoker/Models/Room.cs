using System.Collections.Concurrent;

namespace SimplePlanningPoker.Models
{
    /// <summary>
    /// Represents a room instance, which is associated with participants and their current estimates.
    /// This class is defined as thread-safe.
    /// </summary>
    public class Room
    {
        private readonly ConcurrentDictionary<string, User> participants;

        /// <summary>
        /// Creates a new room instance.
        /// </summary>
        /// <param name="roomId"></param>
        public Room(string roomId)
        {
            RoomId = roomId;
            participants = new ConcurrentDictionary<string, User>();
            State = new ChooseState(this);
        }

        /// <summary>
        /// The unique identifier of the room.
        /// </summary>
        public string RoomId { get; }
        public CardSet CardSet { get; private set; } = CardSets.Default;

        public RoomState State { get; private set; }

        #region Public Methods

        public bool AddParticipant(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var success = participants.TryAdd(user.ConnectionId, user);
            return success;
        }

        /// <summary>
        /// Removes a participant. Returns <see cref="true"/>, if successful.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public bool RemoveParticipant(User user)
        {
            var success = participants.TryRemove(user.ConnectionId, out _);
            return success;
        }

        /// <summary>
        /// Checks if the participant with the given id is member of this room.
        /// </summary>
        public bool ContainsParticipant(string participantId)
        {
            return participants.ContainsKey(participantId);
        }

        /// <summary>
        /// Gets the participant with the specified ID.
        /// </summary>
        /// <param name="participantId">The ID of the participant to get.</param>
        /// <returns>The participant with the specified ID.</returns>
        public User GetParticipant(string participantId)
        {
            if (participantId == null)
                throw new ArgumentNullException(nameof(participantId));

            participants.TryGetValue(participantId, out var participant);
            return participant ?? throw new UserNotFoundException(participantId);
        }
        /// <summary>
        /// Returns the participants.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllParticipants()
        {
            return participants.Values;
        }

        /// <summary>
        /// Returns the current estimates.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string?> GetEstimates()
        {
            return participants.Values?.Select(p => p.Estimate) ?? Enumerable.Empty<string?>();
        }

        /// <summary>
        /// Adds or updates an estimate. 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="estimate"></param>
        public void Estimate(string userId, string estimate)
        {
            ValidateEstimateValue(estimate);
            ValidateParticipantId(userId);

            participants.TryGetValue(userId, out var user);

            if (user == null)
                throw new UserNotFoundException(userId);

            user.Estimate = estimate;
        }

        private void ValidateParticipantId(string participantId)
        {
            if (participantId == null)
                throw new ArgumentNullException(nameof(participantId));
            if (!participants.ContainsKey(participantId))
                throw new ArgumentException($"Participant with token {participantId} not found.");
        }

        private void ValidateEstimateValue(string estimate)
        {
            if (estimate == null)
                throw new ArgumentNullException(nameof(estimate));
            if (!CardSet.Contains(estimate))
                throw new ArgumentException($"Estimate {estimate} is not in the card set.");
        }

        /// <summary>
        /// Reveals the estimates.
        /// </summary>
        public void Reveal()
        {
            State = new ShowState(this);
        }
        /// <summary>
        /// Resets the room. Clears all estimates.
        /// </summary>
        public void Reset()
        {
            State = new ChooseState(this);
            ResetEstimates();
        }

        /// <summary>
        /// Resets the estimates.
        /// </summary>
        private void ResetEstimates()
        {
            participants.Values
            .AsParallel()
            .ForAll(p => p.Estimate = null);
        }

        #endregion

    }

}

