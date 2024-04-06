using System;
using System.ComponentModel.DataAnnotations;
namespace Domain.Models
{
	public class Transaction : BaseModel
	{
		[MaxLength(50)]
		public required string Title { get; set; }

		public double Amount { get; set; }

		public Guid CategoryId { get; set; }

		public Guid AccountId { get; set; }

		[MaxLength(100)]
		public string Notes { get; set; } = string.Empty;

		[MaxLength(200)]
		public string Description { get; set; } = string.Empty;

		public virtual Category? Category { get; set; }

		public virtual Account? Account { get; set; }

	}
}

