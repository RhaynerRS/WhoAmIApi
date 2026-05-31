using MongoDB.Driver;
using Richter.WhoAmIApi.Domain.Rooms.Entities;
using Richter.WhoAmIApi.Domain.Rooms.Repositories;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richter.WhoAmIApi.Infra.Rooms
{
    public class RoomRepository(IMongoDatabase mongo): IRoomRepository
    {
        private readonly IMongoCollection<GameRoom> collection = mongo.GetCollection<GameRoom>("Room");
        public async Task CreateRoom(GameRoom room, CancellationToken cancellation) {
            await collection.InsertOneAsync(room, cancellation);
        }
    }
}
