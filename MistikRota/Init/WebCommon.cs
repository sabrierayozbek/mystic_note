using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MistikRota.Common;
using MistikRota.Entites;

namespace MistikRota.Init
{
    public class WebCommon : ICommon //methodu implement etmek için
    {
        public string GetCurrentUsername()
        {
            if (HttpContext.Current.Session["login"] != null) //sesşona erişimin klasik yolu
            { 
                //Kullancıyı login'de tutuyorduk. Session'daki loginde. HttpContext ile session'a erişim sağladık.            {
                MistikRotaUser user = HttpContext.Current.Session["login"] as MistikRotaUser; //Mistik Rota user nesnesi tutuğumuz için, gelen string bilgiyi Mistik Rota User classına çeviriyoz. 
                return user.Username;
            }

            return "system";
        }
    }
}