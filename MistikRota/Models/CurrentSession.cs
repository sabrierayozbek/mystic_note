using MistikRota.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MistikRota.Models
{
    public class CurrentSession
    {
        //property'miz statik olsun, new'lemeden ulaşalım.

      

        public static MistikRotaUser User
        {
            get //classlar içinden erişim HttpContext.Current.Session şeklinde oluyor
            {
                return Get<MistikRotaUser>("login");
            }
        }

        public static void Set<T>(string key, T obj) //Verdiğimiz bir session anahtar ismine, verdiğimiz tipte bir objeyi set edelim.
        {
            HttpContext.Current.Session[key]= obj; //Set<login, MistikRotaUser> gibi...
        }

        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key] != null) //Kontrol işlemimizi kısalttık.
            {
                return (T)HttpContext.Current.Session[key]; //O anki kullanıcıyı dön.
            }

            return default(T);
        }

        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        public static void Clear() //direkt içini clear methodu ile temizlicek
        {
            HttpContext.Current.Session.Clear();
        }


    }
}