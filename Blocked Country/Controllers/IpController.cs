using Blocked_Country.Helpers;
using Blocked_CountryCore.Interfaces;
using Blocked_CountryCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Blocked_Country.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IpController : ControllerBase
    {
      private readonly IIpApiService _ipApiService;
      private readonly IHttpContextAccessor _contextAccessor;

        public IpController(IIpApiService ipApiService, IHttpContextAccessor contextAccessor)
        {
            _ipApiService = ipApiService;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("lookup")]
        public async Task<ActionResult<IpDetails>> lookup([FromQuery] string? Ip=null) 
        {
            if(Ip is not null&&!IsIp.IsValidIPv4(Ip)) 
            {
                return BadRequest("the Ip is not vaild");
            }
            
            if (Ip is null)
            {
               Ip= _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                // return local host
                Ip = "197.36.183.187";
            }
            var useragent = _contextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
            var countrydetails = await _ipApiService.GetIpDetailsAsync(Ip, useragent);
            return countrydetails == null ? BadRequest($"the country is bloced") : Ok(countrydetails);
          
        }
        [HttpGet("/check-block")]
        public async Task<ActionResult<bool>> Checkblock() 
        {
            var ip = _contextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            ip = "197.36.183.187";
            var useragent = _contextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
            bool Result = await _ipApiService.CheckBlocedCountryAsync(ip, useragent);
            return Result ==false ? BadRequest($"the country is bloced"):Ok("country is not bloced");

        }
    }
}
