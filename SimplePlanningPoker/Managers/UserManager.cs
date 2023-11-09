using System.Collections.Concurrent;
using SimplePlanningPoker.Models;
namespace SimplePlanningPoker.Managers
{
    public class UserManager : IUserManager
    {
        private readonly ConcurrentDictionary<string, User> users;

        public UserManager()
        {
            users = new ConcurrentDictionary<string, User>();
        }

        /// <summary>
        /// Returns all users.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetAllUsers()
        {
            return users.Values;
        }
        
        /// <summary>
        /// Tries to get a user by its id.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool TryGetUser(string userId, out User? user)
        {
            return users.TryGetValue(userId, out user);
        }

        /// <summary>
        /// Checks if a user with the given id exists.
        /// </summary>
        public bool ContainsUser(string userId)
        {
            return users.ContainsKey(userId);
        }

        /// <summary>
        /// Adds a new user to the user manager.
        /// </summary>
        public bool TryAddUser(User user)
        {
            return users.TryAdd(user.ConnectionId, user);
        }

        /// <summary>
        /// Tries to remove a user from the user manager.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool TryRemoveUser(string userId, out User? user)
        {
            return users.TryRemove(userId, out user);
        }
    }

}

