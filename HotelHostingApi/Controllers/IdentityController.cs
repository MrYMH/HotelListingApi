using HotelLisstingApi.Core.Dtos.User;
using HotelLisstingApi.Core.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelHostingApi.Controllers
{
    [Route("api/[controller]")]
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

        //// POST: api/Identity/login
        //[HttpPost]
        //[Route("login")]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult> Login([FromBody] LoginUserDto loginDto)
        //{
        //    var authResponse = await _authManager.Login(loginDto);

        //    if (authResponse == null)
        //    {
        //        return Unauthorized();
        //    }

        //    return Ok(authResponse);
        //}

        // POST: api/Identity/refreshtoken
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
