using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpinionAnalytics.Api.Data.Interface;
using OpinionAnalytics.Application.Repositories;

namespace OpinionAnalytics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentApiController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;

        public CommentApiController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("GetAllComments")]
        public async Task<IActionResult> GetCommentsAsync()
        {
            var comments = await _commentRepository.GetAllAsync();
            return Ok(comments);
        }
    }
}
