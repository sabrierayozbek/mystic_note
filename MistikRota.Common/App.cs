using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace MistikRota.Common
{
    public static class App //dışarıdan erişilecek olan sınıf bu sınıf
    {
        //newlenmeden kullanılacak, üyeleri de static olmalı.
        public static ICommon common = new DefaultCommon(); //nu nesneden newleyip veridm, artık bu sınıfla çalışıyor. 
    }
}

//sıra halinde bu dll'e erişiyorlarsa, bu classa'da erişiyorlar. bu class'a erişiyorlarsa
//defaultcommandaki methoda erişiyorlar.