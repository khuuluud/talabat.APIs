
namespace talabat.APIs.Errors
{
    public class ApiResponse
    {
     

        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMsgForStatusCode(statusCode) ;
        }

        private string? GetDefaultMsgForStatusCode(int? statusCode)
        {
            //500 => Internal server error
            //400 => Bad Request
            //401 => Unothorized
            //404 => Not found

            return StatusCode switch
            {
                400 => "Bad Request",
                401 => "You are not Authorized",
                404 => "Resource Not Found",
                500 => "Internal Server error",
               _ => null
            };

        }
    }
}
