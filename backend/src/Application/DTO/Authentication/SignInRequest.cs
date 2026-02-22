namespace Application.DTO.Authentication;

public class SignInRequest
{
    public string EmailOrPhone { get; set; }
    public string Password { get; set; }
}
