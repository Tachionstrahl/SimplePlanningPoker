using System.Collections.Concurrent;

namespace SimplePlanningPoker.Models
{
    /// <summary>
    /// Represents a room instance, which is associated with participants and their current estimates.
    /// This class is defined as thread-safe.
    /// </summary>
    public class Room
    {
        private readonly ConcurrentDictionary<string, Participant> participants;

        /// <summary>
        /// Creates a new room instance.
        /// </summary>
        /// <param name="roomId"></param>
        public Room(string roomId)
        {
            RoomId = roomId;
            participants = new ConcurrentDictionary<string, Participant>();
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

            var success = participants.TryAdd(user.ConnectionId, new Participant { User = user });
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

        public bool ContainsParticipant(string participantId)
        {
            return participants.ContainsKey(participantId);
        }

        /// <summary>
        /// Gets the participant with the specified ID.
        /// </summary>
        /// <param name="participantId">The ID of the participant to get.</param>
        /// <returns>The participant with the specified ID.</returns>
        public Participant GetParticipant(string participantId)
        {
            if (participantId == null)
                throw new ArgumentNullException(nameof(participantId));

            participants.TryGetValue(participantId, out var participant);
            return participant ?? throw new ParticipantNotFoundException(participantId);
        }
        /// <summary>
        /// Returns the participants.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Participant> GetAllParticipants()
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
        /// <param name="participantId"></param>
        /// <param name="estimate"></param>
        /// <returns><see cref="EstimationResult.Added"/>, if added.
        /// <see cref="EstimationResult.Updated"/>, if updated.
        /// <see cref="EstimationResult.Failed"/>, if failed.</returns>
        public EstimationResult Estimate(string participantId, string estimate)
        {
            if (participantId == null)
                throw new ArgumentNullException(nameof(participantId));
            if (estimate == null)
                throw new ArgumentNullException(nameof(estimate));
            if (!CardSet.Contains(estimate))
                throw new ArgumentException($"Estimate {estimate} is not in the card set.");
            if (!participants.ContainsKey(participantId))
                throw new ArgumentException($"Participant with token {participantId} not found.");

            participants.TryGetValue(participantId, out var participant);

            if (participant == null)
                return EstimationResult.Failed;

            participant.Estimate = estimate;

            return EstimationResult.Success;
        }

        /// <summary>
        /// Resets the estimates.
        /// </summary>
        public void ResetEstimates()
        {
            participants.Values
            .AsParallel()
            .ForAll(p => p.Estimate = null);
        }

        public void Reveal()
        {
            State = new ShowState(this);
        }

        public void Reset()
        {
            State = new ChooseState(this);
            ResetEstimates();
        }

        #endregion

    }

}

