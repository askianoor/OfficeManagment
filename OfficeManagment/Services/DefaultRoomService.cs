using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OfficeManagment.Models;

namespace OfficeManagment.Services
{
    public class DefaultRoomService : IRoomService
    {
        private readonly OfficeApiContext _context;
        private readonly IMapper _mapper;

        public DefaultRoomService(OfficeApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Room> GetRoomAsync(Guid id, CancellationToken ct)
        {
            var entity = await _context.Rooms.SingleOrDefaultAsync(r => r.Id == id, ct);
            if (entity == null) return null; //NotFound();
            
            return _mapper.Map<Room>(entity);
        }

        public async Task<IEnumerable<Room>> GetRoomsAsync(CancellationToken ct)
        {
            var query = _context.Rooms
                .ProjectTo<Room>(_mapper.ConfigurationProvider);

            return await query.ToArrayAsync();
        }
    }
}
