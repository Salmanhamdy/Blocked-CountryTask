using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blocked_CountryCore.Models
{
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? Expiratedata { get; set; } = null;
    }
}
