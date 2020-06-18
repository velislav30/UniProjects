using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VT.Business.DTOs;
using VT.Business.Services;

namespace VT.WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [ApiController]
    public class UserToTorrentController : ControllerBase
    {
        private readonly UserToTorrentService userToTorrentService;

        public UserToTorrentController()
        {
            this.userToTorrentService = new UserToTorrentService();
        }

        // GET: api/UserToTorrent
        [HttpGet]
        public IEnumerable<UserToTorrentDto> GetAll()
        {
            return userToTorrentService.GetAll();
        }

        // GET: api/UserToTorrent/5
        [HttpGet("{id}")]
        public ActionResult<UserToTorrentDto> Get([FromRoute] int id)
        {
            var result = userToTorrentService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/UserToTorrent
        [HttpPost]
        public IActionResult Create([FromBody] UserToTorrentDto userToTorrent)
        {
            if (userToTorrentService.Create(userToTorrent))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // PUT: api/UserToTorrent/5
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UserToTorrentDto userToTorrent)
        {
            userToTorrent.Id = id;

            if (userToTorrentService.Update(userToTorrent))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // DELETE: api/UserToTorrent/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            UserService userService = new UserService();

            var user = userService.GetById(Int32.Parse(HttpContext.User.Claims.First().Value));

            if (user.isAdmin)
            {
                if (userToTorrentService.Delete(id))
                {
                    return Ok();
                }
            }
            else
            {
                if (userToTorrentService.FakeDelete(id))
                {
                    return Ok();
                }
            }

            return BadRequest();
        }
    }
}
