using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficeManagment.Models;

namespace OfficeManagment.Services
{
    public class DefaultRoomService : IRoomService
    {
        private readonly OfficeApiContext _context;

        private DefaultRoomService(OfficeApiContext context)
        {
            _context = context;
        }

        public async Task<Room> GetRoomAsync(Guid id, CancellationToken ct)
        {
            var entity = await _context.Rooms.SingleOrDefaultAsync(r => r.Id == id, ct);
            if (entity == null) return null; //NotFound();

            var resource = new Room
            {
                Href = null, // Url.Link(nameof(GetRoomByIdAsync), new { roomId = entity.Id }),
                Name = entity.Name,
                Rate = entity.Rate
            };

            return resource;
        }
    }
}
