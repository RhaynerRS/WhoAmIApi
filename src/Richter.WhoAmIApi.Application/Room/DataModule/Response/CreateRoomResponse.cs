using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richter.WhoAmIApi.Application.Room.DataModule.Response
{
    public class CreateRoomResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string OwnerId { get; set; }
        public List<string> GuestIds { get; set; }
    }
}
