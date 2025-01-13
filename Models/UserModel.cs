using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public string Permission { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public int GoogleId { get; set; }

        public string ReturnUrl { get; set; }

        public bool RememberLogin { get; set; }
    }
}
