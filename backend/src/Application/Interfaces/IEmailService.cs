namespace Application.Interfaces;

public interface IEmailService
{
    //todo: add email metadata https://www.youtube.com/watch?v=y4Q-Hp7HY_U
    //todo: send by template
    //todo: handle errors, as smtp is not the main thing in the application
    Task SendEmailAsync(string to, string subject, string body);
}