using Blocked_CountryCore.Interfaces;
using Blocked_CountryCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocked_CountryServices.Services
{
   public class LogServices : Ilogservices
    {
        private readonly ICachingServices _cachingServices;
     
        public LogServices(ICachingServices cachingServices)
        {
            _cachingServices = cachingServices;
        }

        public Task<List<LogAttmpt>> GetLogs()
        {
            return _cachingServices.Getlogs();
        }

        public async Task Logattempt(LogAttmpt logAttmpt)
        {
           await _cachingServices.Logattempt(logAttmpt);
        }
    }
}
