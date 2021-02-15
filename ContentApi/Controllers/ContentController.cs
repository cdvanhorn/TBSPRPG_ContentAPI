using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ContentApi.Services;

namespace ContentApi.Controllers {

    [ApiController]
    [Route("/api/[controller]")]
    public class ContentController : ControllerBase {
        IContentService _contentService;

        public ContentController(IContentService contentService) {
            _contentService = contentService;
        }

        [Authorize]
        [HttpGet("{gameid}")]
        public async Task<IActionResult> GetForGame(string gameid) {
            var content = await _contentService.GetAllContentForGame(gameid);
            return Ok(content);
        }

        // [Authorize]
        // [Route("initiallocation/{id}")]
        // public async Task<IActionResult> GetInitialLocation(string id) {
        //     var loc = await _locationService.GetInitialForLocation(id);
        //     if(loc == null)
        //         return BadRequest(new { message = "invalid game id" });
        //     return Ok(loc);
        // }
    }
}