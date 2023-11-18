using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StudentInformationSystem.Application.DTOs;
using StudentInformationSystem.Application.Models.RequestModels;
using StudentInformationSystem.Application.Models.ResponseModels;
using StudentInformationSystem.Application.Services.Interfaces;
using StudentInformationSystem.Core.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    [Authorize(Roles ="Admin")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestModel model)
    {
        try
        {
            // TODO : jwt token içeriden devam et. 
            var registerUserResponseDto = await _authService.RegisterUserAsync(model);
            if (registerUserResponseDto.ResultStatus == ResultStatus.Success)
            {
                var result = await _authService.GenerateJwtTokenAsync(registerUserResponseDto.Data);
                return Ok(result.Data);
            }
            else
                return BadRequest(new { Message = registerUserResponseDto.Message});
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = "Ekleme başarısız." });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        var userValidData = await _authService.ValidateUserAsync(model.Email, model.Password);

        if (userValidData.ResultStatus == ResultStatus.Error)
        {
            return BadRequest(new { Message = "Kullanıcı adı veya şifre hatalı." });
        }

        var result = await _authService.GenerateJwtTokenAsync(userValidData.Data);

        var responseData = new LoginResponseDto
        {
            Token = result.Data,
            UserId = userValidData.Data.Id,
            RoleName = userValidData.Data.UserRole.RoleName
        };

        return Ok(responseData);
    }

    // TODO: İşin bitince kapat.
    public static JwtSecurityToken ParseToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

        return jsonToken;
    }

}
