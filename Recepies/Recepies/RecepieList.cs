using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recepies
{
    class RecepieList
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Recepie> Recepies { get; set; }

        public RecepieList()
        {
            Recepies = new List<Recepie>();
        }
    }
}
