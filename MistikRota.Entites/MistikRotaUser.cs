using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistikRota.Entites
{
    [Table("MistikRotaUsers")]
    public class MistikRotaUser : MyEntityBase
    {
        [DisplayName("İsim"), StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }
        [DisplayName("Soyad"), StringLength(50)]
        public string Surname { get; set; }
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Username { get; set; }
        [DisplayName("E-Posta"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }
        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [StringLength(255), ScaffoldColumn(false)] //images/user_12.jpg //Scaffold etme işlemi o alanın üretme aşamasında kullanılmamasıdır. Eklerken, güncelerken vs. o alanı kullanmayacağımızı belirleriz.
        public string ProfileImageFileName { get; set; }
        [DisplayName("Aktif")]
        public bool IsActive { get; set; }
        [DisplayName("Yönetici")]
        public bool IsAdmin { get; set; }

        [Required, ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }
  
        public virtual List<Story> Stories { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

    }
}
