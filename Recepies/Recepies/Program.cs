using NHibernate.Linq;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NHibernate;

namespace Recepies
{
    class Program
    {
        static void Main(string[] args)
        {
            var session = DbService.OpenSession();
            OptionsScreen(session);
        }

        private static void OptionsScreen(NHibernate.ISession session)
        {
            Console.WriteLine("Recepie Database.");
            Console.WriteLine();
            Console.WriteLine("Make a choise: ");
            Console.WriteLine("[1] Create new recepie");
            Console.WriteLine("[2] Remove recepie");
            Console.WriteLine("[3] Update recepie");
            Console.WriteLine("[4] Display all recepies");
            Console.WriteLine("[5] Search recepies by category");
            Console.WriteLine("[6] Search recepies by ingredient");
            //Console.WriteLine("[S]earch recepies by recepie name");
            Console.WriteLine("[7] Generate new recepielist");
            Console.WriteLine("[8] Display all recepielists");
            Console.WriteLine("[F]ill with testdata");
            Console.WriteLine("[D]elete entire database ");
            Console.WriteLine("[C]reate database");
            Console.WriteLine("[Q]uit");
            Console.Write("");
            string choise = Console.ReadLine();

            switch (choise.ToUpper())
            {
                case "1":
                    CreateNewRecepie(session);
                    OptionsScreen(session);
                    break;
                case "2":
                    RemoveRecepie();
                    OptionsScreen(session);
                    break;
                case "3":
                    UpdateRecepie();
                    OptionsScreen(session);
                    break;
                case "4":
                    session.Flush();
                    DisplayRecepies(session);
                    break;
                case "5":
                    DisplayRecepiesByCategory(session);
                    break;
                case "6":
                    DisplayRecepiesByIngredient(session);
                    break;
                //case "S":
                //    SearchRecepies();
                //    DisplayRecepieProperties(session);
                //    break;
                case "7":
                    GenerateRecepielist(session);
                    OptionsScreen(session);
                    break;
                case "8":
                    DispalyRecepieLists(session);
                    OptionsScreen(session);
                    break;
                case "F":
                    GenerateTestData(session);
                    OptionsScreen(session);
                    break;
                case "D":
                    DeleteDatabase();
                    session.Flush();
                    OptionsScreen(session);
                    break;
                case "C":
                    CreateDatabase();
                    session.Flush();
                    OptionsScreen(session);
                    break;
                case "Q":
                    DbService.CloseSession(session);
                    break;
                default:
                    OptionsScreen(session);
                    break;
            }
        }

        private static void CreateDatabase()
        {
            var cfg = DbService.Configure();
            var export = new SchemaExport(cfg);
            export.Create(true, true);
        }

        private static void DeleteDatabase()
        {
            var cfg = DbService.Configure();
            var export = new SchemaExport(cfg);
            export.Drop(false, true);
        }

        private static void DispalyRecepieLists(NHibernate.ISession session)
        {
            PrintRecepieListsNames(session);
            Console.WriteLine("Enter recepielist ID if you want to see recepies, otherwise press enter ");
            string input = Console.ReadLine();
            int recepieListID = 0;
            if (input == "")
            {
                OptionsScreen(session);
            }
            else
            {
                recepieListID = CheckThatInputIsInt(input);
            }
            DisplayRecepiesInRecepieList(recepieListID, session);
        }

        private static void PrintRecepieListsNames(ISession session)
        {
            var recepieLists = session.Query<RecepieList>().ToList();
            for (int i = 0; i < recepieLists.Count; i++)
            {
                Console.WriteLine((i + 1) + " " + recepieLists[i].Name);
            }
        }

        private static void DisplayRecepiesByIngredient(NHibernate.ISession session)
        {
            Console.WriteLine("Enter searh ingredient: ");
            string searchString = Console.ReadLine();

            List<Recepie> recepieList = new List<Recepie>();
            var ingredient = session.Query<Ingredient>().Where(c => c.Name.Contains(searchString));
            foreach (Ingredient ing in ingredient)
            {
                foreach (Recepie rec in ing.Recepies)
                {
                    recepieList.Add(rec);
                }
            }
            DisplayRecepies(session, recepieList);
        }

