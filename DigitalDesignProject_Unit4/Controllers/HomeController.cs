using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Threading.Tasks;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signInManager, IEmailService emailService)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        // Return to Index
        public IActionResult Index()
        {
            return View();
        }

        // Homepage is protected
        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        private async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                // sign in
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, true, false);

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Secret");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> TryRegister(string email, string password)
        {
            if (email == null || password == null ||  password.Length < 6)
            {
                return RedirectToAction("Index");
            }

            return await Register(email, password);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            var user = new IdentityUser
            {
                UserName = "user",
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                // sign in
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = user.Id , code }, Request.Scheme, Request.Host.ToString());

                await _emailService.SendAsync(email, "Email Verification", $"<a href=\"{link}\">Verify Email</a>", true);

                return RedirectToAction("EmailVerification");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) { return BadRequest(); }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return View();
            }

            return BadRequest();
        }

        public IActionResult EmailVerification() => View();

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
