namespace SCM_System_Api_Server.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}