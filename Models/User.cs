using System.ComponentModel.DataAnnotations;

namespace BackEnd_2024_Project.Models
{
    public class User
    {
        public long Id { get; set; }

        [MinLength(3)]
        [MaxLength(30)]
        [Required]
        public string UserName { get; set; }

        [MaxLength(100)]
        [Required]
        public string Password { get; set; }
        public byte[]? Salt { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(30)]
        public string? FirstName { get; set; }

        [MaxLength(30)]
        public string? LastName { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

    }
    public class UserDTO
    {

        [MinLength(3)]
        [MaxLength(30)]
        public string UserName { get; set; }

        [MaxLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [MaxLength(30)]
        public string? FirstName { get; set; }

        [MaxLength(30)]
        public string? LastName { get; set; }
        public DateTime? JoinDate { get; set; }
        public DateTime? LastLoginDate { get; set; }


    }
}
