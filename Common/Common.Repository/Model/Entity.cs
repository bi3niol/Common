using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Entity<Tkey>
    {
        [Key]
        public Tkey Id { get; set; }
    }
}
