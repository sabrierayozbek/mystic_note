using System.Collections.Generic;
using MistikRota.Entites.Messages;

namespace MistikRota.BusinessLayer.Results
{
    public class BusinessLayerResult<T> where T : class 
    {
        public List<ErrorMessageObj> Errors { get; set; } //Hata varsa hata mesajlarımı içerecek olan property. 
        public T Result { get; set; } //geriye dönecek olan property. (Mistik rota user)

        public BusinessLayerResult() //Liste boş gelirse patlamamak için
        {
            Errors = new List<ErrorMessageObj>();   
        }

        public void AddError(ErrorMessageCode code, string message) //sadece hata ekleme kodunu kısaltmak amacıyla yazılmış bir fonk.
        {
           Errors.Add(new ErrorMessageObj() {Code  = code, Message = message}); //yeni bir item eklerken new'lediğimiz şey bunlar olacak.
        }
    }
}