using Asp.Versioning;
using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Service.V1;
using Back_Almazara.ViewModel;
using Microsoft.AspNetCore.Mvc;
using static Back_Almazara.Utility.PermisionUtility;

namespace Back_Almazara.Controllers.V1
{
    [ApiVersion(1.0)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NoticesController : ControllerBase
    {
        private readonly INoticeService _service;
        private readonly IRedisCacheService _cache;

        public NoticesController(INoticeService service, IRedisCacheService cache) {
            _service = service;
            _cache = cache;
        }

        /// <summary>
        /// Controller that gets all the notices for the list page
        /// </summary>
        /// <returns>List of notices without detail</returns>
        [ApiVersion(1.0)]
        [HttpGet("Index")]
        [Autorize(-1)]
        public async Task<ActionResult> Index(string user_id_i)
        {
            var cacheKey = $"notices:user:{user_id_i}";

            // Intentar obtener del cache
            var cachedNotices = await _cache.GetAsync<List<NoticeDTO>>(cacheKey);
            if (cachedNotices != null)
            {
                return Ok(cachedNotices);
            }

            // Si no está en cache, obtener de la base de datos
            var response = _service.Index(user_id_i);
            if (response.Success)
            {
                // Guardar en cache con tags
                await _cache.SetWithTagsAsync(
                    cacheKey,
                    response.Data,
                    new[] { "notices", $"user:{user_id_i}" }
                );

                return Ok(response.Data);
            }

            return NotFound("No se han encontrado noticias.");
        }

        /// <summary>
        /// Controller that get a notice with all the content.
        /// </summary>
        /// <param name="notice_id_i">Identificator of the notice</param>
        /// <returns>Notice with all the information and content</returns>
        [HttpGet("Details")]
        [Autorize(-1)]
        public async Task<ActionResult> Details(string notice_id_i )
        {
            var cacheKey = $"notices:detail:{notice_id_i}";

            var cachedNotice = await _cache.GetAsync<NoticeDetailDTO>(cacheKey);
            if (cachedNotice != null)
            {
                return Ok(cachedNotice);
            }

            var response = _service.Detail(notice_id_i);
            if (response.Success)
            {
                await _cache.SetWithTagsAsync(
                    cacheKey,
                    response.Data,
                    new[] { "notices", "notice_details", $"notice:{notice_id_i}" },
                    30 // 30 minutos para detalles (más tiempo que listados)
                );
                return Ok(response.Data);
            }

            return NotFound(response.Message);
        }

        /// <summary>
        /// Controller that gets all the notices of a determinated user's favorite list
        /// </summary>
        /// <param name="user_id_i">Identificador of the user </param>
        /// <returns>Favorite notices for the user_id</returns>
        [ApiVersion(1.0)]
        [HttpGet("IndexFavorites")]
        [Autorize(4)]
        public async Task<ActionResult> IndexFavorites(string user_id_i)
        {
            string cacheKey = $"notices:favorites:user:{user_id_i}";

            var cachedFavorites = await _cache.GetAsync<List<NoticeDTO>>(cacheKey);
            if (cachedFavorites != null)
            {
                return Ok(cachedFavorites);
            }

            var response = _service.IndexFavorites(user_id_i);
            if (response.Success)
            {
                await _cache.SetWithTagsAsync(
                  cacheKey,
                  response.Data,
                  new[] { "notices", "favorites", $"user:{user_id_i}" }
              );
                return Ok(response.Data);
            }

            return NotFound("No se han encontrado noticias.");
        }

        /// <summary>
        /// Controller that generates simple notices
        /// </summary>
        /// <param name="notice">Notice information</param>
        /// <returns>Creates a notice</returns>
        [HttpPost("Create")]
        [Autorize(2)]
        public async Task<ActionResult> Create(NoticeCreateDTO notice)
        {
            var response = _service.Create(notice);
            if (response.Success)
            {
                await _cache.InvalidateByTagAsync("notices");

                return Ok(response.Message);
            }

            return Conflict(response.Message);
        }

        /// <summary>
        /// Controller that edits a notice
        /// </summary>
        /// <param name="notice">Notice detail with all the new info</param>
        /// <returns>Edits a real notice</returns>
        [HttpPut("Edit")]
        [Autorize(3)]
        public async Task<ActionResult> Edit(NoticeDetailViewModel notice)
        {
            var response = _service.Edit(notice);
            if (response.Success)
            {
                await _cache.InvalidateByTagAsync("notices");
                await _cache.InvalidateByTagAsync("notice_details");
                await _cache.InvalidateByTagAsync($"notice:{notice.IdI}");
                return Ok(response.Message);
            }

            return Conflict(response.Message);
        }

        /// <summary>
        /// Controller that deletes a notice
        /// </summary>
        /// <param name="notice_id_i">Notice identificator</param>
        /// <returns>True or False if its done</returns>
        [HttpPost("Delete")]
        [Autorize(2)]
        public async Task<ActionResult> Delete(string notice_id_i)
        {
            var response = _service.Delete(notice_id_i);
            if (response.Success)
            {
                await _cache.InvalidateByTagAsync("notices");
                await _cache.InvalidateByTagAsync("notice_details");
                await _cache.InvalidateByTagAsync($"notice:{notice_id_i}");

                return Ok(response.Message);
            }

            return NotFound(response.Message);
        }

        /// <summary>
        /// Controller that deletes a notice
        /// </summary>
        /// <param name="notice_id_i">Notice identificator</param>
        /// <returns>True or False if its done</returns>
        [HttpPost("Favorite")]
        [Autorize(4)]
        public async Task<ActionResult> FavoriteNotice(FavoriteViewModel favoriteInfo)
        {
            var response = _service.FavoriteNotice(favoriteInfo);
            if (response.Success)
            {
                await _cache.InvalidateByTagAsync("favorites");
                await _cache.InvalidateByTagAsync($"user:{favoriteInfo.user_id_i}");

                return Ok("Se ha modificado el favorito");
            }

            return Conflict("No se ha podido modificar el favorito.");
        }

    }
}
