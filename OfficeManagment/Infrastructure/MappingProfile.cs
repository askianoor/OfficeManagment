using AutoMapper;
using OfficeManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OfficeManagment.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RoomEntity, Room>().ForMember(dest => dest.Self, 
                opt => opt.MapFrom(src => Link.To(nameof(Controllers.RoomsController.GetRoomByIdAsync), new { roomId = src.Id })));
        }
    }
}
