using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MistikRota.Entites
{
   [Table("Categories")]
   public class Category : MyEntityBase
    { 
        [DisplayName("Kategori"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(50, ErrorMessage = "{0} alanı max {1} karakter içerir.")]
        public string Title { get; set; }

        [DisplayName("Açıklama"), Required(ErrorMessage = "{0} alanı gereklidir."),StringLength(500, ErrorMessage = "{0} alanı max {1} karakter içerir.")]
        public string Description { get; set; }

        public virtual List<Story> Stories { get; set; }

        public Category()
        {
            Stories = new List<Story>();
        }
    }
}
