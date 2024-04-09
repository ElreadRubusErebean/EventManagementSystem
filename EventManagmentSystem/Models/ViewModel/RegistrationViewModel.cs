namespace EventManagmentSystem.Models.ViewModel
{
    public class RegistrationViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; } = false;
        //Rolle des Users definieren entweder NormUser oder Seller
        public UserRole Role { get; set; } = UserRole.NormalUser; //default ist NormalUser
    }

    public enum UserRole
    {
        NormalUser,
        Seller
    }
}
