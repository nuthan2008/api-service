using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using APIService.Models;

namespace APIService.Filters.ActionFilter;
public class ApiResponseWrapperFilterAttribute : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // This method is called before the action method is invoked.
        // Implement any pre-action behavior here if needed.
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // This method is called after the action method has executed.
        if (context.Result is ObjectResult objectResult)
        {
            // context.HttpContext.
            // Get the response object from the action result.
            var responseObject = objectResult.Value;

            // Wrap the response object into the desired format.
            var requestData = new RequestData { RequestId = Guid.NewGuid() }; // Generate a unique request ID
            var responseData = new ResponseData<object> // Assuming object is your generic type
            {
                Status = "success",
                StatusDetails = new StatusDetails { Id = Guid.NewGuid(), Type = null, Action = null, Total = 0 },
                DataObjs = responseObject
            };

            var wrappedResponse = new ApiResponse<object>(requestData, responseData);

            // Set the wrapped response as the action result.
            context.Result = new OkObjectResult(wrappedResponse);
        }
    }
}
