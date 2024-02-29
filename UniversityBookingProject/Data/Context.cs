using Microsoft.EntityFrameworkCore;
using UniversityBookingProject.Models;
namespace UniversityBookingProject.Data
{
    public class Context : DbContext
    {
        public DbSet<RoomBooking> RoomBookings { get; set; }

        public Context(DbContextOptions<Context> options) : base(options)
        {

        }
    }
}
