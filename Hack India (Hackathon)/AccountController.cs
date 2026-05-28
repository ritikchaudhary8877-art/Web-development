using BusinessProAdvisor.Application.DTO.TemplateModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewProjectCleanArchitecture.Application.DTO;
using NewProjectCleanArchitecture.Application.IService;
using NewProjectCleanArchitecture.Application.Model;
using NewProjectCleanArchitecture.Domain.Entities;
using System.Net.Mail;

namespace NewProjectCleanArchitecture.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<EmployeeModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<EmployeeModel> _signInManager;
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService, UserManager<EmployeeModel> userManager, SignInManager<EmployeeModel> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _accountService = accountService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RegisterSuperAdmin()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterSuperAdmin(UserRegisrationDto userRegistrationDto)
        {
            TemplateResponse<UserRegisrationDto>? templateResponse = new();
            templateResponse.Success = false;
            templateResponse.Data = userRegistrationDto;
            templateResponse.Message = "Register Failed";
            try
            {
                ModelState.Remove("Message");
                if (ModelState.IsValid)
                {
                    templateResponse = await _accountService.RegisterSuperAdmin(userRegistrationDto).ConfigureAwait(false);
                    if (templateResponse.Success)
                    {
                        return Json(new { success = true, message = "Registration successful!", redirectUrl = Url.Action("Login", "Account") });

                    }
                    else
                    {
                        return Json(new { success = false, message = templateResponse.Message });

                    }
                }
                return Json(new { success = false, message = "Invalid Model State" });

            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Something went wrong" });

            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel userobj)
        {
            TemplateResponse<LoginViewModel>? templateResponse = new();
            templateResponse.Success = false;
            templateResponse.Data = userobj;
            templateResponse.Message = "Login Failed";

            if (ModelState.IsValid)
            {
                var userName = userobj.Email;
                if (IsValidEmail(userobj.Email))
                {
                    var user = await _userManager.FindByEmailAsync(userobj.Email);
                    if (user != null)
                    {
                        userName = user.UserName;
                    }
                    else
                    {
                        return Json(new { success = false, message = "This Email does not exist" });
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(userName, userobj.Password, userobj.Rememberme, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = "Login Successfully", redirectUrl = Url.Action("Index", "Home") });
                }
                else
                {
                    return Json(new { success = false, message = "Invalid credentials" });
                }
            }

            return Json(new { success = false, message = "Invalid Model State" });
        }
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="forgotPasswordViewModelDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModelDto forgotPasswordViewModelDto)
        {
            if (ModelState.IsValid)
            {
                UserForGetDto? user = await _accountService.GetUserDetailsByEmail(forgotPasswordViewModelDto.Email);
                if (user != null)
                {
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { Email = user.Email, Token = user.Token }, protocol: HttpContext.Request.Scheme);
                    user.passwordResetLink = passwordResetLink;
                    var ab = await _accountService.SendForgotPasswordEmail(user);
                    return RedirectToAction("ForgotPasswordConfirmation", "User");
                }
                return RedirectToAction("ForgotPasswordConfirmation", "User");
            }
            return View(forgotPasswordViewModelDto);
        }
        /// <summary>
        /// Forgot Password Confirmation method Page
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// User Controller action method Used to get reset password page.
        /// </summary>
        /// <returns>Return the reset password view.</returns>

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string Token, string Email)
        {
            if (Token == null || Email == null)
            {
                ViewBag.ErrorTitle = "Invalid Password Reset Token";
                ViewBag.ErrorMessage = "The Link is Expired or Invalid";
                return View("Error");
            }
            else
            {
                ResetPasswordViewModel model = new ResetPasswordViewModel();
                model.Token = Token;
                model.Email = Email;
                return View(model);
            }
        }
        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.ResetPassword(model);
                if (result.Success == true)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
                if (result.Message == "Invalid token.")
                {
                    ViewBag.ErrorTitle = "Invalid Password Reset Token";
                    ViewBag.ErrorMessage = "The Link is Expired or Invalid";
                }
            }
            return View(model);
        }
        /// <summary>
        /// User Controller action method Used to get reset password Confirmation page.
        /// </summary>
        /// <returns>Return the reset password view.</returns>
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        /// <summary>
        /// Change Password Get Method For view
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        /// <summary>
        /// User Controller action method Used to Post Change Password page.
        /// </summary>
        /// <returns>Return the reset password view.</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            TemplateResponse<ChangePasswordViewModel> templateResponse = new()
            {
                Success = false,
                Data = model,
                Message = "Add Data Failed"
            };
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                //fetch the User Details
                var loginUserId = User.Claims.ToList()[0].Value;
                model.Id = loginUserId;
                var repoResponse = await _accountService.GetUserDetailsByIdAsync(loginUserId);
                if (repoResponse == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var result = await _accountService.ChangePassword(model);
                if (repoResponse != null)
                {
                    if (repoResponse.Success)
                    {
                        return RedirectToAction("ChangePasswordConfirmation", "User");
                    }
                }
            }
            return View(model);
        }

        /// <summary>
        /// User Controller action method Used to get reset password page.
        /// </summary>
        /// <returns>Return the reset password view.</returns>
        [Authorize]
        [HttpGet]
        public IActionResult ChangePasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
    }
}