using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models
{
    public class RoleModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }
    }
}
