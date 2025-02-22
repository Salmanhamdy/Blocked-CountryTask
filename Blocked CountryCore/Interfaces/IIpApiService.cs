using Blocked_CountryCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocked_CountryCore.Interfaces
{
    public interface IIpApiService
    {
        Task<bool> CheckBlocedCountryAsync(string ip, string useragent);
        Task<IpDetails> GetIpDetailsAsync(string ipAddress, string useragent);
    }
}
