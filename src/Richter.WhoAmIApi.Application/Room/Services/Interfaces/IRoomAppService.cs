using Richter.WhoAmIApi.Application.Room.DataModule.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richter.WhoAmIApi.Application.Room.Services.Interfaces
{
    public interface IRoomAppService
    {
        Task<CreateRoomResponse> CreateGameRoom(CancellationToken cancellation);
    }
}
