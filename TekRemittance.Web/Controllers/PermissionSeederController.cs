using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using TekRemittance.Service.Interfaces;
using TekRemittance.Web.Models;
using System;
using System.Threading.Tasks;

namespace TekRemittance.Web.Controllers
{
    [EnableCors("AllowFrontend")]
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionSeederController : ControllerBase
    {
        private readonly IPermissionHelperService _permissionHelper;

        public PermissionSeederController(IPermissionHelperService permissionHelper)
        {
            _permissionHelper = permissionHelper;
        }

        [AllowAnonymous]
        [HttpPost("seed-permissions")]
        public async Task<IActionResult> SeedPermissions()
        {
            try
            {
                await _permissionHelper.SeedDefaultPermissionsAsync();
                return Ok(ApiResponse<string>.Success("Permissions seeded successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.Error(ex.Message));
            }
        }
    }
}
