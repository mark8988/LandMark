using LandMark.EF;

namespace LandMark.Middleware.ViewModels.Admin_Info
{
    public class Admin_VM : Admin
    {
        public string GroupName { get; set; }
    }

    public class Admin_Login
    {
        public string account { get; set; }
        public string password { get; set; }
    }
    public class LoginResponse
    {
        public string Token { get; set; }
        public List<string> Menus { get; set; }
    }
}
