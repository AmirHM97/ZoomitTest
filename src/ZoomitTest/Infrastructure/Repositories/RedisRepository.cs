using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ZoomitTest.Infrastructure.Repositories
{
    public interface IRedisRepository
    {
        Task AddAsync<T>(string key, T value, When when = When.Always, CommandFlags flag = CommandFlags.None) where T : class;
        Task<T> GetAsync<T>(string key, CancellationToken token = default) where T : class;
    }

    public class RedisRepository : IRedisRepository
    {
        private readonly IDistributedCache _distributedCache;

        public RedisRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task AddAsync<T>(string key, T value, When when = When.Always, CommandFlags flag = CommandFlags.None) where T : class
        {
            if (value is not null)
            {
                var bytes = JsonSerializer.SerializeToUtf8Bytes(value);
                await _distributedCache.SetAsync(key, bytes);
            }
        }
        public async Task<T> GetAsync<T>(string key, CancellationToken token = default) where T : class
        {
            var result = await _distributedCache.GetAsync(key, token);
            return string.IsNullOrEmpty(result?.ToString()) ? (T)null : JsonSerializer.Deserialize<T>(result);
        }
    }
}
