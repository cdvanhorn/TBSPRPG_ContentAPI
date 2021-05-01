using System;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ContentApi.Services;
using ContentApi.ViewModels;

namespace ContentApi.Controllers {
    [ApiController, Route("/api/[controller]")]
    public class ContentController : ControllerBase {
        private readonly IContentService _contentService;

        public ContentController(IContentService contentService) {
            _contentService = contentService;
        }

        [Authorize, HttpGet("{gameId:guid}")]
        public async Task<IActionResult> GetForGame(Guid gameId) {
            var content = await _contentService.GetAllContentForGame(gameId);
            return Ok(content);
        }

        [Authorize, Route("latest/{gameId:guid}")]
        public async Task<IActionResult> GetLatestForGame(Guid gameId) {
            var content = await _contentService.GetLatestForGame(gameId);
            return Ok(content);
        }

        [Authorize, HttpGet("filter/{gameId:guid}")]
        public async Task<IActionResult> FilterContent(Guid gameId, [FromQuery] ContentFilterRequest filterRequest) {
            var content = await _contentService.GetPartialContentForGame(gameId, filterRequest);
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