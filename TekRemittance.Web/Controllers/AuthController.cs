using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;
using TekRemittance.Web.Models.dto;
using TekRemittance.Repository.Interfaces;
using TekRemittance.Repository.Entities;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;

namespace TekRemittance.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly ITokenRevocationRepository _revocationRepo;
        private readonly IAuditLogService _audit;
        private readonly IPermissionHelperService _permissionHelper;

        public AuthController(IUserService userService, IConfiguration config, ITokenRevocationRepository revocationRepo, IAuditLogService audit, IPermissionHelperService permissionHelper)
        {
            _userService = userService;
            _config = config;
            _revocationRepo = revocationRepo;
            _audit = audit;
            _permissionHelper = permissionHelper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] loginDTO login)
        {
            try
            {
                var user = await _userService.ValidateCredentialsAsync(login.LoginName, login.Password);
                if (user == null)
                {
                    return Unauthorized(ApiResponse<string>.Error("Invalid credentials", 201));
                }
                if (!user.IsSupervise)
                {
                    return Unauthorized(ApiResponse<string>.Error("You are not authorized to log in. Please wait for supervisor approval.", 201));
                }

                if (user.IsActive == false)
                {
                    return Unauthorized(ApiResponse<string>.Error("You are not authorized to log in. Your Status Is InActive.", 201));
                }

                var token = await GenerateJwtTokenAsync(user);
                try
                {
                    await _audit.AddAsync(new AuditLog
                    {
                        Id = Guid.NewGuid(),
                        EntityName = "Auth",
                        EntityId = user.Id,
                        Action = "Login",
                        OldValues = "{}",
                        NewValues = JsonSerializer.Serialize(new { user = user.LoginName, expiresUtc = token.Expires }),
                        PerformedBy = user.LoginName ?? user.Name ?? string.Empty,
                        PerformedOn = DateTime.UtcNow
                    });
                }
                catch { /* best-effort audit, do not block login */ }
                return Ok(ApiResponse<object>.Success(new {
                    userid=user.Id,
                    name = user.Name,
                    token = token.Token,
                    expiresUtc = token.Expires,
                    expiresLocal = token.Expires.ToLocalTime()
                }, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var jti = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                var expStr = User.FindFirst(JwtRegisteredClaimNames.Exp)?.Value;
                var sub = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(jti) || string.IsNullOrEmpty(expStr))
                {
                    return Unauthorized(ApiResponse<string>.Error("Invalid token", 401));
                }
                // exp is seconds since epoch
                if (!long.TryParse(expStr, out var expSeconds))
                {
                    return Unauthorized(ApiResponse<string>.Error("Invalid token exp", 401));
                }
                var expiresAt = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                await _revocationRepo.RevokeAsync(jti, expiresAt);
                try
                {
                    Guid.TryParse(sub, out var userId);
                    await _audit.AddAsync(new AuditLog
                    {
                        Id = Guid.NewGuid(),
                        EntityName = "Auth",
                        EntityId = userId,
                        Action = "Logout",
                        OldValues = JsonSerializer.Serialize(new { jti }),
                        NewValues = "{}",
                        PerformedBy = User.FindFirst("unique_name")?.Value
                                        ?? User.FindFirst("name")?.Value
                                        ?? string.Empty,
                        PerformedOn = DateTime.UtcNow
                    });
                }
                catch { /* best-effort audit, do not block logout */ }
                return Ok(ApiResponse<string>.Success("Logged out", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }

        private async Task<(string Token, DateTime Expires)> GenerateJwtTokenAsync(userDTO user)
        {
            var key = _config["Jwt:Key"] ?? string.Empty;
            var issuer = _config["Jwt:Issuer"] ?? string.Empty;
            var audience = _config["Jwt:Audience"] ?? string.Empty;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var jti = Guid.NewGuid().ToString();
            
            // Get user permissions
            var userPermissions = await _permissionHelper.GetUserPermissionsAsync(user.Id);
            
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.LoginName ?? string.Empty),
                new Claim("name", user.Name ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
            };
            
            // Add permissions as claims
            foreach (var permission in userPermissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            var hours = _config.GetValue<int>("Jwt:ExpiresHours", 8);
            var expires = DateTime.UtcNow.AddHours(hours);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenString, expires);
        }

        [HttpGet("user-modules")]
        public async Task<IActionResult> GetUserModules()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty);
                var userPermissions = await _permissionHelper.GetUserPermissionsAsync(userId);
                
                var modules = new Dictionary<string, List<string>>();
                
                foreach (var permission in userPermissions)
                {
                    var parts = permission.Split('.');
                    if (parts.Length >= 2)
                    {
                        var module = parts[0];
                        var screen = parts.Length > 2 ? parts[1] : "General";
                        
                        if (!modules.ContainsKey(module))
                            modules[module] = new List<string>();
                            
                        if (!modules[module].Contains(screen))
                            modules[module].Add(screen);
                    }
                }
                
                return Ok(ApiResponse<object>.Success(modules, 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
