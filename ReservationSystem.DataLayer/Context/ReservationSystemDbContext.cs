using Microsoft.EntityFrameworkCore;
using ReservationSystem.Domain.Entities;

namespace ReservationSystem.DataLayer.Context
{
    public class ReservationSystemDbContext : DbContext
    {
        #region Ctor

        public ReservationSystemDbContext(DbContextOptions<ReservationSystemDbContext> options) : base(options) { }

        #endregion

        #region DbSets

        public DbSet<User> Users { get; set; } = null!;

        public DbSet<Doctor> Doctors { get; set; } = null!;

        public DbSet<DoctorTiming> DoctorTimings { get; set; } = null!;

        public DbSet<Reservation> Reservations { get; set; } = null!;

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relation in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                relation.DeleteBehavior = DeleteBehavior.Restrict;
            }

            #region Seed Data

            modelBuilder.Entity<User>().HasData(new User()
            {
                Id = 1,
                FullName = "ادمین سایت",
                IsAdmin = true,
                Password = "$2a$11$L7BwWAEYOscnFnrFlP0eluWfe3MJbekiP2zH0.zRVlutHcYEZs5hK",
                RegisterDate = DateTime.MinValue,
                UserName = "admin"
            });

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
