
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Threading.Tasks;
using Website.Models;

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

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            string email = user.email;
            string password = user.password;

            if (email == null)
            {
                TempData["Error"] = "Please enter an email!";
                return RedirectToAction("Index");
            }

            var userFromDb = await _userManager.FindByEmailAsync(email);

            if (userFromDb == null)
            {
                TempData["Error"] = "No account is linked with the email provided";
                return RedirectToAction("Index");
            }

            // sign in
            var signInResult = await _signInManager.PasswordSignInAsync(userFromDb, password, true, false);

            if (!signInResult.Succeeded)
            {
                TempData["Error"] = "Incorrect password!";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Secret");

        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            string email = user.email;
            string password = user.password;

            System.Diagnostics.Debug.WriteLine("Register Post");
            if (email != null && password != null && email.Length > 2 && password.Length > 5)
            {
                var userFromDb = await _userManager.FindByEmailAsync(email);

                if (userFromDb != null)
                {
                    //user.error = "Account with that email already exists!";
                    TempData["Error"] = "Account with that email already exists!";
                    return RedirectToAction("Index");
                }

                var newUser = new IdentityUser
                {
                    UserName = email,
                    Email = email
                };

                var result = await _userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    // sign in
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                    var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = newUser.Id, code }, Request.Scheme, Request.Host.ToString());

                    await _emailService.SendAsync(email, "Email Verification", $"<a href=\"{link}\">Verify Email</a>", true);

                    //System.Diagnostics.Debug.WriteLine("Registered!");
                    return RedirectToAction("EmailVerification");

                    //return RedirectToAction("Secret");
                }
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
                return RedirectToAction("Index");
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
