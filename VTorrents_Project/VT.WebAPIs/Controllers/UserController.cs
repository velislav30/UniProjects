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
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController()
        {
            this.userService = new UserService();
        }

        /*[HttpPost]
        public IActionResult Login([FromBody] UserDto user)
        {
            UserDto resultUser = AuthenticateUser(user);

            if (resultUser != null)
            {
                return Ok();
            }

            return Unauthorized();
        }

        private UserDto AuthenticateUser(UserDto user)
        {
            return userService.GetUserByLoginCredentials(user.Username,user.Password);
        }*/

        // GET: api/User
        [HttpGet]
        public IEnumerable<UserDto> GetAll()
        {
            return userService.GetAll();

        }

        [HttpGet("username={username}")]
        public IEnumerable<UserDto> GetAllWithUsername([FromRoute] String username)
        {
            if (username.IsNullOrEmpty())
            {
                return userService.GetAll();
            }
            return userService.GetAllWithUsername(username);
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public ActionResult<UserDto> Get([FromRoute] int id)
        {
            var result = userService.GetById(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // POST: api/User
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create([FromBody] UserDto user)
        {
            if (userService.Create(user))
            {
                return Ok();
            }

            return BadRequest();
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UserDto user)
        {
            user.Id = id;

            if (userService.Update(user))
            {
                return Ok();
            }

            return BadRequest();
        }

        // DELETE: api/User/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var user = userService.GetById(Int32.Parse(HttpContext.User.Claims.First().Value));

            if (user.isAdmin)
            {
                if (userService.Delete(id))
                {
                    return Ok();
                }
            }
            else
            {
                if (userService.FakeDelete(id))
                {
                    return Ok();
                }
            }

            return BadRequest();
        }
    }
}
