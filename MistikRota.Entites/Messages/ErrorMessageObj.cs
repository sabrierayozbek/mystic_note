using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistikRota.Entites.Messages
{
    public class ErrorMessageObj //ErrorMassageCode'larımızo kullanabilmek için kendi classımızı oluşturduk.
    {
        public ErrorMessageCode Code { get; set; } 
         public string Message { get; set; }
    }
}
