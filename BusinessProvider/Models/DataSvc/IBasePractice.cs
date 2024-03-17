using System.ComponentModel.DataAnnotations;

namespace BusinessProvider.Models.DataSvc
{
    public interface IBasePractice
    {
        [Key]
        string Id { get; set; }

        [Required]
        string BusinessName { get; set; }

        int IncorporationYear { get; set; }
    }
}