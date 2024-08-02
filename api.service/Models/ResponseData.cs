namespace APIService.Models;

public class ResponseData<T>
{
    public string Status { get; set; } = string.Empty;
    public required StatusDetails StatusDetails { get; set; }
    public required T DataObjs { get; set; }
}