        private static void DisplayRecepiesByCategory(NHibernate.ISession session)
        {
            Console.WriteLine("Enter searh category: ");
            string searchString = Console.ReadLine();

            List<Recepie> recepieList = new List<Recepie>();
            var category = session.Query<Category>().Where(c => c.Name.Contains(searchString));
            foreach (Category cat in category)
            {
                foreach (Recepie rec in cat.Recepies)
                {
                    recepieList.Add(rec);
                }
            }
            DisplayRecepies(session, recepieList);
        }

        private static void GenerateTestData(NHibernate.ISession session)
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            List<Category> categories = new List<Category>();
            string name = null;

            name = "Ärtsoppa";
            ingredients.Add(new Ingredient { Name = "Ärtor" });
            ingredients.Add(new Ingredient { Name = "Salt" });
            ingredients.Add(new Ingredient { Name = "Vatten" });
            categories.Add(new Category { Name = "Vegetariskt" });
            //categories.Add(new Category { Name = "Soppa" });

            InsertRecepieIntoDatabase(name, ingredients, categories, session);

            categories.Clear();
            ingredients.Clear();
            name = "Fisk i Ugn";
            ingredients.Add(new Ingredient { Name = "Fisk" });
            ingredients.Add(new Ingredient { Name = "Salt" });
            ingredients.Add(new Ingredient { Name = "Potatis" });
            ingredients.Add(new Ingredient { Name = "Ärtor" });
            ingredients.Add(new Ingredient { Name = "Dill" });
            categories.Add(new Category { Name = "Fisk" });
            categories.Add(new Category { Name = "Sunt" });

            InsertRecepieIntoDatabase(name, ingredients, categories, session);

            categories.Clear();
            ingredients.Clear();

            name = "Grönsakssoppa";
            ingredients.Add(new Ingredient { Name = "Morot" });
            ingredients.Add(new Ingredient { Name = "Broccoli" });
            ingredients.Add(new Ingredient { Name = "Bönor" });
            ingredients.Add(new Ingredient { Name = "Ärtor" });
            ingredients.Add(new Ingredient { Name = "Blomkål" });
            categories.Add(new Category { Name = "Sommarmat" });
            //categories.Add(new Category { Name = "Sunt" });
            categories.Add(new Category { Name = "Soppa" });

            InsertRecepieIntoDatabase(name, ingredients, categories, session);

            categories.Clear();
            ingredients.Clear();
            name = "Falukorv och makaroner";
            ingredients.Add(new Ingredient { Name = "Falukorv" });
            ingredients.Add(new Ingredient { Name = "Makaroner" });
            categories.Add(new Category { Name = "Kött" });

            //InsertRecepieIntoDatabase(name, ingredients, categories, session);

            //categories.Clear();
            //ingredients.Clear();
            //name = "Korvgryta";
            //ingredients.Add(new Ingredient { Name = "Korv" });
            //ingredients.Add(new Ingredient { Name = "Potatis" });
            //ingredients.Add(new Ingredient { Name = "Senap" });
            //categories.Add(new Category { Name = "Sommarmat" });
            //categories.Add(new Category { Name = "Kött" });

            //InsertRecepieIntoDatabase(name, ingredients, categories, session);

            //categories.Clear();
            //ingredients.Clear();
            //name = "Kyckling med rotfrukter";
            //ingredients.Add(new Ingredient { Name = "Kyckling" });
            //ingredients.Add(new Ingredient { Name = "Potatis" });
            //ingredients.Add(new Ingredient { Name = "Rödbeta" });
            //categories.Add(new Category { Name = "Kyckling" });
            //categories.Add(new Category { Name = "Kött" });
            //categories.Add(new Category { Name = "Höstmat" });

            //InsertRecepieIntoDatabase(name, ingredients, categories, session);

            //categories.Clear();
            //ingredients.Clear();
            //name = "Canneloni med tomatsås";
            //ingredients.Add(new Ingredient { Name = "Canneloni" });
            //ingredients.Add(new Ingredient { Name = "Tomatsås" });
            //categories.Add(new Category { Name = "Vegatariskt" });

            //InsertRecepieIntoDatabase(name, ingredients, categories, session);

            //categories.Clear();
            //ingredients.Clear();
            //name = "Broccolipaj";
            //ingredients.Add(new Ingredient { Name = "Broccoli" });
            //ingredients.Add(new Ingredient { Name = "Paprika" });
            //categories.Add(new Category { Name = "Vegatariskt" });
            //categories.Add(new Category { Name = "Paj" });

