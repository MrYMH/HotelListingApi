﻿using HotelLisstingApi.Core.Dtos.User;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            var errors = await _authManager.Register(apiUserDto);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok();
        }


    }
}
