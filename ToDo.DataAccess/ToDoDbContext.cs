using ToDo.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace ToDo.DataAccess
{
    public class ToDoDbContext : DbContext
    {
        public virtual DbSet<Deal> Deals { get; set; }

        public ToDoDbContext(DbContextOptions<ToDoDbContext> dbContextOptions, ILogger logger) : base(dbContextOptions)
        {
            logger.Information($"{nameof(ToDoDbContext)} created. Context Id: {this.ContextId}");
        }
    }
}
