
using DatingAPI.Exceptions;
using DatingAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace DatingAPI.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case DatingException e:
                    HandleCustomException(context);
                    return Task.CompletedTask;

                default:
                    return Task.FromResult(context.Result = new ObjectResult(new ExceptionResponse { Message = "Neočekivana greška", Title = "SystemException" }) { StatusCode = (int)HttpStatusCode.BadRequest });
            }
        }
        private void HandleCustomException(ExceptionContext context)
        {
            if (context.Exception is NullExection)
            {
                var nullMessage = new
                {
                    ExeptionType = "NullException",
                    Message = "Nisu popunjeni podaci"
                };

                context.Result = new ObjectResult(nullMessage)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };

                context.ExceptionHandled = true;
                return;
            }
        }
    }
}
