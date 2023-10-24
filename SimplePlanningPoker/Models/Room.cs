using System.Collections.Concurrent;

namespace SimplePlanningPoker.Models
{
    /// <summary>
    /// Represents a room instance, which is associated with participants and their current estimates.
    /// This class is defined as thread-safe.
    /// </summary>
    public class Room
    {
        public string RoomId { get; }
        private readonly ConcurrentDictionary<string, User> participants;
        private ConcurrentDictionary<string, string> estimates;

        public Room(string roomId)
        {
            RoomId = roomId;
            participants = new ConcurrentDictionary<string, User>();
            estimates = new ConcurrentDictionary<string, string>();
        }

        /// <summary>
        /// Adds a partipant. Returns <see cref="true"/>, if successful.
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public bool AddParticipant(User participant)
        {
            return participants.TryAdd(participant.Token, participant);
        }

        /// <summary>
        /// Removes a participant. Returns <see cref="true"/>, if successful.
        /// </summary>
        /// <param name="participantId"></param>
        /// <returns></returns>
        public bool RemoveParticipant(string participantId)
        {
            return participants.TryRemove(participantId, out _);
        }

        public IEnumerable<User> GetParticipants()
        {
            return participants.Values;
        }

        public EstimationResult Estimate(string participantId, string estimate)
        {
            var added = estimates.TryAdd(participantId, estimate);
            if (added)
            {
                return EstimationResult.Added;
            }
            estimates.TryGetValue(participantId, out var currentEstimate);
            if (currentEstimate == null)
            {
                return EstimationResult.Failed;
            }
            else
            {
                return estimates.TryUpdate(participantId, estimate, currentEstimate)
                    ? EstimationResult.Updated
                    : EstimationResult.Failed;
            }
        }

        public void ResetEstimates()
        {
            estimates = new ConcurrentDictionary<string, string>();
        }
    }

    public enum EstimationResult
    {
        Added,
        Updated,
        Failed
    }

}

