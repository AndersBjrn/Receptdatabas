using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;

namespace Recepies
{
    public class NHibernateMapper
    {
        private readonly ModelMapper _modelMapper;

        public NHibernateMapper()
        {
            _modelMapper = new ModelMapper();
        }

        public HbmMapping Map()
        {
            MapRecepie();
            MapIngredient();
            MapCategory();
            MapRecepieList();
            return _modelMapper.CompileMappingForAllExplicitlyAddedEntities();
        }

        private void MapRecepie()
        {
            _modelMapper.Class<Recepie>(e =>
            {
                e.Id(p => p.Id, p => p.Generator(Generators.GuidComb));
                e.Property(p => p.Name);

                e.Set(y => y.Ingredients, collectionMapping =>
                {
                    collectionMapping.Table("RecepieIngredient");
                    collectionMapping.Inverse(true);
                    collectionMapping.Cascade(Cascade.None);
                    collectionMapping.Key(keyMap => keyMap.Column(col => col.Name("RecepieId")));
                }, map => map.ManyToMany(y => y.Column("IngredientId")));

                e.Set(x => x.Categories, collectionMapping =>
                {
                    collectionMapping.Inverse(true);
                    collectionMapping.Table("RecepieCategory");
                    collectionMapping.Cascade(Cascade.None);
                    collectionMapping.Key(keyMap => keyMap.Column("RecepieId"));
                }, map => map.ManyToMany(p => p.Column("CategoryId")));

                e.Set(x => x.RecepieLists, collectionMapping =>
                {
                    collectionMapping.Inverse(true);
                    collectionMapping.Table("RecepieListRecepie");
                    collectionMapping.Cascade(Cascade.None);
                    collectionMapping.Key(keyMap => keyMap.Column("RecepieId"));
                }, map => map.ManyToMany(p => p.Column("RecepieListId")));

            });
        }

        private void MapIngredient()
        {
            _modelMapper.Class<Ingredient>(e =>
            {
                e.Id(c => c.Id, c => c.Generator(Generators.GuidComb));
                e.Property(c => c.Name);

                e.Set(x => x.Recepies, collectionmapping =>
                {
                    collectionmapping.Table("RecepieIngredient");
                    collectionmapping.Cascade(Cascade.None);
                    collectionmapping.Key(keymap => keymap.Column("IngredientId"));
                }, map => map.ManyToMany(p => p.Column("RecepieId")));

            });
        }

        private void MapCategory()
        {
            _modelMapper.Class<Category>(e =>
            {
                e.Id(c => c.Id, c => c.Generator(Generators.GuidComb));
                e.Property(c => c.Name);

                e.Set(x => x.Recepies, collectionmapping =>
                {
                    collectionmapping.Table("RecepieCategory");
                    collectionmapping.Cascade(Cascade.None);
                    collectionmapping.Key(keymap => keymap.Column("CategoryId"));
                }, map => map.ManyToMany(p => p.Column("RecepieId")));

            });
        }

        private void MapRecepieList()
        {
            _modelMapper.Class<RecepieList>(e =>
            {
                e.Id(c => c.Id, c => c.Generator(Generators.GuidComb));
                e.Property(c => c.Name);

                e.Set(x => x.Recepies, collectionmapping =>
                {
                    collectionmapping.Table("RecepieListRecepie");
                    collectionmapping.Cascade(Cascade.None);
                    collectionmapping.Key(keymap => keymap.Column("RecepieListId"));
                }, map => map.ManyToMany(p => p.Column("RecepieId")));

            });
        }
    }
}