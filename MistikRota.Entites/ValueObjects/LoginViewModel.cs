using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MistikRota.Entites.ValueObjects
{
    //Diğer ana modelim ile karışmasın diye View'ler için oluşturduğum model.
    public class LoginViewModel
    {
        [DisplayName("Kullanici Adi"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(50,ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Username { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(50, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Password { get; set; }
    }
}