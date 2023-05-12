using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace PloomesBackend.Controllers.Extensions
{
    public static class ControllerExtensions
    {
        public static ContentResult CreatedJsonContentResult(this Controller controller, object returnData)
        {
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(returnData),
                ContentType = "application/json",
                StatusCode = StatusCodes.Status201Created
            };
        }
    }
}
