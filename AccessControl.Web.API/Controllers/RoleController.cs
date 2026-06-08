using AccessControl.Web.API.Models;
using AccessControl.Web.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleService roleService, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            try
            {
                var roles = await _roleService.GetAllRolesAsync();
                _logger.LogInformation("Retrived {count} roles", roles);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");

            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetRolesById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid user Id");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server Error{ex.Message}");
            }
            var roles = await _roleService.GetRolesByIdAsync(id);
            _logger.LogInformation("Retrived role with id {Roleid}", id);
            if (roles == null)
            {
                return NotFound();
            }
            return Ok(roles);
        }
        [HttpPost]

        public async Task<IActionResult> CreateRoles(Roles roles)
        {
            try
            {
                if (roles == null)
                {
                    return BadRequest("Role object is null.");
                }
                var createdroles = await _roleService.CreateRolesAsync(roles);
                _logger.LogInformation("Created role with id {RoleId}", createdroles.RoleId);
                return Ok(createdroles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateRoles(int id, Roles roles)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid UserId");
                }
                if (roles == null)
                {
                    return BadRequest("Roles data is null");
                }
                if (string.IsNullOrEmpty(roles.RoleName))
                {
                    return BadRequest("RoleNamesare required fields");
                }
                var updateRoles = await _roleService.UpdateRolesAsync(id, roles);
                _logger.LogInformation("Updated role with id {RoleId}", id);

                if (updateRoles == null)
                {
                    return NotFound();
                }
                return Ok(updateRoles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"interna server erroe:{ex.Message}");
            }
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteRoles(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid user Id");
                }
                var isDeleted = await _roleService.DeleteRolesAsync(id);
                _logger.LogInformation("Role with id {RoleId} deleted successfully", id);
                if (!isDeleted)
                {
                    return NotFound();
                }
                return Ok($"Role with id {id} deleted successfully.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
