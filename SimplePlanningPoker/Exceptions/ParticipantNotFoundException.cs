namespace SimplePlanningPoker;

/// <summary>
/// The exception that is thrown when a user with a given ID is not found.
/// </summary>
public class UserNotFoundException : Exception
{
    /// <summary>
    /// Exception thrown when a user with a given ID cannot be found.
    /// </summary>
    public UserNotFoundException(string userId)
        : base($"User with ID {userId} not found.")
    {
    }

}
