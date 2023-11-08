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

        public IEnumerable<User> GetAllUsers()
        {
            return users.Values;
        }

        public bool TryGetUser(string userId, out User? user)
        {
            return users.TryGetValue(userId, out user);
        }
        public bool ContainsUser(string userId)
        {
            return users.ContainsKey(userId);
        }

        public bool TryRemoveUser(string userId, out User? user)
        {
            return users.TryRemove(userId, out user);
        }

        public bool TryAddUser(User user)
        {
            return users.TryAdd(user.ConnectionId, user);
        }
    }

}

