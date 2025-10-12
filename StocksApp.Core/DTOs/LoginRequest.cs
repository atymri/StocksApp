using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApp.ServiceContracts.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email can't be null.")]
        [EmailAddress(ErrorMessage = "Email Address is not in correct format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password can't be null.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
