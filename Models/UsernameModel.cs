using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models;

public class UsernameModel
{
    [Required]
    public string Username { get; set; }
}