using MistikRota.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MistikRota.Filters
{
    public class Auth : FilterAttribute, IAuthorizationFilter //login işlemi için
    {
        public void OnAuthorization(AuthorizationContext filterContext) //Auth class'ımızın attribute olarak tanımlanması ve AuthrozionFilter interface'inden implement edilmesi.
        {
            if (CurrentSession.User == null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
                //Artık Auth attribute'unu hangi metodun başına eklersem ya da controller ın başına eklersem çalışacak. 
            }
        }
    }
}