using Blocked_CountryCore.Interfaces;
using Blocked_CountryCore.Models;
using Blocked_CountryServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Apis.Errors;
using Talabat.Apis.Helpers;

namespace Blocked_Country.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
      private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpPost("block")]
        public async Task<IActionResult> BlockCountry([FromBody] string countryCode)
        {
          var Status=await _countryService.BlockCountryAsync(countryCode);

            return Status ? Ok($"Country {countryCode} blocked successfully.") : BadRequest(new ApiResponse(400, $"Country '{countryCode}' is already blocked.")); ;
        }

        [HttpDelete("block/{countryCode}")]
        public async Task<IActionResult> UnblockCountry(string countryCode)
        {
            var Status = await _countryService.UnblockCountryAsync(countryCode);
            return Status ? Ok($"Country {countryCode} unblocked successfully."): NotFound(new ApiResponse(404, $"Country '{countryCode}' is not blocked."));
        }

        [HttpGet("blocked")]
        public async Task<ActionResult<IReadOnlyList<Country>>> GetBlockedCountries(int page,int pageSize )
        {
            var countries = await _countryService.GetBlockedCountriesAsync();
            var count = countries.Count();
            countries = countries.Skip(pageSize * (page - 1)).Take(pageSize).ToList();
            if (countries is not null)
                return Ok(new Pagination<Country>(pageSize,page, count, countries));
            else
                return Ok(BadRequest(new ApiResponse(400, "There are no bloced countries")));
        }
        [HttpPost("temporal-block")]
        public async Task<ActionResult<bool>> temporalblock(string countryCode, int durationMinutes) 
        {
            if ( durationMinutes < 1 && durationMinutes > 1440)
                return BadRequest("durationMinutes is out of range");
            if (string.IsNullOrEmpty(countryCode))
                return BadRequest("invaild country code");
            var Status = await _countryService.BlockCountryAsync(countryCode,durationMinutes);
            return Status ? Ok($"Country {countryCode} blocked successfully.") : Conflict(new ApiResponse(409,$"Country '{countryCode}' is already blocked.")); ;
        }
       
    }
}
