using SafeScribe_cp05.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeScribe_cp05.Services
{
    //FUNÇÃO PARA A BLACKLIST FUNCIONAR
    public class InMemoryTokenBlacklistService : ITokenBlacklistService
    {
        private static readonly HashSet<string> _blacklistedJtis = new HashSet<string>();

        private static readonly object _lock = new object();

        public Task AddToBlacklistAsync(string jti)
        {
            lock (_lock)
            {
                _blacklistedJtis.Add(jti);
            }
            return Task.CompletedTask;
        }

        public Task<bool> IsBlacklistedAsync(string jti)
        {
            lock (_lock)
            {
                return Task.FromResult(_blacklistedJtis.Contains(jti));
            }
        }
    }
}