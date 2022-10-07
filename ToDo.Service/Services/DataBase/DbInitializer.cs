using ToDo.DataAccess;
using ToDo.DataAccess.Entities;

namespace ToDo.Services.DataBase
{
    public static class DbInitializer
    {
        public static void Initialize(ToDoDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any Deals.
            if (context.Deals.Any())
            {
                return;   // DB has been seeded
            }

            var deals = new Deal[]
            {
            new Deal { Id = 1, Name = "купить продукты на борщ", ParentId = null },
            new Deal { Id = 2, Name = "сметана", ParentId = 1 },
            new Deal { Id = 3, Name = "свекла", ParentId = 1 },
            new Deal { Id = 4, Name = "отнести книги в библиотеку", ParentId = null },
            new Deal { Id = 5, Name = "Жюль Верн", ParentId = 4 },
            new Deal { Id = 6, Name = "Путешествие к центру Земли", ParentId = 5 },
            new Deal { Id = 7, Name = "Дети капитана Гранта", ParentId = 5 },
            };

            foreach (var d in deals)
            {
                context.Deals.Add(d);
            }
            
            context.SaveChanges();
        }
    }
}
