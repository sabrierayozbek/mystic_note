using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MistikRota.Entites.ValueObjects
{
    public class RegisterViewModel
    {
        [DisplayName("Kullanici Adi"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(50, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Username { get; set; }
        [DisplayName("E-Posta"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(70, ErrorMessage = "{0} alanı için geçerli olan karakter sınırını aştınız.")]
        [RegularExpression(@"^[\w-\._\+%]+@(?:[\w-]+\.)+[\w]{2,6}$", ErrorMessage = "Lütfen geçerli bir e-posta adresi giriniz.")]
        public string EMail { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(50, ErrorMessage = "{0} max. {1} karakter olmalı.")]
        public string Password { get; set; }
        [DisplayName("Şifre Tekrar"), Required(ErrorMessage = "{0} alanı boş geçilemez."), StringLength(50, ErrorMessage = "{0} max. {1} karakter olmalı."), Compare("Password", ErrorMessage = "{0} ile {1} uyuşmuyor.")]
        
        public string RePassword { get; set; }
    }
}