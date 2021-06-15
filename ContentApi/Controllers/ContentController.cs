using System;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ContentApi.Services;
using ContentApi.ViewModels;

namespace ContentApi.Controllers {
    [ApiController, Route("/api/[controller]")]
    public class ContentController : ControllerBase {
        private readonly IContentService _contentService;
        private readonly ISourceService _sourceService;

        public ContentController(IContentService contentService, ISourceService sourceService) {
            _contentService = contentService;
            _sourceService = sourceService;
        }

        [Authorize, HttpGet("{gameId:guid}")]
        public async Task<IActionResult> GetForGame(Guid gameId)
        {
            var content = await _contentService.GetAllContentForGame(gameId);
            return Ok(content);
        }
        
        [Authorize, HttpGet("{language:string}/{sourceKey:guid}")]
        public async Task<IActionResult> GetSourceContent(string language, Guid sourceKey)
        {
            var source = await _sourceService.GetSourceForKey(sourceKey, null, language);
            return Ok(new SourceViewModel()
            {
                Id = sourceKey,
                Language = language,
                Source = source
            });
        }

        [Authorize, Route("latest/{gameId:guid}")]
        public async Task<IActionResult> GetLatestForGame(Guid gameId) {
            var content = await _contentService.GetLatestForGame(gameId);
            return Ok(content);
        }

        [Authorize, HttpGet("filter/{gameId:guid}")]
        public async Task<IActionResult> FilterContent(Guid gameId, [FromQuery] ContentFilterRequest filterRequest) {
            try
            {
                var content = await _contentService.GetPartialContentForGame(gameId, filterRequest);
                return Ok(content);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "invalid filter arguments" });
            }
        }
    }
}