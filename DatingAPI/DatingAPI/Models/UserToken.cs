using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DatingAPI.Models
{
    public class UserToken
    {
        public int Id { get; set; }
        public string? TokenString { get; set; }
        public string? TokenType { get; set; }
        [Required]
        public DateTime Expires { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}
