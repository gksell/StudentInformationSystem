using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly IMapper _mapper;

    public AuthController(IUserService userService, IUserRoleService userRoleService, IMapper mapper)
    {
        _userService = userService;
        _userRoleService = userRoleService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
    {
        try
        {
            var userRegisterDto = _mapper.Map<RegisterUserDto>(model);
            var user = await _userService.RegisterUserAsync(userRegisterDto);
            if (user != null)
            {
                var jwtToken = await _userService.GenerateJwtTokenAsync(user);
                return Ok(new { Token = jwtToken });
            }
            else
                return BadRequest(new { Message = "Kullanıcı zaten ekli" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Ekleme başarısız." });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Hatalı model bilgisi.");
        }

        var user = await _userService.ValidateUserAsync(model.Email, model.Password);

        if (user == null)
        {
            return BadRequest("Email veya Şifre hatalı.");
        }

        var token = _userService.GenerateJwtTokenAsync(user);

        return Ok(new { Token = token });
    }

    // TODO: İşin bitince kapat.
    public static JwtSecurityToken ParseToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        return jsonToken;
    }

}
