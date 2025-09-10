using System.Collections.Generic;
using System.Text.Json;
using Asp.Versioning;
using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Service.V1;
using Back_Almazara.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using static Back_Almazara.Utility.PermisionUtility;

namespace Back_Almazara.Controllers.V1
{
    [ApiVersion(1.0)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _service;
        private readonly IRedisCacheService _cache;

        public CommentsController(ICommentsService service, IRedisCacheService cache) {
            _service = service;
            _cache = cache;
        }

        /// <summary>
        /// Controller that gets all the comments of a notice
        /// </summary>
        /// <returns>List of comments</returns>
        [ApiVersion(1.0)]
        [HttpGet("Index")]
        [Autorize(-1)]
        public async Task<ActionResult> Index(string notice_id_i)
        {

            string cacheKey = $"comments:notice:{notice_id_i}";

            var cachedComments = await _cache.GetAsync<List<CommentDTO>>(cacheKey);
            if (cachedComments != null)
            {
                return Ok(cachedComments);
            }

            var response = _service.Index(notice_id_i);
            if (response.Success)
            {
                await _cache.SetWithTagsAsync(
                   cacheKey,
                   response.Data,
                   new[] { "comments", $"notice:{notice_id_i}" }
               );
                return Ok(response.Data);
            }

            return NotFound("No se han encontrado noticias.");
        }


    
        /// <summary>
        /// Controller that generates new comments
        /// </summary>
        /// <param name="notice">Comment information</param>
        /// <returns>Creates a comment</returns>
        [HttpPost("Create")]
        [Autorize(4)]
        public async Task<ActionResult> Create(CommentViewModel comment)
        {
            var response = _service.Create(comment);
            if (response.Success)
            {
                await _cache.InvalidateByTagAsync("comments");
                await _cache.InvalidateByTagAsync($"notice:{comment.NoticeIdI}");

                return Ok(response.Message);
            }

            return Conflict(response.Message);
        }

        /// <summary>
        /// Controller that edits a comment
        /// </summary>
        /// <param name="notice">Notice detail with all the new info</param>
        /// <returns>Edits a real notice</returns>
        [HttpPut("Edit")]
        [Autorize(4)]
        public async Task<ActionResult> Edit(CommentViewModel comment)
        {
            var response = _service.Edit(comment);
            if (response.Success)
            {
                await _cache.InvalidateByTagAsync("comments");
                await _cache.InvalidateByTagAsync($"notice:{comment.NoticeIdI}");

                return Ok(response.Message);
            }

            return Conflict(response.Message);
        }

        /// <summary>
        /// Controller that deletes a comment
        /// </summary>
        /// <param name="comment_id_i">Comment identificator</param>
        /// <returns>True or False if its done</returns>
        [HttpPost("Delete")]
        [Autorize(4)]
        public async Task<ActionResult> Delete(string comment_id_i)
        {
            var response = _service.Delete(comment_id_i);
            if (response.Success && response.Data?.NoticeIdI != null)
            {
                await _cache.InvalidateByTagAsync("comments");
                await _cache.InvalidateByTagAsync($"notice:{response.Data.NoticeIdI}");

                return Ok(response?.Message);
            }

            return NotFound(response.Message);
        }


    }
}
