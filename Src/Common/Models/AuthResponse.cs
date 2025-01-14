namespace Kurier.Common.Models
{
    public class AuthResponse
    {
        public UserAuthToken Token { get; set; }
        public string Message { get; set; }
    }
}
