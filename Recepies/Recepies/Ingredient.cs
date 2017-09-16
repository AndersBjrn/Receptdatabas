using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recepies
{
    class Ingredient
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Recepie> Recepies { get; set; }

        public Ingredient()
        {
            Recepies = new List<Recepie>();
        }

        public virtual void AddRecepie(Recepie recepie)
        {
            Recepies.Add(recepie);
            recepie.Ingredients.Add(this);
        }
    }
}
