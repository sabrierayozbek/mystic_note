using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistikRota.Common
{
    public class DefaultCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            return "system"; //method implement edilip, varsayılan olarak dönecek username'im.
        }
    }
}
