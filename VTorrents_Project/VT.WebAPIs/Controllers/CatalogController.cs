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
    public class CatalogController : ControllerBase
    {
        private readonly CatalogService catalogService;

        public CatalogController()
        {
            this.catalogService = new CatalogService();
        }

        // GET: api/catalog
        [HttpGet]
        public IEnumerable<CatalogDto> GetAll()
        {
            return catalogService.GetAll();
        }

        [HttpGet("title={title}")]
        public IEnumerable<CatalogDto> GetAllWithTitle([FromRoute] String title)
        {
            if (title.IsNullOrEmpty())
            {
                return catalogService.GetAll();
            }
            return catalogService.GetAllWithTitle(title);
        }

        // GET: api/catalog/5
        [HttpGet("{id}")]
        public ActionResult<CatalogDto> Get([FromRoute] int id)
        {
            var result = catalogService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/catalog
        [HttpPost]
        public IActionResult Create([FromBody] CatalogDto catalog)
        {
            if (catalogService.Create(catalog))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // PUT: api/catalog/5
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] CatalogDto catalog)
        {
            catalog.Id = id;

            if (catalogService.Update(catalog))
            {
                return NoContent();
            }

            return BadRequest();
        }

        // DELETE: api/catalog/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            UserService userService = new UserService();

            var user = userService.GetById(Int32.Parse(HttpContext.User.Claims.First().Value));

            if (user.isAdmin)
            {
                if (catalogService.Delete(id))
                {
                    return Ok();
                }
            }
            else
            {
                if (catalogService.FakeDelete(id))
                {
                    return Ok();
                }
            }

            return BadRequest();
        }
    }
}
