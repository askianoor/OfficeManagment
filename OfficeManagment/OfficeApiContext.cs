using Microsoft.EntityFrameworkCore;
using OfficeManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManagment
{
    public class OfficeApiContext : DbContext
    {
        public OfficeApiContext(DbContextOptions options) : base(options) { }

        public DbSet<RoomEntity> Rooms { get; set; }

        public DbSet<BookingEntity> Bookings { get; set; }
    }
}
