using FestivalZnanostiApi.Enums;

namespace FestivalZnanostiApi.Middlewares.UserContext
{
    public class UserContext
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public UserRole? Role { get; set; }
    }
}
