namespace Application.Interfaces;

public interface IEmailService
{
    //todo: add email metadata https://www.youtube.com/watch?v=y4Q-Hp7HY_U
    Task SendEmailAsync(string to, string subject, string body);
    Task SendEmailWithTemplateAsync<T>(string to, string subject, string template, T model);
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, string attachmentPath);
}