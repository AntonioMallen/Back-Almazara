namespace Back_Almazara.Service.V1
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);

        bool UserExists(string email);

        public string EmailUser(string userID);
    }
}
