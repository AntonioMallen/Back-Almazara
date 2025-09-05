namespace Back_Almazara.DTOS
{
    public class JwtSettingsDTO
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
    }
}
