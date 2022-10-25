using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem.Application.Security;
using ReservationSystem.Application.Services.Interfaces;
using ReservationSystem.Domain.ViewModels;

namespace ReservationSystem.Web.Controllers
{
    public class AccountController : BaseController
    {
        #region Ctor

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Register

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel registerUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "مقادیر ورودی معتبر نمی باشد";
                return View(registerUserViewModel);
            }

            var registerResult = await _userService.RegisterUser(registerUserViewModel);

            switch (registerResult)
            {
                case RegisterUserResult.Success:
                    TempData[SuccessMessage] = "عملیات ثبت نام با موفقیت انجام شد";
                    return RedirectToAction("Login", "Account");
                case RegisterUserResult.UserNameExists:
                    ModelState.AddModelError("UserName", "نام کاربری وارد شده از قبل موجود است");
                    break;
            }

            return View(registerUserViewModel);
        }

        #endregion

        #region Login

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel loginUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData[ErrorMessage] = "مقادیر ورودی معتبر نمی باشد";
                return View(loginUserViewModel);
            }

            var loginResult = await _userService.CheckUserForLogin(loginUserViewModel);

            switch (loginResult)
            {
                case LoginUserResult.Success:
                    break;
                case LoginUserResult.UserNotFound:
                    ModelState.AddModelError("UserName", "کاربر مورد نظر یافت نشد");
                    return View(loginUserViewModel);
                case LoginUserResult.WrongPassword:
                    ModelState.AddModelError("Password", "کلمه عبور وارد شده صحیح نمی باشد");
                    return View(loginUserViewModel);
            }

            #region Login User

            var userId = await _userService.GetUserIdByUserName(loginUserViewModel.UserName);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var properties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(principal, properties);

            TempData[SuccessMessage] = "شما با موفقیت وارد شدید";

            #endregion

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Logout

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        #endregion
    }
}
