using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace ToDo.DataAccess.Entities
{
    public class Deal
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public int? ParentId { get; set; }

        public bool Checked { get; set; }

    }
}
