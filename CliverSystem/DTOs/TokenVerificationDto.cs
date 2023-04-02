﻿using System.ComponentModel.DataAnnotations;

namespace CliverSystem.DTOs
{
    public class TokenVerificationDto
    {
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Code { get; set; } = string.Empty;
    }
}
