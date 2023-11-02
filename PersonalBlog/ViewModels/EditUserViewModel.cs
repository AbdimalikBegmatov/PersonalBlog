namespace PersonalBlog.ViewModels
{
    public class EditUserViewModel
    {
        public string? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
