using System.ComponentModel.DataAnnotations;

namespace DatingAPI.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string? TokenString { get; set; }
        [Required]
        public DateTime ExpiresOn { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public  User? User { get; set; }
        public Guid UserId { get; set; }
        public string SessionId { get; set; }
    }
}
