using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MistikRota.Entites
{
    [Table("Comments")]
    public class Comment : MyEntityBase
    {
        [Required]
        public string Text { get; set; }

        public virtual Story Story { get; set; }
        public virtual MistikRotaUser Owner { get; set; }
    }
}
