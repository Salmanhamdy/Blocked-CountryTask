using Blocked_CountryCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocked_CountryCore.Interfaces
{
    public interface ICountryService
    {
        Task<bool> BlockCountryAsync(string countryCode, int ? durationMinutes=null);
        Task <bool>UnblockCountryAsync(string countryCode);
        Task<IReadOnlyList<Country>> GetBlockedCountriesAsync();
    }
}
