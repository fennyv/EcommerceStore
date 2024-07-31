using EcommerceStoreMVC.Mapping;
using EcommerceStoreMVC.Models.DTO;
using EcommerceStoreMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EcommerceStoreMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    Address = registerDto.Address,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    IsDeleted = false


                };

                var result = _userManager.CreateAsync(user, registerDto.Password).Result;

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "client");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View(registerDto);

        }

        public async Task<IActionResult> Logout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                await _signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, loginDto.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ViewBag.ErrorMessages = "Invalid Login Attempt";

                //                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(loginDto);
        }

        [Authorize]
        public IActionResult Profile()
        {
            var appUser = _userManager.GetUserAsync(User).Result;

            if (appUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var mapper = new ProfileMapper();
            var profileDto = mapper.ApplicationUserToProfileDto(appUser);



            return View(profileDto);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Profile(ProfileDto profileDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Pleasefill all required fields with valid values";
            }
                else
            {
                var appUser = _userManager.GetUserAsync(User).Result;

                if (appUser == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                appUser.FirstName = profileDto.FirstName;
                appUser.LastName = profileDto.LastName;
                appUser.PhoneNumber = profileDto.PhoneNumber;
                appUser.Address = profileDto.Address;
                appUser.UpdatedDate = DateTime.Now;
                appUser.Email = profileDto.Email;

                

                var result = _userManager.UpdateAsync(appUser).Result;

                if (result.Succeeded)
                {
                    ViewBag.SuccessMessage = "Profile updated successfully";
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(profileDto);

        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize]
        public IActionResult UpdatePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]

        public IActionResult UpdatePassword(PasswordDto passwordDto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Please fill all required fields with valid values";
            }
            else
            {
                var appUser = _userManager.GetUserAsync(User).Result;

                if (appUser == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var result = _userManager.ChangePasswordAsync(appUser, passwordDto.CurrentPassword, passwordDto.NewPassword).Result;

                if (result.Succeeded)
                {
                    ViewBag.SuccessMessage = "Password updated successfully";

                    
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(passwordDto);
        }

       
        public IActionResult ForgotPassword()
        {
            if(_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword([Required, EmailAddress] string email)
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Email = email;
            if(!ModelState.IsValid)
            {
                ViewBag.EmailError = ModelState["email"].Errors[0].ErrorMessage?? "Invalid Email Address";
                 
            }
            else
            {
                var appUser = _userManager.FindByEmailAsync(email).Result;

                if(appUser == null)
                {
                    ViewBag.ErrorMessage = "User with the provided email not found";
                }
                else
                {
                    var token = _userManager.GeneratePasswordResetTokenAsync(appUser).Result;

                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = email, token = token }, Request.Scheme) ?? "Url Link Error";

                    //ViewBag.PasswordResetLink = passwordResetLink;
                    // send email
                    var senderName = _configuration["Brevo:SenderName"] ?? "";
                    var senderEmail = _configuration["Brevo:SenderEmail"] ?? "";
                    var username = appUser.FirstName + " " + appUser.LastName;
                    var subject = "Password Reset Link";
                    var body = $"Dear {username}, <br/> Please click on the link below to reset your password <br/> <a href='{passwordResetLink}'>Reset Password</a>";
                    EmailSender.SendEmail(senderName, senderEmail, username, email, subject, body);



                    ViewBag.SuccessMessage = "Password reset link has been sent to your email address";
                }
            }
            return View();
        }
    }
}
