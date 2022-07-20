using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace core2.Filters
{
    public class CustomFilter : Attribute, IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            int i = 10;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            int i = 20;
        }

    }
}
