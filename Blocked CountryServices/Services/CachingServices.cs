using Blocked_CountryCore.Interfaces;
using Blocked_CountryCore.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocked_CountryServices.Services
{
    public class CachingServices : ICachingServices
    {
        private readonly IMemoryCache _memoryCache;
        private const string BlocedCountryKeys = "BlocedCountryKeys";
        private const string LogAttemptKeys = nameof(LogAttemptKeys);
        // private readonly ConcurrentDictionary<string, Country> _blockedCountries = new ();
        // private readonly List<LogAttmpt> logAttmpts = new List<LogAttmpt>();
        public CachingServices(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _memoryCache.Set(BlocedCountryKeys, new ConcurrentDictionary<string, Country>());
            _memoryCache.Set(LogAttemptKeys, new List<LogAttmpt>());
        }

        public async Task<IReadOnlyList<Country>> GetBlockedCountriesAsync()
        {
            var blockedCountries = _memoryCache.GetOrCreate(BlocedCountryKeys, entry => new ConcurrentDictionary<string, Country>());
            return await Task.FromResult(blockedCountries.Select(c => c.Value).ToList());
        }

        public Task BlockCountryAsync(string countryCode, int? durationMinutes = null)
        {
            var blockedCountries = _memoryCache.GetOrCreate(BlocedCountryKeys, entry => new ConcurrentDictionary<string, Country>());
            blockedCountries.TryAdd(countryCode, new Country() { Code = countryCode, Expiratedata = durationMinutes == null ? null : DateTime.Now.AddMinutes(durationMinutes.Value) });
            _memoryCache.Set(BlocedCountryKeys, blockedCountries);
            return Task.CompletedTask;

        }

        public async Task UnblockCountryAsync(string countryCode)
        {
            var blockedCountries = _memoryCache.GetOrCreate(BlocedCountryKeys, entry => new ConcurrentDictionary<string, Country>());
            blockedCountries.TryRemove(countryCode, out Country country);
            _memoryCache.Set(BlocedCountryKeys, blockedCountries);
            await Task.CompletedTask;
        }

        public async Task Logattempt(LogAttmpt logAttmpt)
        {
            var logAttmpts = _memoryCache.GetOrCreate(LogAttemptKeys, entry => new List<LogAttmpt>());
            logAttmpts.Add(logAttmpt);
            _memoryCache.Set(LogAttemptKeys, logAttmpts);
            await Task.CompletedTask;
        }

        public async Task<List<LogAttmpt>> Getlogs()
        {
            await Task.CompletedTask;
            var logAttmpts = _memoryCache.GetOrCreate(LogAttemptKeys, entry => new List<LogAttmpt>());
            return logAttmpts.ToList();

        }

        public async Task RemoveTemporalBlockedCountries()
        {
            var result = new ConcurrentDictionary<string, Country>();
            var blockedCountries = _memoryCache.GetOrCreate(BlocedCountryKeys, entry => new ConcurrentDictionary<string, Country>());
            var newblocedCountries = blockedCountries.Where(c => c.Value.Expiratedata == null || (c.Value.Expiratedata is not null && c.Value.Expiratedata > DateTime.Now));

            foreach (var country in newblocedCountries)
            {
                result.TryAdd(country.Key, country.Value);
            }
            _memoryCache.Set(BlocedCountryKeys, result);
            await Task.CompletedTask;

        }
    }
}