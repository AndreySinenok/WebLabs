using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinenok.Domain.Entities
{
    public class Category : Entity
    {
        public string NormalizedName { get; set; }
        public ICollection<Gadget> Gadgets { get; set; }
    }
}
