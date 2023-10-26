using System.Collections.Concurrent;
using System.Collections.Immutable;

namespace SimplePlanningPoker.Models
{
    /// <summary>
    /// Represents a room instance, which is associated with participants and their current estimates.
    /// This class is defined as thread-safe.
    /// </summary>
    public class Room : IRoomState, IShowState, IChooseState
    {

        private readonly ConcurrentDictionary<string, User> participants;
        private ConcurrentDictionary<string, string> estimates;

        /// <summary>
        /// Creates a new room instance.
        /// </summary>
        /// <param name="roomId"></param>
        public Room(string roomId)
        {
            RoomId = roomId;
            participants = new ConcurrentDictionary<string, User>();
            estimates = new ConcurrentDictionary<string, string>();
        }

        /// <summary>
        /// The unique identifier of the room.
        /// </summary>
        public string RoomId { get; }

        public CardSet CardSet { get; set; }

        public IEnumerable<string> Participants => participants.Select(x => x.Value.Name);

        public IDictionary<string, string> Estimates => throw new NotImplementedException();

        public bool AddParticipant(User participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));
                
            return participants.TryAdd(participant.Id, participant);
        }

        /// <summary>
        /// Removes a participant. Returns <see cref="true"/>, if successful.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public bool RemoveParticipant(User participant)
        {
            return participants.TryRemove(participant.Id, out _);
        }

        /// <summary>
        /// Returns the participants.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetParticipants()
        {
            return participants.Values;
        }

        /// <summary>
        /// Returns the current estimates.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetEstimates()
        {
            return estimates.Values.ToImmutableArray();
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
            if (!participants.ContainsKey(participantId))
            {
                throw new ArgumentException($"Participant with token {participantId} not found.");
            }
            var added = estimates.TryAdd(participantId, estimate);
            if (added)
            {
                return EstimationResult.Success;
            }
            estimates.TryGetValue(participantId, out var currentEstimate);
            if (currentEstimate == null)
            {
                return EstimationResult.Failed;
            }
            else
            {
                return estimates.TryUpdate(participantId, estimate, currentEstimate)
                    ? EstimationResult.Success
                    : EstimationResult.Failed;
            }
        }

        /// <summary>
        /// Resets the estimates.
        /// </summary>
        public void ResetEstimates()
        {
            estimates = new ConcurrentDictionary<string, string>();
        }
    }

    /// <summary>
    /// Result of an estimate operation.
    /// </summary>
    public enum EstimationResult
    {
        /// <summary>
        /// The estimate was added.
        /// </summary>
        Success,
        /// <summary>
        /// The estimate failed.
        /// </summary>
        Failed
    }

}

