using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Account : BaseModel
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }

        [MaxLength(50)]
        public required string Name { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool isActive { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}