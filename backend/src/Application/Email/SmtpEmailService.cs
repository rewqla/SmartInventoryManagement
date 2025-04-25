using Application.Interfaces;
using FluentEmail.Core;

namespace Application.Email;

public class SmtpEmailService : IEmailService
{
    private readonly IFluentEmail _fluentEmail;

    public SmtpEmailService(IFluentEmail fluentEmail)
    {
        _fluentEmail = fluentEmail;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            await _fluentEmail.To(to)
                .Subject(subject)
                .Body(body)
                .SendAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email to the {to} with error: {ex.InnerException?.Message ?? ex.Message}");
        }
    }
}