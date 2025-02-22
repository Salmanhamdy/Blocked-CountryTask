using Blocked_CountryCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocked_CountryCore.Interfaces
{
    public interface ICachingServices
    {
        Task BlockCountryAsync(string countryCode, int? durationMinutes=null);
        public Task<IReadOnlyList<Country>> GetBlockedCountriesAsync();
        Task<List<LogAttmpt>> Getlogs();
        Task Logattempt(LogAttmpt logAttmpt);
        Task RemoveTemporalBlockedCountries();
        Task UnblockCountryAsync(string countryCode);
    }
}
