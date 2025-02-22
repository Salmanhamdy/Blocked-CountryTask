using Blocked_CountryCore.Interfaces;
using Blocked_CountryCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blocked_Country.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly Ilogservices _ilogservices;

        public LogsController(Ilogservices ilogservices)
        {
            _ilogservices = ilogservices;
        }
        [HttpGet("GetLogs")]
        public async Task<ActionResult<List<LogAttmpt>>> getlogs(bool getall) 
        {
            var Result = (await _ilogservices.GetLogs()).Where(l=>getall||l.BlockedStatus==true);
            
            return  Ok(Result);
        }
    }
}
