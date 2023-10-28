namespace SimplePlanningPoker;

/// <summary>
/// The exception that is thrown when a participant with a given ID is not found.
/// </summary>
public class ParticipantNotFoundException : Exception
{
    /// <summary>
    /// Exception thrown when a participant with a given ID cannot be found.
    /// </summary>
    public ParticipantNotFoundException(string participantId)
        : base($"Participant with ID {participantId} not found.")
    {
    }

}
