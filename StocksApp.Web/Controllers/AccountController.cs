using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using StocksApp.Core.Domain.IdentityEntities;
using StocksApp.ServiceContracts.DTOs;

namespace StocksApp.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(ServiceContracts.DTOs.RegisterRequest request)
        {
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
            await _signInManager.SignInAsync(user, request.RememberMe);
            return RedirectToAction(nameof(TradeController.Index), "Trade");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // just creates an authorization cookie.
        public async Task<IActionResult> Login(ServiceContracts.DTOs.LoginRequest request)
        {
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
                ModelState.AddModelError("Login", "Invalid Email or Password.");
                return View(request);
            }
            return RedirectToAction(nameof(TradeController.Index), "Trade");
        }

        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(TradeController.Index), "Trade");
        }
    }
}
