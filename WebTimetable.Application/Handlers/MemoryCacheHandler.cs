using Microsoft.Extensions.Caching.Memory;
using WebTimetable.Application.Handlers.Abstractions;

namespace WebTimetable.Application.Handlers;

public class MemoryCacheHandler : MemoryCache, ICacheHandler
{
        private readonly MemoryCacheEntryOptions _defaultCacheConfiguration;
        
        public MemoryCacheHandler()
            : base(new MemoryCacheOptions())
        {
            _defaultCacheConfiguration = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12),
                SlidingExpiration = TimeSpan.FromHours(12)
            };
        }
        
        /// <summary>
        /// Try to retrieve cached value by parameters that are used as a composite key.
        /// </summary>
        /// <param name="value">The value associated with the given key.</param>
        /// <param name="compositeKey">The objects that represent the composite key.</param>
        /// <returns>True if the key was found. False otherwise.</returns>
        public bool TryRetrieveCache<T>(out T value, params object[] compositeKey)
        {
            var keyName = string.Join("|", compositeKey);
            return this.TryGetValue(keyName, out value);
        }
        
        /// <summary>
        /// Sets the cache value by parameters that are used as a composite key.
        /// </summary>
        /// <param name="value">The value to associate with the key.</param>
        /// <param name="compositeKey">The objects that represent the composite key.</param>
        public void SetCache<T>(T value, params object[] compositeKey)
        {
            var keyName = string.Join("|", compositeKey);
            this.Set(keyName, value, _defaultCacheConfiguration);
        }
}