namespace FestivalZnanostiApi.DTOs
{
    public class ChangePasswordDto
    {
        public int AccountId { get; set; }
        public string? OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
