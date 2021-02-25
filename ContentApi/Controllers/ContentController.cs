using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ContentApi.Services;
using ContentApi.ViewModels;

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

        [Authorize]
        [Route("latest/{gameid}")]
        public async Task<IActionResult> GetLatestForGame(string gameid) {
            var content = await _contentService.GetLatestForGame(gameid);
            return Ok(content);
        }

        [Authorize]
        [Route("filter/{gameid}")]
        public async Task<IActionResult> FilterContent(string gameid, ContentFilterRequest filterRequest) {
            var content = await _contentService.GetPartialContentForGame(gameid, filterRequest);
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