using HotelLisstingApi.Core.Dtos.User;
using HotelLisstingApi.Core.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HotelHostingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0", Deprecated = true)]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public IdentityController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        //api/Identity/register
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authManager.RegisterAsync(apiUserDto);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        // POST: api/Identity/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        public async Task<ActionResult> RefreshToken([FromBody] LoginUserDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authManager.GetTokenAsync(request);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost("addrole")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authManager.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }
    }
}
