using System.Collections.Concurrent;
namespace SimplePlanningPoker.Managers
{
    

    public class UserManager
    {
        private readonly ConcurrentDictionary<string, string> userTokens;

        public UserManager()
        {
            userTokens = new ConcurrentDictionary<string, string>();
        }

        public async Task<string?> GenerateUserTokenAsync()
        {
            string token = Guid.NewGuid().ToString();
            return await Task.Run(() => userTokens.TryAdd(token, token) ? token : null);
        }

        public Task<bool> IsValidUserTokenAsync(string token)
        {
            return Task.Run(() => userTokens.ContainsKey(token));
        }

        public Task<bool> RemoveUserTokenAsync(string token)
        {
            return Task.Run(() => userTokens.TryRemove(token, out _));
        }
    }

}

