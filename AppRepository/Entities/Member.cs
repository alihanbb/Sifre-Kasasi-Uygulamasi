namespace AppRepository.Entities
{
    public class Member
    {
        public int Id { get; set; } 
        public string WebName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public AppUser AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}
