using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Richter.WhoAmIApi.Application.Identity.DataModule.Requests;
using Richter.WhoAmIApi.Application.Identity.Services;
using Richter.WhoAmIApi.Application.Room.DataModule.Response;
using Richter.WhoAmIApi.Application.Room.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richter.WhoAmIApi.API.Controllers
{
    [ApiController]
    [Route("rooms")]
    public class RoomController(IRoomAppService roomAppService) : ControllerBase
    {
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CreateRoomResponse>> CreateRoom(CancellationToken cancellation = default)
        {
            var room = await roomAppService.CreateGameRoom(cancellation);
            return Ok(room);
        }
    }
}
