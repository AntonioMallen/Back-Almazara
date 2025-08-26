using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Back_Almazara.Models;
using Back_Almazara.Repository.V1;
using Back_Almazara.Service.V1;
using Back_Almazara.Utility;
using Microsoft.Extensions.Configuration;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILoginRepository _loginEntity;
    private readonly IHashUtility _hashUtility;
    

    public EmailService(IConfiguration configuration, ILoginRepository loginEntity, IHashUtility hashUtility)
    {
        _configuration = configuration;
        _loginEntity = loginEntity;
        _hashUtility = hashUtility;
    }

    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = true)
    {
        var smtpClient = new SmtpClient
        {
            Host = _configuration["Email:Smtp:Host"],
            Port = int.Parse(_configuration["Email:Smtp:Port"]),
            EnableSsl = bool.Parse(_configuration["Email:Smtp:EnableSsl"]),
            Credentials = new NetworkCredential(
                _configuration["Email:Smtp:Username"],
                _configuration["Email:Smtp:Password"]
            )
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_configuration["Email:Smtp:From"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml,
        };

        mailMessage.To.Add(to);

        await smtpClient.SendMailAsync(mailMessage);
    }

    public bool UserExists(string email) {
        var user = _loginEntity.ExistsUser(email);

        return user.Success;
    }

    public string EmailUser(string userID)
    {
        var userIdI = (int)_hashUtility.FromBase36(userID ?? "");

        var user = _loginEntity.ExistsUser("", userIdI);
        
        return user.Data?.EmailNv ?? "";
    }
}
