using ToDo.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ToDo.DataAccess
{
    public class ToDoDbContext : DbContext
    {
        public virtual DbSet<Deal> Deals { get; set; }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> dbContextOptions) : base(dbContextOptions)
        {
            Console.WriteLine("dbContext created: " + this.ContextId);
        }
    }
}
