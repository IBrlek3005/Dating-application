using System.ComponentModel.DataAnnotations;

namespace DatingAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required] 
        public string? UserName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
        public byte[]? Salt { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }
    }
}
