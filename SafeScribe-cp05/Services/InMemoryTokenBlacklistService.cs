using SafeScribe_cp05.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SafeScribe_cp05.Services
{
    // A implementação usa memória (HashSet) para armazenar os JTIs revogados.
    public class InMemoryTokenBlacklistService : ITokenBlacklistService
    {
        // HashSet é ideal para pesquisa rápida (O(1))
        private static readonly HashSet<string> _blacklistedJtis = new HashSet<string>();

        // Lock para garantir que o acesso ao HashSet seja thread-safe
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