            //InsertRecepieIntoDatabase(name, ingredients, categories, session);

            //categories.Clear();
            //ingredients.Clear();
            //name = "Köttfärspaj";
            //ingredients.Add(new Ingredient { Name = "Köttfärs" });
            //ingredients.Add(new Ingredient { Name = "Lök" });
            //categories.Add(new Category { Name = "Kött" });
            //categories.Add(new Category { Name = "Paj" });

            //InsertRecepieIntoDatabase(name, ingredients, categories, session);

            //categories.Clear();
            //ingredients.Clear();
            //name = "Kassler i ugn";
            //ingredients.Add(new Ingredient { Name = "Kassler" });
            //ingredients.Add(new Ingredient { Name = "Annanas" });
            //categories.Add(new Category { Name = "Kött" });

            //InsertRecepieIntoDatabase(name, ingredients, categories, session);

            //categories.Clear();
            //ingredients.Clear();
            //name = "Kasslerpaj";
            //ingredients.Add(new Ingredient { Name = "Kassler" });
            //ingredients.Add(new Ingredient { Name = "Ost" });
            //categories.Add(new Category { Name = "Kött" });
            //categories.Add(new Category { Name = "Paj" });

            //InsertRecepieIntoDatabase(name, ingredients, categories, session);

            //categories.Clear();
            //ingredients.Clear();
            //name = "Kantarellsoppa";
            //ingredients.Add(new Ingredient { Name = "Kantarell" });
            //ingredients.Add(new Ingredient { Name = "Ost" });
            //categories.Add(new Category { Name = "Svamp" });
            //categories.Add(new Category { Name = "Soppa" });
            //categories.Add(new Category { Name = "Höstmat" });
            //categories.Add(new Category { Name = "Vegetariskt" });

            //InsertRecepieIntoDatabase(name, ingredients, categories, session);

            //categories.Clear();
            //ingredients.Clear();
            //name = "Champinjonburgare";
            //ingredients.Add(new Ingredient { Name = "Champinjon" });
            //ingredients.Add(new Ingredient { Name = "Ost" });
            //categories.Add(new Category { Name = "Svamp" });
            //categories.Add(new Category { Name = "Vegetariskt" });

            InsertRecepieIntoDatabase(name, ingredients, categories, session);

        }

        private static void DisplayRecepieProperties(NHibernate.ISession session, List<Recepie> recepies)
        {
            Console.WriteLine("Enter recepie ID if you want to see ingredients and categories, otherwise press enter ");
            int inputInt = CheckThatInputIsInt(Console.ReadLine());
            while (inputInt < 1 || inputInt > recepies.Count)
            {
                Console.Write("Invalid entry, enter number again");
                inputInt = CheckThatInputIsInt(Console.ReadLine());
            }
            var recepie = recepies[inputInt - 1];

            Console.Write("Ingredients: ");
            foreach (var ingr in recepie.Ingredients)
            {
                Console.Write(ingr.Name + " ");
            }
            Console.WriteLine();
            Console.Write("Categories: ");
            foreach (var cat in recepie.Categories)
            {
                Console.Write(cat.Name + " ");
            }
            Console.WriteLine();
            Console.Write("Press any key to continue!");
            Console.ReadKey();
            Console.Clear();
            OptionsScreen(session);
        }

        private static List<Ingredient> GetIngredientsByRecepie(Recepie recepie, NHibernate.ISession session)
        {
            var ingredients = session.Query<Ingredient>().ToList();
            return ingredients;
        }

        private static List<Category> GetCategoryByRecepie(Recepie recepie, NHibernate.ISession session)
        {
            var categories = session.Query<Category>().ToList();
            return categories;
        }

        public static int CheckThatInputIsInt(string input)//Makes sure the imput string is convertable to int and returns it
        {
            int inputInt = 0;
            while (!int.TryParse(input, out inputInt))
            {
                Console.Write("Please enter a valid ID: ");
                input = Console.ReadLine();
            }
            return inputInt;
        }

        private static void RemoveRecepie()
        {
            throw new NotImplementedException();
        }

        private static void UpdateRecepie()
        {
            throw new NotImplementedException();
        }

