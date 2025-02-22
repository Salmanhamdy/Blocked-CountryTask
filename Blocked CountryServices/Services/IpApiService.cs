using Blocked_CountryCore.Interfaces;
using Blocked_CountryCore.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Blocked_CountryServices.Services
{
    public class IpApiService:IIpApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ICachingServices _cachingServices;
        private readonly Ilogservices _ilogservices;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public IpApiService(HttpClient httpClient, IConfiguration configuration, ICachingServices cachingServices, Ilogservices ilogservices)
        {
            _httpClient = httpClient;
            _cachingServices = cachingServices;
            _apiKey = configuration["IpApiSettings:ApiKey"];
            _baseUrl = configuration["IpApiSettings:BaseUrl"];
            _ilogservices = ilogservices;
        }

        public async Task<bool> CheckBlocedCountryAsync(string ip,string useragent)
        {
            var blocedcuntry = await _cachingServices.GetBlockedCountriesAsync();
            var Countrydetails = await GetIpDetailsAsync(ip,useragent);
            return Countrydetails != null;


        
        }

        public async Task<IpDetails> GetIpDetailsAsync(string ipAddress, string useragent)
        {
            string url = $"{_baseUrl}?apiKey={_apiKey}&ip={ipAddress}";
            var response = await _httpClient.GetAsync(url);
            var blocedcuntry = await _cachingServices.GetBlockedCountriesAsync();
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IpDetails>(content);
                var logattemp = new LogAttmpt()
                {
                    IpAddress = ipAddress,
                    CountryCode = result.country_code2,
                    BlockedStatus = true,
                    Timestamp = DateTime.Now,
                    UserAgent = useragent

                };
                if (blocedcuntry.Any(c => c.Code == result.country_code2)) 
                {
                     await _ilogservices.Logattempt(logattemp);
                     return null;
                }

                logattemp.BlockedStatus = false;
                await  _ilogservices.Logattempt(logattemp);
                return result;
            }

            return null;
        }

    }
}
