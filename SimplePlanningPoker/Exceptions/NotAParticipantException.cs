namespace SimplePlanningPoker;

public class NotAParticipantException : Exception
{
    public NotAParticipantException(string roomId) 
    : base($"You are not a participant of {roomId}. Make sure to join the room first.")
    {
    }
}
