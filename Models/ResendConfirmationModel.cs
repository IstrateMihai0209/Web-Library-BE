using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.Models;

public class ResendConfirmationModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}