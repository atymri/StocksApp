using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures; 
    
namespace StocksApp.ServiceContracts.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email can't be null.")]
        [EmailAddress(ErrorMessage = "Email Address is not in correct format.")]
        [Remote(action: "IsEmailInUseForLogin",  controller: "Account",ErrorMessage = "Email is not registred")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password can't be null.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
