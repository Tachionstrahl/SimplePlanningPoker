using System.Collections.Concurrent;

namespace SimplePlanningPoker.Models
{  
    public class Room
    {
        public string RoomId { get; }
        private readonly ConcurrentDictionary<string, string> participants;

        public Room(string roomId)
        {
            RoomId = roomId;
            participants = new ConcurrentDictionary<string, string>();
        }

        public bool AddParticipant(string participantId, string participantName)
        {
            return participants.TryAdd(participantId, participantName);
        }

        public bool RemoveParticipant(string participantId)
        {
            return participants.TryRemove(participantId, out _);
        }


        public IEnumerable<string> GetParticipants()
        {
            return participants.Values;
        }
    }

}

