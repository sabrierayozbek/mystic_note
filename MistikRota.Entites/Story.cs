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
    [Table("Stories")]
    public class Story : MyEntityBase
    {
        [DisplayName("Öykü Başlığı"), Required, StringLength(70)]
        public string Title { get; set; }
        [DisplayName("Not Metni"), Required]
        public string Content { get; set; }
        [DisplayName("Taslak")]
        public bool IsDraft { get; set; }
        [DisplayName("Beğenilme")]
        public int LikeCount { get; set; }
        [DisplayName("Kategori")]
        public int CategoryId { get; set; } //ilişkili old. tablonun ismi, oradaki ilişkili old. propertyinin adını yazarak otomatik olarak bu ilişkinin sağlanmasına neden oldum. 
        //bizim için ait old. category id'yi bu property gönderecek story'i selectlediğim zaman.

        public virtual MistikRotaUser Owner { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual Category Category { get; set; } //Buradan id'yi öğrenmeye gitseydik category tablosuna bir sorgu atıp category nesnesini geri dönüp id'yi vercek. iş yükü olacak. 
        public virtual List<Liked> Likes { get; set; }

        public Story()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }



    }
}
