using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace StocksApp.ServiceContracts.DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Full Name can't be null.")]
        public string FullName { get; set; }
        
        [Required(ErrorMessage = "Email can't be null.")]
        [EmailAddress(ErrorMessage = "Email Address is not in correct format.")]
        [Remote(action: "IsEmailInUseForRegister", controller: "Account", ErrorMessage = "Email Address is already in use")]
        public string Email { get; set; }
        
        
        [Required(ErrorMessage = "Phone number can't be null.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number is not in correct format.")]
        [StringLength(11, ErrorMessage = "Phone number must be 11 characters.")]
        [DataType(DataType.PhoneNumber)]
        [Remote(action:"CheckIfPhoneNumberExists", controller: "Account", ErrorMessage = "Phone number is in use")]
        public string Phone { get; set; }
        
        
        [Required(ErrorMessage = "Password can't be null.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        
        [Required(ErrorMessage = "Confirm password can't be null.")]
        [Compare(nameof(Password), ErrorMessage = "Password and its confirmation are not same.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool RememberMe { get; set; }
    }
}
