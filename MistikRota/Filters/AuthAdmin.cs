using MistikRota.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MistikRota.Filters
{
    public class AuthAdmin : FilterAttribute, IAuthorizationFilter //filter attribute attribute özelliği kazandırmasını sağlıyor, bu da iauthoratizon filterdan implement etmemeizi
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.User != null && CurrentSession.User.IsAdmin == false)
            { //bu nesne null değilse admin olup olmadığını okuyabilirim.
                filterContext.Result = new RedirectResult("/Home/AccessDenied");
            }
        }
    }
}