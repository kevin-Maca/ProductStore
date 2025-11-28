namespace ProductStore.Web.DTOs
{
    public class UserTokenDTO
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
