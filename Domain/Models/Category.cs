using System;
using System.ComponentModel.DataAnnotations;
namespace Domain.Models
{
    public class Category : BaseModel
    {
        public Category()
        {
            Transactions = new HashSet<Transaction>();
        }

        [MaxLength(50)]
        public required string Name { get; set; }

        public Guid? CategoryId { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}

