namespace APIService.Models;

public class StatusDetails
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public int Total { get; set; }
}