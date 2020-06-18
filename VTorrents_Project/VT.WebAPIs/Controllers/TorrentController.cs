using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
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
    public class TorrentController : ControllerBase
    {
        private readonly TorrentService torrentService;

        public TorrentController()
        {
            this.torrentService = new TorrentService();
        }

        // GET: api/Torrent
        [HttpGet]
        public IEnumerable<TorrentDto> GetAll()
        {
            return torrentService.GetAll();
        }

        [HttpGet("subtypeId={subtypeId}")]
        public IEnumerable<TorrentDto> GetAllBySubType([FromRoute] int subtypeId)
        {
            return torrentService.GetAllBySubType(subtypeId);
        }

        [HttpGet("uploaderId={uploaderId}")]
        public IEnumerable<TorrentDto> GetAllByUploader([FromRoute] int uploaderId)
        {
            return torrentService.GetAllByUploader(uploaderId);
        }

        [HttpGet("downloaderId={downloaderId}")]
        public IEnumerable<TorrentDto> GetAllByDownloader([FromRoute] int downloaderId)
        {
            return torrentService.GetAllByDownloader(downloaderId);
        }

        [HttpGet("subtypeId={subtypeId}/{title}")]
        public IEnumerable<TorrentDto> GetAllWithTitle([FromRoute] int subtypeId, [FromRoute] String title)
        {
            if (title.IsNullOrEmpty())
            {
                return torrentService.GetAllBySubType(subtypeId);
            }
            return torrentService.GetAllWithTitle(subtypeId, title);
        }

        // GET: api/Torrent/5
        [HttpGet("{id}")]
        public ActionResult<TorrentDto> Get([FromRoute] int id)
        {
            var result = torrentService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/Torrent
        [HttpPost]
        public IActionResult Create([FromBody] TorrentDto torrent)
        {
            if (torrentService.Create(torrent))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // PUT: api/Torrent/5
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] TorrentDto torrent)
        {
            torrent.Id = id;

            if (torrentService.Update(torrent))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // DELETE: api/Torrent/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            UserService userService = new UserService();

            var user = userService.GetById(Int32.Parse(HttpContext.User.Claims.First().Value));

            if (user.isAdmin)
            {
                if (torrentService.Delete(id))
                {
                    return Ok();
                }
            }
            else
            {
                if (torrentService.FakeDelete(id))
                {
                    return Ok();
                }
            }

            return BadRequest();
        }

        [HttpGet("id={id}/downloaderId={downloaderId}")]
        public IActionResult Download([FromRoute]int id, [FromRoute]int downloaderId)
        {
            if (torrentService.Download(id,downloaderId))
            {
                return NoContent();
            }

            return BadRequest();
        }
    }
}
