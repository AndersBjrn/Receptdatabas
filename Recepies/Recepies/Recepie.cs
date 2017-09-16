using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recepies
{
    class Recepie
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual ICollection<RecepieList> RecepieLists { get; set; }

        public Recepie()
        {
            Categories = new List<Category>();
            Ingredients = new List<Ingredient>();
            RecepieLists = new List<RecepieList>();
        }

        public virtual void AddIngredient(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
            ingredient.Recepies.Add(this);
        }

        public virtual void AddCategory(Category category)
        {
            Categories.Add(category);
            category.Recepies.Add(this);
        }

        public virtual void AddRecepieList(RecepieList recepieList)
        {
            RecepieLists.Add(recepieList);
            recepieList.Recepies.Add(this);
        }
    }
}
