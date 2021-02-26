﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Website.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Website.Data;
using Website.Services;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly AppDbContext _context;

        private static readonly string errorColor = "#ff4d4d";
        private static readonly string successColor = "#97ff80";

        public HomeController(UserManager<IdentityUser> usermanager, SignInManager<IdentityUser> signInManager, IEmailService emailService, IWebHostEnvironment webHostEnvironment, AppDbContext context)
        {
            _userManager = usermanager;
            _signInManager = signInManager;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        // Homepage is protected
        [Authorize]
        public IActionResult Index()
        {
            var dataModel = new DataModel();
            dataModel.Events = GetsEvents();
            dataModel.PioneerGroups = GetPioneers();

            return View(dataModel);
        }

        public IActionResult LockScreen()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            string email = user.Email;
            string password = user.Password;

            if (email == null)
            {
                TempDataError("Please enter an email!");
                return RedirectToAction(nameof(LockScreen));
            }

            var userFromDb = await _userManager.FindByEmailAsync(email);

            if (userFromDb == null)
            {
                TempDataError("Incorrect Email");
                return RedirectToAction(nameof(LockScreen));
            }

            bool success = await TrySignIn(password, userFromDb);
            if (!success)
            {
                TempDataError("Incorrect Password");
                return RedirectToAction(nameof(LockScreen));
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TrySignIn(string password, IdentityUser userFromDb)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(userFromDb, password, true, false);
            return signInResult.Succeeded;
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            string email = user.Email;
            string password = user.Password;

            if (!IsEmailValid(email))
            {
                TempDataError("Please use a valid, school email");
                return RedirectToAction(nameof(LockScreen));
            }

            if (string.IsNullOrEmpty(password))
            {
                TempDataError("Please enter a password");
                return RedirectToAction(nameof(LockScreen));
            }

            var userFromDb = await _userManager.FindByEmailAsync(email);

            if (userFromDb != null)
            {
                if (!userFromDb.EmailConfirmed)
                {
                    await SendVerificationEmail(userFromDb);
                    TempDataSuccess("A verification email has been sent!");

                    return RedirectToAction(nameof(LockScreen));
                }

                TempDataError("Account with that email already exists!");
                return RedirectToAction(nameof(LockScreen));
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
                await SendVerificationEmail(newUser);

                //System.Diagnostics.Debug.WriteLine("Registered!");
                TempDataSuccess("A verification email has been sent!");
                return RedirectToAction(nameof(LockScreen));
            }

            TempDataError("Server error, please try again!");
            return RedirectToAction(nameof(LockScreen));
        }

        private async Task SendVerificationEmail(IdentityUser newUser)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

            var link = Url.Action(nameof(VerifyEmail), "Home", new { userId = newUser.Id, code }, Request.Scheme, Request.Host.ToString());

            _emailService.Send(newUser.Email, "Email Verification", $"<a href=\"{link}\">Verify Email</a>");
        }

        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, code);

                if (result.Succeeded)
                {
                    TempDataSuccess("Email confirmed! You can now log in");
                    return RedirectToAction(nameof(LockScreen));
                }
            }

            TempDataError("Invalid link. Please ensure that the link is valid");
            return RedirectToAction(nameof(LockScreen));
        }

        public async Task<IActionResult> SendResetPassword(User u)
        {
            string email = u.Email;

            if (string.IsNullOrWhiteSpace(email))
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                TempDataError("No user found with the given email!");
                return RedirectToAction(nameof(LockScreen));
            }

            if (!_userManager.IsEmailConfirmedAsync(user).Result)
            {
                TempDataError("Email with given account is not verified!");
                return RedirectToAction(nameof(LockScreen));
            }

            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;

            var link = Url.Action(nameof(ResetPassword), "Home", new { email = user.Email, token }, Request.Scheme, Request.Host.ToString());

            _emailService.Send(user.Email, "Password Reset", $"<a>Follow the link to reset your password</a><br><br><a href=\"{link}\">Reset Password</a><br><a>You can ignore this email if you did not request a reset</a>");

            TempDataSuccess("Email has been sent");
            return RedirectToAction(nameof(LockScreen));
        }

        public IActionResult ResetPassword(string email, string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel obj)
        {
            System.Diagnostics.Debug.WriteLine($"Email: {obj.Email}\nPassword: {obj.Password}\nToken: {obj.Token}");
            var user = await _userManager.FindByEmailAsync(obj.Email);

            var result = await _userManager.ResetPasswordAsync(user, obj.Token, obj.Password);

            if (result.Succeeded)
            {
                TempDataSuccess("Password reset successful!");
            }
            else
            {
                TempDataError(result.Errors.ToString());
            }

            return RedirectToAction(nameof(LockScreen));
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(LockScreen));
        }

        #region Helper Methods

        private void TempDataError(string message)
        {
            TempData["Error"] = message;
            TempData["Color"] = errorColor;
        }

        private void TempDataSuccess(string message)
        {
            TempData["Error"] = message;
            TempData["Color"] = successColor;
        }

        private bool IsEmailValid(string email)
        {
            if (email == null) { return false; }

            int idx = email.LastIndexOf('@');

            if (idx == -1 ) { return false; }

            string domain = email.Substring(idx + 1).ToLower();    //domain half e.g. my.org

            return domain == "pathways.in" || domain == "pathwaysschool.in";
        }

        #endregion

        #region Data Model Helpers

        private Event[] GetsEvents()
        {
            return JsonConvert.DeserializeObject<Event[]>(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath, "data", "events.json")));
        }

        private PioneerGroup[] GetPioneers()
        {
            return JsonConvert.DeserializeObject<PioneerGroup[]>(System.IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath, "data", "pioneers.json")));
        }

        #endregion
    }
}
