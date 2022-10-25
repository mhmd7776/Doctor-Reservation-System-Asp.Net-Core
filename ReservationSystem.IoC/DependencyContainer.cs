using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReservationSystem.Application.Services.Implementations;
using ReservationSystem.Application.Services.Interfaces;
using ReservationSystem.DataLayer.Repositories;
using ReservationSystem.Domain.Interfaces;

namespace ReservationSystem.IoC
{
    public class DependencyContainer
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            #region Services

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDoctorService, DoctorService>();

            #endregion

            #region Repositories

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDoctorRepository, DoctorRepository>();

            #endregion
        }
    }
}
