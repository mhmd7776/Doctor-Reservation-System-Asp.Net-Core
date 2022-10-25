using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationSystem.Domain.ViewModels;

namespace ReservationSystem.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<RegisterUserResult> RegisterUser(RegisterUserViewModel registerUserViewModel);

        Task<LoginUserResult> CheckUserForLogin(LoginUserViewModel loginUserViewModel);

        Task<int> GetUserIdByUserName(string userName);

        Task<bool> IsUserAdmin(int userId);

        Task<List<SelectListViewModel>> GetNotDoctorUsersSelectList();
    }
}
