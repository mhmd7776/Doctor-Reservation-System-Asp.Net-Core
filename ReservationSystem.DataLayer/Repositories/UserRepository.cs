using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReservationSystem.DataLayer.Context;
using ReservationSystem.Domain.Entities;
using ReservationSystem.Domain.Interfaces;

namespace ReservationSystem.DataLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Ctor

        private readonly ReservationSystemDbContext _context;

        public UserRepository(ReservationSystemDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<bool> IsUserExistsByUserName(string userName)
        {
            return await _context.Users
                .AnyAsync(s => s.UserName.Equals(userName));
        }

        public async Task AddUser(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByUserName(string userName)
        {
            return await _context.Users
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(s => s.UserName.Equals(userName));
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users
                .Include(s => s.Doctor)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public IQueryable<User> GetUsersAdQueryable()
        {
            return _context.Users
                .Include(s => s.Doctor)
                .AsQueryable();
        }
    }
}
