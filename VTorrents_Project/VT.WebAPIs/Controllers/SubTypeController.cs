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
    public class SubTypeController : ControllerBase
    {
        private readonly SubTypeService subTypeService;

        public SubTypeController()
        {
            this.subTypeService = new SubTypeService();
        }

        // GET: api/subType
        [HttpGet]
        public IEnumerable<SubTypeDto> GetAll()
        {
            return subTypeService.GetAll();
        }

        [HttpGet("catalogId={catalogId}")]
        public IEnumerable<SubTypeDto> GetAllWithTitle([FromRoute] int catalogId)
        {
                return subTypeService.GetAllByCatalog(catalogId);
        }

        [HttpGet("catalogId={catalogId}/{title}")]
        public IEnumerable<SubTypeDto> GetAllWithTitle([FromRoute] int catalogId, [FromRoute] String title)
        {
            if (title.IsNullOrEmpty())
            {
                return subTypeService.GetAllByCatalog(catalogId);
            }
            return subTypeService.GetAllWithTitle(catalogId, title);
        }

        // GET: api/subType/5
        [HttpGet("{id}")]
        public ActionResult<SubTypeDto> Get([FromRoute] int id)
        {
            var result = subTypeService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/subType
        [HttpPost]
        public IActionResult Create([FromBody] SubTypeDto subType)
        {
            if (subTypeService.Create(subType))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // PUT: api/subType/5
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] SubTypeDto subType)
        {
            subType.Id = id;

            if (subTypeService.Update(subType))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // DELETE: api/subType/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            UserService userService = new UserService();

            var user = userService.GetById(Int32.Parse(HttpContext.User.Claims.First().Value));

            if (user.isAdmin)
            {
                if (subTypeService.Delete(id))
                {
                    return Ok();
                }
            }
            else
            {
                if (subTypeService.FakeDelete(id))
                {
                    return Ok();
                }
            }

            return BadRequest();
        }
    }
}
