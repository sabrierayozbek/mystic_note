using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistikRota.Entites //bir öyükünün birden fazla like'ı ve like'layanı old. için.
    //Çoka çok ilişki vardır ve biz bunun bir ara tablo ile üzerinden hareket ettim.
{ 
    [Table("Likes")]
   public class Liked
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Story Story { get; set; } //notun kendisi
        public MistikRotaUser LikedUser { get; set; } //bu notu likelayan user
    }
}
