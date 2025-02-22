
namespace Talabat.Apis.Errors
{
    public class ApiResponse
    {
  
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statuscode, string? message=null)
        {
            StatusCode = statuscode;
            Message = message ??GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int? Statuscode)
        {
            return Statuscode switch
            {
                400=>"Bad Requst",
                401=>"You are not Authorized",
                404=>" Resource Not Found",
                500=>"Internal Server Errors", 
                _=>null
            };
        }
    }
}
