namespace Domain.Models
{
    public class BaseModel
	{
		
		public Guid Id { get; set; }
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
		public int Status { get; set; }

    }
}

