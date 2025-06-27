using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinenok.Domain.Entities
{
    public class Entity
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
    }
}
