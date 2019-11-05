using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OfficeManagment.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class RoomsController : ControllerBase
    {
        private readonly OfficeApiContext _context;

        public RoomsController(OfficeApiContext context)
        {
            _context = context;
        }

        [HttpGet(Name = nameof(GetRooms))]
        public IActionResult GetRooms()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Api -> /rooms/{roomId}
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        [HttpGet("{roomId}", Name = nameof(GetRoomByIdAsync))]
        public async Task<IActionResult> GetRoomByIdAsync(Guid roomId, CancellationToken ct)
        {
            var entity = await _context.Rooms.SingleOrDefaultAsync(r => r.Id == roomId, ct);
            if (entity == null) return NotFound();

            var resource = new Room
            {
                Href = Url.Link(nameof(GetRoomByIdAsync), new { roomId = entity.Id }),
                Name = entity.Name,
                Rate = entity.Rate
            };

            return Ok(resource);
        }
    }
}
