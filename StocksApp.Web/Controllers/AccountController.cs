using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksApp.Core.Domain.IdentityEntities;

namespace StocksApp.Web.Controllers
{
    [Route("[controller]/[action]")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
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
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }
                return View(request);
            }

            // Sign in
            _logger.LogInformation($"Successfully Registred {request.Email}");
            await _signInManager.SignInAsync(user, request.RememberMe);
            return RedirectToAction(nameof(TradeController.Index), "Trade");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // just creates an authorization cookie.
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
        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(TradeController.Index), "Trade");
        }
        public async Task<IActionResult> IsEmailInUseForRegister(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return Json(result == null);
        }
        public async Task<IActionResult> IsEmailInUseForLogin(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return Json(result != null);
        }
        public IActionResult CheckIfPhoneNumberExists(string phone)
        {
            var result = _userManager.Users.Any(u => u.PhoneNumber == phone);
            return  Json(!result);
        }
    }
}
