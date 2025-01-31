namespace TestBackEnd.Domain.DTOs.Auth
{
    public class UserSessionDTO
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string[] UserRoles { get; set; }
    }
}
