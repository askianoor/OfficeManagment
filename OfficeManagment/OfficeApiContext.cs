using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OfficeManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManagment
{
    //Change DbContext to the IdentityDbContext
    public class OfficeApiContext : IdentityDbContext<UserEntity,UserRoleEntity,Guid>
    {
        public OfficeApiContext(DbContextOptions options) : base(options) { }

        public DbSet<RoomEntity> Rooms { get; set; }

        public DbSet<BookingEntity> Bookings { get; set; }
    }
}
