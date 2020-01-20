using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace MistikRota.Common.Helper
{
   public class ConfigHelper //bir configuration dosyasın okumalarını yapıp bize tip dönüşümü için yaratıldı
    {
        public static T Get<T>(string key) //verdiğmiz anahtar değeri okuyup değeri bize döndürcek.
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));
        }
    }
}
