using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recepies
{
    class Category
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Recepie> Recepies { get; set; }

        public Category()
        {
            Recepies = new List<Recepie>();
        }

        public virtual void AddRecepie(Recepie recepie)
        {
            Recepies.Add(recepie);
            recepie.Categories.Add(this);
        }
    }
}
