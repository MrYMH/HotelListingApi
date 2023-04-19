using HotelLisstingApi.Core.Dtos.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelLisstingApi.Core.IRepositories
{
    public interface IAuthManager
    {
        Task<AuthModel> RegisterAsync(ApiUserDto userDto);
        Task<AuthModel> GetTokenAsync(LoginUserDto model);
        Task<string> AddRoleAsync(AddRoleModel model);
    }
}
