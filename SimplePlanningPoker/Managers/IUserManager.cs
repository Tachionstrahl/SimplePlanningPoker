using SimplePlanningPoker.Models;

namespace SimplePlanningPoker;

/// <summary>
/// A manager for users.
/// </summary>
public interface IUserManager
{
    /// <summary>
    /// Returns all users.
    /// </summary>
    /// <returns></returns>
    IEnumerable<User> GetAllUsers();

    /// <summary>
    /// Tries to get a user by its id.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    bool TryGetUser(string userId, out User? user);

    /// <summary>
    /// Checks if a user with the given id exists.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    bool ContainsUser(string userId);

    /// <summary>
    /// Adds a new user to the user manager.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    bool TryAddUser(User user);

    /// <summary>
    /// Tries to remove a user from the user manager.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    bool TryRemoveUser(string userId, out User? user);

}
