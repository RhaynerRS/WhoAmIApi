namespace Richter.WhoAmIApi.CrossCutting.DTO
{
    public class JwtCreationDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiracao { get; set; }
    }
}
