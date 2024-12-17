namespace NotTinderAppMAUI.Models.DTOs;

public class ResponseDto<TData>
{
    public bool IsSuccess { get; set; }
    public TData Data { get; set; }
    public string Message { get; set; }
}

public class ResponseDto
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = "";
}