using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Api.Exception;

public class CustomExceptionFilter:IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var response = new
        {
            Message = "An error occurred while processing your request.",
            Detail = context.Exception.Message
        };

        context.Result = new JsonResult(response)
        {
            StatusCode = 500
        };

        context.ExceptionHandled = true;
    }
}