        private static void GenerateRecepielist(ISession session)
        {
            Console.WriteLine("Enter name for your new recepielist");
            string listName = ValidateName(Console.ReadLine());
            Console.Write("Do you want a random or manually created list (r/m)? ");
            string input = Console.ReadLine();
            while (input.ToUpper() != "R" && input.ToUpper() != "M")
            {
                Console.Write("Enter 'R' for random list or 'M' for manual list");
                input = Console.ReadLine();
            }
            switch (input.ToUpper())
            {
                case "R":
                    GenerateRandomRecepieList(session, listName);
                    break;
                case "M":
                    GenerateManualRecepieList(listName);
                    break;
            }
        }

        private static void GenerateManualRecepieList(string listName)
        {
            throw new NotImplementedException();
        }

        private static void GenerateRandomRecepieList(ISession session, string listName)
        {
            List<Category> categories = AskAndAddCategoryToRecepieList(session);
            List<Recepie> validRecepies = GetValidRecepies(categories);
            int numberOfRecepies = 0;
            Console.Write("How many recepies do you want in your list?: ");
            numberOfRecepies = CheckThatInputIsInt(Console.ReadLine());
            while (numberOfRecepies < 2 || numberOfRecepies > 14)
            {
                Console.Write("You can only have 2-14 recepies in a list");
                numberOfRecepies = CheckThatInputIsInt(Console.ReadLine());
            }
            if (numberOfRecepies > validRecepies.Count)
            {
                Console.WriteLine($"Maximum number of valid recepies is {validRecepies.Count}");
                numberOfRecepies = validRecepies.Count;
            }
            List<Recepie> randomizedRecepies = GenerateRandomListOfValidRecepies(numberOfRecepies, validRecepies);
            InsertRecepieListIntoDatabase(session, listName, randomizedRecepies);
        }

        private static List<Recepie> GenerateRandomListOfValidRecepies(int numberOfRecepies, List<Recepie> validRecepies)
        {
            List<Recepie> randomList = Shuffle(validRecepies);
            List<Recepie> recepieList = new List<Recepie>();
            for (int i = 0; i < numberOfRecepies; i++)
            {
                recepieList.Add(randomList[i]);
            }
            return recepieList;
        }

