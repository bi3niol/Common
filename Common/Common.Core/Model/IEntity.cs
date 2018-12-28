using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public interface IEntity<Tkey>
    {
        [Key]
        Tkey Id { get; set; }
    }
}
