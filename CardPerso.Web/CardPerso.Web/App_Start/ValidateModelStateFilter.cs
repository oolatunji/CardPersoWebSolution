using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CardPerso.Web
{
    public class ValidateModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var requiredFields = new StringBuilder();

            if (!actionContext.ModelState.IsValid)
            {
                var modelType = actionContext.ActionArguments["model"].GetType();

                var length = actionContext.ModelState.Keys.Count;

                var counter = 1;

                actionContext.ModelState.Keys.ToList().ForEach((key) =>
                {
                    string modelDisplayName = string.Empty;

                    var modelProperty = key.Split('.')[1];

                    MemberInfo property = modelType.GetProperty(modelProperty);

                    var attribute = property.GetCustomAttributes(typeof(DisplayNameAttribute), true).Cast<DisplayNameAttribute>().FirstOrDefault();

                    modelDisplayName = attribute != null ? attribute.DisplayName : modelProperty;

                    if(counter < length)
                    {
                        requiredFields.Append($"{modelDisplayName}, ");
                    }
                    else
                    {
                        requiredFields.Append($"{modelDisplayName}");
                    }
                    
                    counter++;                  
                });
                             
                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, $"The following fields are required: {requiredFields.ToString()}");
            }
        }
    }
}