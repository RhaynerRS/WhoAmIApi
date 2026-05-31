using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Richter.WhoAmIApi.CrossCutting.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Richter.WhoAmIApi.Domain.Rooms.Entities
{
    public class GameRoom
    {
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string OwnerId { get; set; }
        public List<string> GuestIds { get; set; } = [];

        protected GameRoom() { }

        public GameRoom(string ownerId)
        {
            Id = Guid.NewGuid().ToString();
            Code = CreateCode();
            OwnerId = ownerId;
        }

        public static string CreateCode()
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(32);
            string base64 = Convert.ToBase64String(randomBytes);
            return base64[..5];
        }

        public void AddGuest(string guestId)
        {
            if (GuestIds.Count > 3)
                throw new RegraDeNegocioException("The room is full");

            if (GuestIds.Contains(guestId) || guestId==OwnerId)
                throw new RegraDeNegocioException("That user is alredy on the room");

            GuestIds.Add(guestId);
        }

        public void RemoveGuest(string guestId)
        {
            GuestIds.Remove(guestId);
        }
    }
}