        static List<Recepie> Shuffle(List<Recepie> array)
        {
            Random random = new Random();
            int n = array.Count;
            for (int i = 0; i < array.Count; i++)
            {
                int r = i + random.Next(n - i);
                Recepie t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
            return array;
        }

        private static List<Recepie> GetValidRecepies(List<Category> categories)
        {
            List<Recepie> validRecepies = new List<Recepie>();
            foreach (Category cat in categories)
            {
                foreach (Recepie rec in cat.Recepies)
                {
                    validRecepies.Add(rec);
                }
            }
            return validRecepies.Distinct().ToList();
        }

        private static void InsertRecepieListIntoDatabase(ISession session, string listName, List<Recepie> recepies)
        {
            var recepieList = new RecepieList();
            recepieList.Name = listName;
            recepieList.Recepies = recepies;
            session.Save(recepieList);
        }

        private static List<Category> AskAndAddCategoryToRecepieList(NHibernate.ISession session)
        {
            List<Category> categories = new List<Category>();
            List<Category> avaliableCategories = new List<Category>();
            int categoryID = 0;
            Console.WriteLine("Do you want to add specific categories? (y/n)");
            string input = YesOrNo();
            while (input != "N")
            {
                avaliableCategories = ReturnAvaliableCategories(session, categories);
                DisplayCategories(session, avaliableCategories);
                Console.WriteLine("Enter category id to add ");
                categoryID = CheckThatInputIsInt(Console.ReadLine());
                AddCategoryToList(session, categories, avaliableCategories, categoryID);
                Console.WriteLine("Do you want to add another category? (y/n)");
                input = YesOrNo();
            }
            return categories;
        }

        private static void AddCategoryToList(ISession session, List<Category> categories, List<Category> avaliableCategories, int intput)
        {
            categories.Add(avaliableCategories[intput-1]);
        }

        private static List<Category> ReturnAvaliableCategories(NHibernate.ISession session, List<Category> categories)
        {
            var cats = session.Query<Category>().ToList();
            foreach (Category cat in categories)
            {
                cats.Remove(cat);
            }
            return cats;
        }

        private static void DisplayCategories(NHibernate.ISession session, List<Category> categories)
        {
            categories = categories.Distinct().ToList();
            for (int i = 0; i < categories.Count; i++)
            {
                Console.WriteLine((i + 1) + " " + categories[i].Name);
            }
        }

        private static string GetCategoryIDByID(int categoryID)
        {
            string category = null;
            var connectionString = "Server = (localdb)\\mssqllocaldb; Database = Recepies";

            var sql = $"select category.id from category where category.id = '{categoryID}';";

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        category = reader[i].ToString();
                    }
                }
            }
            return category;
        }

        private static void DisplayRecepies(NHibernate.ISession session)
        {
            var allRecepies = session.Query<Recepie>().ToList();
            for (int i = 0; i < allRecepies.Count; i++)
            {
                Console.WriteLine((i + 1) + " " + allRecepies[i].Name + " ");
            }
            DisplayRecepieProperties(session, allRecepies);
        }

        private static void DisplayRecepies(NHibernate.ISession session, List<Recepie> recepieList)
        {
            var allRecepies = recepieList;
            for (int i = 0; i < allRecepies.Count; i++)
            {
                Console.WriteLine((i + 1) + " " + allRecepies[i].Name + " ");
            }
            DisplayRecepieProperties(session, allRecepies);
        }

        private static void DisplayRecepiesInRecepieList(int index, NHibernate.ISession session)
        {
            index = index - 1;
            var recepies = session.Query<RecepieList>().ToList();
            foreach (Recepie rec in recepies[index].Recepies.Distinct())
            {
                Console.WriteLine(rec.Name);
            }
        }

        private static void CreateNewRecepie(NHibernate.ISession session)
        {
            Console.Clear();
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            name = ValidateName(name);

            Console.Write("Enter ingredients, finish with blank space: ");
            List<Ingredient> ingredients = EnterIngredients();

            Console.Write("Enter categoies (optional), finish with blank space: ");
            List<Category> categories = EnterCategories();

            InsertRecepieIntoDatabase(name, ingredients, categories, session);
        }

        private static void InsertRecepieIntoDatabase(string name, List<Ingredient> ingredients, List<Category> categories, NHibernate.ISession session)
        {
            Recepie recepie = new Recepie();
            recepie.Name = name;

            foreach (Ingredient ingredient in ingredients)
            {
                recepie.AddIngredient(ingredient);

                session.Save(ingredient);
            }

            foreach (Category category in categories)
            {
                recepie.AddCategory(category);
                category.AddRecepie(recepie);
                session.Save(category);
            }
            session.Save(recepie);
        }

        private static List<Category> EnterCategories()
        {
            List<Category> categories = new List<Category>();
            int categoryCounter = 1;
            Console.WriteLine($"Enter category {categoryCounter} (blank space to skip): ");
            Category category = new Category();
            category.Name = Console.ReadLine();
            while (category.Name != "")
            {
                categoryCounter++;
                Console.WriteLine($"Enter category {categoryCounter}: ");
                category.Name = Console.ReadLine();
                categories.Add(category);
                //TODO implement check
            }
            return categories;
        }

        private static List<Ingredient> EnterIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            int ingredientCounter = 0;
            Ingredient ingredient = new Ingredient();
            do
            {
                ingredientCounter++;
                Console.WriteLine($"Enter ingredient {ingredientCounter}: ");
                ingredient.Name = Console.ReadLine();
                ingredients.Add(ingredient);
                //TODO implement check

            } while (ingredient.Name != "");
            return ingredients;
        }

        private static string YesOrNo()//Checks that the answer is yes, y or no, n and returns y or n in upper case
        {
            string input = Console.ReadLine();
            while (!input.Equals("yes", StringComparison.InvariantCultureIgnoreCase) && !input.Equals("no", StringComparison.InvariantCultureIgnoreCase) && !input.Equals("y", StringComparison.InvariantCultureIgnoreCase) && !input.Equals("n", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.WriteLine("Enter answer again (y/n): ");
                input = Console.ReadLine();
            }
            if (input.Equals("yes", StringComparison.InvariantCultureIgnoreCase) || input.Equals("y", StringComparison.InvariantCultureIgnoreCase))
            {
                input = "y";
            }
            else
            {
                input = "n";
            }
            return input.ToUpper();
        }

        private static string ValidateName(string name)
        {
            while (!Regex.Match(name, "^[a-zA-ZåäöÅÄÖ ]+$").Success)
            {
                Console.Write("Invalid entry, enter name again: ");
                name = Console.ReadLine();
            }
            return FirstCharToUpper(name.ToLower());
        }

        private static string FirstCharToUpper(string input)
        {
            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
