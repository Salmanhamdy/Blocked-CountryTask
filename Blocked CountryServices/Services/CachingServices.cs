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
    public class CachingServices: ICachingServices
    {
        private readonly IMemoryCache _memoryCache;
        private const string BlocedCountryKeys = "BlocedCountryKeys";
        private const string LogAttemptKeys = nameof(LogAttemptKeys);
        private readonly ConcurrentDictionary<string, Country> _blockedCountries = new ();
        private readonly List<LogAttmpt> logAttmpts = new List<LogAttmpt>();
        public CachingServices(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _memoryCache.Set(BlocedCountryKeys, _blockedCountries);
            _memoryCache.Set(LogAttemptKeys, logAttmpts);
        }

        public async Task<IReadOnlyList<Country>> GetBlockedCountriesAsync()
        {
            var blockedCountries = _memoryCache.GetOrCreate(BlocedCountryKeys, entry => new ConcurrentDictionary<string, Country>());
            return await Task.FromResult(blockedCountries.Select(c=>c.Value).ToList());
        }

        public Task BlockCountryAsync(string countryCode, int? durationMinutes = null)
        {
            _blockedCountries.TryAdd(countryCode, new Country() { Code = countryCode ,Expiratedata=durationMinutes==null?null:DateTime.Now.AddMinutes(durationMinutes.Value)});
            _memoryCache.Set(BlocedCountryKeys, _blockedCountries);
            return Task.CompletedTask;

        }

        public async Task UnblockCountryAsync(string countryCode)
        {
            _blockedCountries.TryRemove(countryCode, out Country country);
            _memoryCache.Set(BlocedCountryKeys, _blockedCountries);
            await Task.CompletedTask;
        }

        public async Task Logattempt(LogAttmpt logAttmpt)
        {
            logAttmpts.Add(logAttmpt);
            _memoryCache.Set(LogAttemptKeys, logAttmpts);
            await Task.CompletedTask;
        }

        public async Task<List<LogAttmpt>> Getlogs()
        {
            await Task.CompletedTask;
            return  logAttmpts.ToList();
            
        }

        public async Task RemoveTemporalBlockedCountries()
        {
            var newblocedCountries = _blockedCountries.Where(c =>c.Value.Expiratedata!=null && c.Value.Expiratedata > DateTime.Now);
            _memoryCache.Set(BlocedCountryKeys, newblocedCountries);
            await Task.CompletedTask;
             
        }
    }
}
