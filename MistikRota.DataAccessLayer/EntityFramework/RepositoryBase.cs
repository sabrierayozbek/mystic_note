using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MistikRota.DataAccessLayer;

namespace MistikRota.DataAccessLayer.EntityFramework
{
    //singleton pattern
   public class RepositoryBase
   {
       protected static DatabaseContext context; //Repository classından miras ald. içim, bu protected nesneyi orada kullanabilirim.
       
       protected RepositoryBase() //class'ın newlenmemesi için proteceted bir ctor oluşturuldu. miras alan bunu sadece new'leyebilir.
       {
           CreateContext();
       }

        //ctor'un içinde methodu çağırarak db'nin burada oluşmasını sağlıyprum. repo'nun içinde oluşmasına lüzum kalmadı.


       private static void CreateContext()//static methodlar newlenmeye gerek olmadan çalışır.
            //bu method ilk kez çağrıldığında bir context dönecek ve bir daha çağrılırsa, başka bir context göndermeyip aynı context'i gönderecek.
       {
           if (context == null)
           {      
                
          context = new DatabaseContext();
  
           }
       }
   }
}
