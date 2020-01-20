using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistikRota.Entites
{
   public class MyEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] //id otomatik artan olacak ve id kolonu primary key olacak.
        public int Id { get; set; }
        [ScaffoldColumn(false), Required]
        public DateTime CreatedOn { get; set; }
        [ScaffoldColumn(false), Required]
        public DateTime ModifiedDate { get; set; }
        [ScaffoldColumn(false), Required, StringLength(30)]
        public string ModifiedUsername { get; set; }
    }
}
