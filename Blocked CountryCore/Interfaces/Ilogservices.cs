using Blocked_CountryCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocked_CountryCore.Interfaces
{
    public interface Ilogservices
    {
        Task Logattempt(LogAttmpt logAttmpt);

        Task<List<LogAttmpt>> GetLogs();
    }
}
