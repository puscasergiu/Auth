using System.Threading.Tasks;
using Auth.Core.Repositories;
using StackExchange.Redis;

namespace Auth.Infrastructure.Repositories
{
    public class RevokedTokenRepository : IRevokedTokenRepository
    {
        private readonly RedisKey _revokedTokensSetKey = new("RevokedTokens");
        private readonly IDatabase _database;

        public RevokedTokenRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task<bool> ExistsAsync(string token)
        {
            return await _database.SetContainsAsync(_revokedTokensSetKey, new RedisValue(token));
        }

        public async Task InsertAsync(string token)
        {
            await _database.SetAddAsync(_revokedTokensSetKey, new RedisValue(token));
        }
    }
}
