using AutoMapper;
using HotelLisstingApi.Core.Dtos.User;
using HotelLisstingApi.Core.IRepositories;
using HotelLisstingApi.Core.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelListingApi.EF.Repositories
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private ApiUser _user;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");
            }

            return result.Errors;
        }
    }
}
