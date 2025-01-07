using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace talabat.APIs.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _expireTimeInSecond;

        public CachedAttribute(int ExpireTimeInSecond )
        {
            _expireTimeInSecond = ExpireTimeInSecond;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var ChacheService =   context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var ChacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            var CachedResponse =   await   ChacheService.GetChachedResponse(ChacheKey);
            if (!string.IsNullOrEmpty(CachedResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = CachedResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }

          var Excuted = await  next.Invoke(); // will excute endpoint
            if ( Excuted.Result is OkObjectResult result)
            {
                ChacheService.CasheResponseasync(ChacheKey, result.Value, TimeSpan.FromSeconds(_expireTimeInSecond));
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();
          

            KeyBuilder.Append(request.Path); //api/controller
            foreach (var (key , value) in request.Query.OrderBy(X => X.Key))
            {
                // Sort = Name 
                // Page Index = 1
                // Page Size = 5

                KeyBuilder.Append($"|{key}-{value}");
                // api/Products|Sort-Name|PageIndex-1|PageSize-5

            }
            return KeyBuilder.ToString();
        }
    }
}
