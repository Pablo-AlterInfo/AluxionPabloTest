using System.ComponentModel.DataAnnotations;

namespace AluxionTest.Models
{
    public class DbUser
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
