using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcVideoclubNC.Filters
{
    public class AuthorizeClientesAttribute : AuthorizeAttribute, 
        IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var usuario = context.HttpContext.User;
            if(usuario.Identity.IsAuthenticated == false)
            {
                //Si no está validado, lo enviamos a LogIn
                //Controller, Action
                RouteValueDictionary rutalogin =
                    new RouteValueDictionary(
                        new
                        {
                            controller = "Manage"
                            ,
                            action = "LogIn"
                        });
                RedirectToRouteResult action =
                    new RedirectToRouteResult(rutalogin);
                context.Result = action;
            }
        }
    }
}
