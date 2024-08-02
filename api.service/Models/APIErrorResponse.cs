namespace APIService.Models;
public class APIErrorResponse
{
    public int Id { get; set; }

    public string? Status { get; set; }

    public ErrorModel? Result { get; set; }

    public string? Error { get; set; }

    public string? ErrorCode { get; set; }

    public APIErrorResponse(int id, string status, ErrorModel? errorModel)
    {
        Id = id;
        Status = status;
        Result = errorModel;
    }
}