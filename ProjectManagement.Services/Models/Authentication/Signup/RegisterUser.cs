﻿using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.Models.Authentication.Signup
{
    public class RegisterUser
    {
        [Required]
        public string? UserName { get; set; }
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}