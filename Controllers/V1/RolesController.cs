using Asp.Versioning;
using Back_Almazara.DTOS;
using Back_Almazara.Service.V1;
using Back_Almazara.ViewModel;
using Microsoft.AspNetCore.Mvc;
using static Back_Almazara.Utility.PermisionUtility;

namespace Back_Almazara.Controllers.V1
{
    [ApiVersion(1.0)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _service;
        private readonly IRedisCacheService _cache;

        public RolesController(IRolesService service, IRedisCacheService cache)
        {
            _service = service;
            _cache = cache;
        }

        /// <summary>
        /// Controller that gets all the users with his roles
        /// </summary>
        /// <returns>List of notices without detail</returns>
        [ApiVersion(1.0)]
        [HttpGet("Index")]
        [Autorize(1)]
        public async Task<ActionResult> Index()
        {
            var cacheKey = "roles:users";
            var cached = await _cache.GetAsync<List<RoleResultDTO>>(cacheKey);
            if (cached != null)
            {
                return Ok(cached);
            }

            var response = _service.Index();
            if (response.Success)
            {
                await _cache.SetWithTagsAsync(
                    cacheKey,
                    response.Data,
                    new[] { "roles" }
                );
                return Ok(response.Data);
            }

            return NotFound("No se han encontrado noticias.");
        }

        /// <summary>
        /// Controller that gets the list of roles types
        /// </summary>
        /// <returns>List of distinct roles</returns>
        [HttpGet("ListRoles")]
        [Autorize(1)]
        public async Task<ActionResult> ListRoles()
        {
            var cacheKey = "roles:list";
            var cached = await _cache.GetAsync<List<RoleDTO>>(cacheKey);
            if (cached != null)
            {
                return Ok(cached);
            }

            var response = _service.ListRoles();
            if (response.Success)
            {
                await _cache.SetWithTagsAsync(
                    cacheKey,
                    response.Data,
                    new[] { "roles" },
                    60 
                );
                return Ok(response.Data);
            }

            return Conflict("No se ha podido obtener los roles.");
        }

        /// <summary>
        /// Controller that edits a notice
        /// </summary>
        /// <param name="notice">Notice detail with all the new info</param>
        /// <returns>Edits a real notice</returns>
        [HttpPost("Modify")]
        [Autorize(3)]
        public async Task<ActionResult> Modify(string user_id_i, string role_id_i)
        {
            var response = _service.Modify(user_id_i, role_id_i);
            if (response.Success)
            {
                await _cache.InvalidateByTagAsync("roles");
                return Ok(response.Message);
            }

            return Conflict(response.Message);
        }



    }
}
