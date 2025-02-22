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
    public class CountryService : ICountryService
    {
        private readonly ICachingServices _cachingServices;

        public CountryService(ICachingServices cachingServices)
        {
            _cachingServices = cachingServices;
        }

        public async Task<bool> BlockCountryAsync(string countryCode ,int? durationMinutes = null)
        {
            countryCode = countryCode.Trim().ToUpper();
            var BlocedCountry = await _cachingServices.GetBlockedCountriesAsync();
            if (BlocedCountry.Any(p=>p.Code==countryCode))
            {
                return await Task.FromResult((false));
            }
            await _cachingServices.BlockCountryAsync(countryCode,durationMinutes);
            return await Task.FromResult((true));
        }

        public async Task<IReadOnlyList<Country>> GetBlockedCountriesAsync()
        {
            return await _cachingServices.GetBlockedCountriesAsync(); ;
        }

        public async Task<bool> UnblockCountryAsync(string countryCode)
        {
            countryCode = countryCode.Trim().ToUpper();
            var BlocedCountry = await _cachingServices.GetBlockedCountriesAsync();
            if (!BlocedCountry.Any(p => p.Code == countryCode))
            {
                return await Task.FromResult((false));
            }

            _cachingServices.UnblockCountryAsync(countryCode);
            return await Task.FromResult((true)); 
        }
    }
}
