using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationSystem.Domain.Entities;

namespace ReservationSystem.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsUserExistsByUserName(string userName);

        Task AddUser(User user);

        Task<User?> GetUserByUserName(string userName);

        Task<User?> GetUserById(int id);

        IQueryable<User> GetUsersAdQueryable();
    }
}
