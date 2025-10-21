using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksApp.Core.Domain.IdentityEntities;
using StocksApp.Web.Filters.ActionFilters;

namespace StocksApp.Web.Controllers
{
    /// <summary>  
    /// The AccountController handles user authentication and registration-related actions.  
    /// It provides endpoints for user login, registration, logout, and validation of user details.  
    /// </summary>  
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        /// <summary>  
        /// Initializes a new instance of the <see cref="AccountController"/> class.  
        /// </summary>  
        /// <param name="userManager">The UserManager service for managing user-related operations.</param>  
        /// <param name="signInManager">The SignInManager service for handling user sign-in operations.</param>  
        /// <param name="logger">The logger instance for logging information and errors.</param>  
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>  
        /// Displays the registration page for new users.  
        /// </summary>  
        /// <returns>The registration view.</returns>  
        [HttpGet]
        [Authorize(Policy = "NotAuthenticated")]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>  
        /// Handles the registration of a new user.  
        /// </summary>  
        /// <param name="request">The registration request containing user details.</param>  
        /// <returns>Redirects to the Trade page on success, or redisplays the registration view on failure.</returns>  
        [HttpPost]
        [Authorize(Policy = "NotAuthenticated")]
        public async Task<IActionResult> Register(ServiceContracts.DTOs.RegisterRequest request)
        {
            _logger.LogInformation("{ClassName}.{MethodName}", nameof(AccountController), nameof(Register));
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return View(request);
            }

            var user = new ApplicationUser()
            {
                Email = request.Email,
                FullName = request.FullName,
                PhoneNumber = request.Phone,
                IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserName = request.Email // user uses email to log in.  
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return View(request);
            }

            // Sign in  
            _logger.LogInformation($"Successfully Registered {request.Email}");
            await _signInManager.SignInAsync(user, request.RememberMe);
            return RedirectToAction(nameof(TradeController.Index), "Trade");
        }

        /// <summary>  
        /// Displays the login page for users.  
        /// </summary>  
        /// <returns>The login view.</returns>  
        [HttpGet]
        [Authorize(Policy = "NotAuthenticated")]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>  
        /// Handles user login.  
        /// </summary>  
        /// <param name="request">The login request containing user credentials.</param>  
        /// <param name="returnUrl">The URL to redirect to after successful login.</param>  
        /// <returns>Redirects to the specified return URL or the Trade page on success, or redisplays the login view on failure.</returns>  
        [HttpPost]
        [Authorize(Policy = "NotAuthenticated")]
        public async Task<IActionResult> Login(ServiceContracts.DTOs.LoginRequest request, string? returnUrl)
        {
            _logger.LogInformation("{ClassName}.{MethodName}", nameof(AccountController), nameof(Login));
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();
                return View(request);
            }

            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password,
                request.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Login", "Your account is locked. Please try again later.");
                }

                ModelState.AddModelError("Login", "Invalid Email or Password.");
                return View(request);
            }
            _logger.LogInformation($"Successfully logged in {request.Email}");

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction(nameof(TradeController.Index), "Trade");
        }

        /// <summary>  
        /// Logs out the currently signed-in user.  
        /// </summary>  
        /// <returns>Redirects to the Trade page after logout.</returns>  
        [Authorize]
        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(TradeController.Index), "Trade");
        }

        /// <summary>  
        /// Checks if an email is already in use during registration.  
        /// </summary>  
        /// <param name="email">The email to check.</param>  
        /// <returns>A JSON result indicating whether the email is available.</returns>  
        [AllowAnonymous]
        [OnlyAjaxActionFilter]
        public async Task<IActionResult> IsEmailInUseForRegister(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return Json(result == null);
        }

        /// <summary>  
        /// Checks if an email is already registered for login.  
        /// </summary>  
        /// <param name="email">The email to check.</param>  
        /// <returns>A JSON result indicating whether the email is registered.</returns>  
        [AllowAnonymous]
        [OnlyAjaxActionFilter]
        public async Task<IActionResult> IsEmailInUseForLogin(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return Json(result != null);
        }

        /// <summary>  
        /// Checks if a phone number is already in use.  
        /// </summary>  
        /// <param name="phone">The phone number to check.</param>  
        /// <returns>A JSON result indicating whether the phone number is available.</returns>  
        [AllowAnonymous]
        [OnlyAjaxActionFilter]
        public IActionResult CheckIfPhoneNumberExists(string phone)
        {
            var result = _userManager.Users.Any(u => u.PhoneNumber == phone);
            return Json(!result);
        }
    }
}
