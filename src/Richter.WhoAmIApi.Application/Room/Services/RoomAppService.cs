using Mapster;
using Richter.WhoAmIApi.Application.Room.DataModule.Response;
using Richter.WhoAmIApi.Application.Room.Services.Interfaces;
using Richter.WhoAmIApi.CrossCutting.DTO;
using Richter.WhoAmIApi.Domain.Rooms.Entities;
using Richter.WhoAmIApi.Domain.Rooms.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richter.WhoAmIApi.Application.Room.Services
{
    public class RoomAppService(IRoomRepository roomRepository, UsuarioAutenticadoDto applicant) : IRoomAppService
    {
        public async Task<CreateRoomResponse> CreateGameRoom(CancellationToken cancellation)
        {
            GameRoom room = new(applicant.Id);
            await roomRepository.CreateRoom(room, cancellation);
            return room.Adapt<CreateRoomResponse>();
        } 
    }
}
