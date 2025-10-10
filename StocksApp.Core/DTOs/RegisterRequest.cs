using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.ServiceContracts.DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Full Name can't be null.")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Email can't be null.")]
        [EmailAddress(ErrorMessage = "Email Address is not in correct format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number can't be null.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number is not in correct format.")]
        [StringLength(11, ErrorMessage = "Phone number must be 11 characters.")]
        [DataType(DataType.PhoneNumber)]
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
