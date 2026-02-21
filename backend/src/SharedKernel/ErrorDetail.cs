namespace SharedKernel;

public record ErrorDetail
{
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
}
