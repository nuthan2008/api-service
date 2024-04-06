using System;
namespace Domain.Dto.Request
{
	public class AccountRequest
	{
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool isActive { get; set; }
    }
}

