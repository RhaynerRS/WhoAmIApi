using Richter.WhoAmIApi.Domain.Rooms.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richter.WhoAmIApi.Domain.Rooms.Repositories
{
    public interface IRoomRepository
    {
        Task CreateRoom(GameRoom room, CancellationToken cancellation);
    }
}
