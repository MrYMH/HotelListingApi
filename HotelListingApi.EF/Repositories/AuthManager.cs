using AutoMapper;
using HotelHostingApi.EF.Configuration;
using HotelLisstingApi.Core.Dtos.User;
using HotelLisstingApi.Core.IRepositories;
using HotelLisstingApi.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelListingApi.EF.Repositories
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        //private ApiUser _user;
        private readonly IConfiguration _config;
       

        private readonly RoleManager<IdentityRole> _roleManager;

        private const string _loginProvider = "HotelListingApi";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager , IConfiguration config , RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _config = config;
            
            _roleManager= roleManager;
        }

        //public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
        //{
        //    _user = _mapper.Map<ApiUser>(userDto);
        //    _user.UserName = userDto.Email;

        //    var result = await _userManager.CreateAsync(_user, userDto.Password);

        //    if (result.Succeeded)
        //    {
        //        await _userManager.AddToRoleAsync(_user, "User");
        //    }

        //    return result.Errors;
        //}

        //public async Task<AuthResponseDto> Login(LoginUserDto loginDto)
        //{
        //    _user = await _userManager.FindByEmailAsync(loginDto.Email);
        //    bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);

        //    if (_user == null || isValidUser == false)
        //    {
        //        return null;
        //    }

        //    var token = await GenerateToken();
        //    return new AuthResponseDto
        //    {
        //        Token = token,
        //        UserId = _user.Id
        //    };

        //}

        //private async Task<string> GenerateToken()
        //{
        //    var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));

        //    var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

        //    var roles = await _userManager.GetRolesAsync(_user);
        //    var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
        //    var userClaims = await _userManager.GetClaimsAsync(_user);

        //    var claims = new List<Claim>
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Email, _user.Email),
        //        new Claim("uid", _user.Id),
        //    }
        //    .Union(userClaims).Union(roleClaims);

        //    var token = new JwtSecurityToken(
        //        issuer: _config["JwtSettings:Issuer"],
        //        audience: _config["JwtSettings:Audience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddMinutes(Convert.ToInt32(_config["JwtSettings:DurationInMinutes"])),
        //        signingCredentials: credentials
        //        );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

        //public async Task<string> CreateRefreshTocken()
        //{
        //    await _userManager.RemoveAuthenticationTokenAsync(_user, _loginProvider, _refreshToken);
        //    var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);
        //    var result = await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);
        //    return newRefreshToken;
        //}

        //public async Task<AuthResponseDto> VerifyRefreshTocken(AuthResponseDto request)
        //{
        //    var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        //    var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
        //    var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == JwtRegisteredClaimNames.Email)?.Value;
        //    _user = await _userManager.FindByNameAsync(username);

        //    if (_user == null || _user.Id != request.UserId)
        //    {
        //        return null;
        //    }

        //    var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user,
        //        _loginProvider, _refreshToken, request.RefreshToken);

        //    if (isValidRefreshToken)
        //    {
        //        var token = await GenerateToken();
        //        return new AuthResponseDto
        //        {
        //            Token = token,
        //            UserId = _user.Id,
        //            RefreshToken = await CreateRefreshTocken()
        //        };
        //    }

        //    await _userManager.UpdateSecurityStampAsync(_user);
        //    return null;
        //}


        //new implement

        public async Task<AuthModel> RegisterAsync(ApiUserDto userDto)
        {
            if(await _userManager.FindByEmailAsync(userDto.Email) is not null)
            {
                return new AuthModel { Message = "Email is already registered!" };
            }

            if (await _userManager.FindByNameAsync(userDto.Username) is not null)
            {
                return new AuthModel { Message = "Username is already registered!" };
            }
            var user = new ApiUser
            {
                UserName = userDto.Username,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName
            };

        }

        public async Task<AuthModel> GetTokenAsync(LoginUserDto model)
        {
            var authModel = new AuthModel();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }


        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Sonething went wrong";
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApiUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();


            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);


            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(Convert.ToInt32(_config["JWT:DurationInDays"])),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;

        }
    }
}
