using Cubipool.Entity;
using Microsoft.EntityFrameworkCore;

namespace Cubipool.Repository.Context
{
    public class EFDbContext : DbContext
    {
        public DbSet<Resource> Resources { get; set; }
        public DbSet<Cubicle> Cubicles { get; set; }
        public DbSet<PointsRecord> PointsRecords { get; set; }
        public DbSet<Prize> Prizes { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<SharedSpace> SharedSpaces { get; set; }
        public DbSet<Constant> Constants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPrize> UserPrizes { get; set; }
        public DbSet<UserReservation> UserReservations { get; set; }

        public EFDbContext(DbContextOptions<EFDbContext> options)
            : base(options)
        {
        }
    }
}
