using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.Application.Security;
using ReservationSystem.Application.Services.Interfaces;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Interfaces;
using ReservationSystem.Domain.ViewModels;

namespace ReservationSystem.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        #region Ctor

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        #endregion

        public async Task<RegisterUserResult> RegisterUser(RegisterUserViewModel registerUserViewModel)
        {
            #region Sanitize Input Values

            registerUserViewModel.UserName = registerUserViewModel.UserName.Trim().ToLower().SanitizeText();
            registerUserViewModel.FullName = registerUserViewModel.FullName.Trim().SanitizeText();
            registerUserViewModel.Password = registerUserViewModel.Password.SanitizeText();
            registerUserViewModel.ConfirmPassword = registerUserViewModel.ConfirmPassword.SanitizeText();

            #endregion

            if (await _userRepository.IsUserExistsByUserName(registerUserViewModel.UserName))
            {
                return RegisterUserResult.UserNameExists;
            }

            var user = new User
            {
                UserName = registerUserViewModel.UserName,
                FullName = registerUserViewModel.FullName,
                Password = PasswordHasher.BCryptHashPassword(registerUserViewModel.Password)
            };

            await _userRepository.AddUser(user);

            return RegisterUserResult.Success;
        }

        public async Task<LoginUserResult> CheckUserForLogin(LoginUserViewModel loginUserViewModel)
        {
            #region Sanitize Input Values

            loginUserViewModel.UserName = loginUserViewModel.UserName.Trim().ToLower().SanitizeText();
            loginUserViewModel.Password = loginUserViewModel.Password.SanitizeText();

            #endregion

            var user = await _userRepository.GetUserByUserName(loginUserViewModel.UserName);

            if (user == null) return LoginUserResult.UserNotFound;

            if (!PasswordHasher.BCryptVerifyPassword(loginUserViewModel.Password, user.Password))
            {
                return LoginUserResult.WrongPassword;
            }

            return LoginUserResult.Success;
        }

        public async Task<int> GetUserIdByUserName(string userName)
        {
            #region Sanitize Input Values

            userName = userName.Trim().ToLower().SanitizeText();

            #endregion

            var user = await _userRepository.GetUserByUserName(userName);

            return user?.Id ?? int.MinValue;
        }

        public async Task<bool> IsUserAdmin(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null) return false;

            return user.IsAdmin;
        }

        public async Task<List<SelectListViewModel>> GetNotDoctorUsersSelectList()
        {
            var usersQueryable = _userRepository.GetUsersAdQueryable();

            var result = await usersQueryable
                .Where(s => s.Doctor == null)
                .Select(s => new SelectListViewModel
                {
                    Id = s.Id,
                    Title = s.FullName
                }).ToListAsync();

            return result;
        }
    }
}
