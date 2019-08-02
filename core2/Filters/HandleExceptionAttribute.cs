using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core2.Filters
{
    public class HandleExceptionAttribute:ExceptionFilterAttribute
    {
        public string ViewName { get; set; } = "Error";
        public Type exceptionType { get; set; } = null;
        public override void OnException(ExceptionContext context)
        {
            if (exceptionType!=null)
            {
                if (context.Exception.GetType()== exceptionType)
                {
                    var result = new ViewResult { ViewName = ViewName };
                    var modelDataprovider = new EmptyModelMetadataProvider();
                    result.ViewData = new ViewDataDictionary
                    (
                        modelDataprovider, context.ModelState
                    );
                    result.ViewData.Add("HandleException", context.Exception);
                    context.Result = result;
                    context.ExceptionHandled = true;
                }
            }
          

        }

    }
}
