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
        private readonly IGameService _gameService;

        public ContentController(IContentService contentService, ISourceService sourceService, IGameService gameService) {
            _contentService = contentService;
            _sourceService = sourceService;
            _gameService = gameService;
        }

        [Authorize, HttpGet("{gameId:guid}")]
        public async Task<IActionResult> GetForGame(Guid gameId)
        {
            var content = await _contentService.GetAllContentForGame(gameId);
            return Ok(new ContentViewModel(content));
        }
        
        [Authorize, HttpGet("source/{language}/{sourceKey:guid}")]
        public async Task<IActionResult> GetSourceContent(string language, Guid sourceKey)
        {
            try
            {
                var source = await _sourceService.GetSourceForKey(sourceKey, null, language);
                return Ok(new SourceViewModel()
                {
                    Id = sourceKey,
                    Language = language,
                    Source = source
                });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        
        [Authorize, HttpGet("source/{gameId:guid}/{sourceKey:guid}")]
        public async Task<IActionResult> GetSourceContent(Guid gameId, Guid sourceKey)
        {
            var game = await _gameService.GetGameById(gameId);
            if(game == null)
                return BadRequest(new { message = $"invalid game id {gameId}" });
            
            var source = await _sourceService.GetSourceForKey(sourceKey, null, game.Language);
            return Ok(new SourceViewModel()
            {
                Id = sourceKey,
                Language = game.Language,
                Source = source
            });
        }

        [Authorize, Route("latest/{gameId:guid}")]
        public async Task<IActionResult> GetLatestForGame(Guid gameId) {
            var content = await _contentService.GetLatestForGame(gameId);
            return Ok(new ContentViewModel(content));
        }

        [Authorize, HttpGet("filter/{gameId:guid}")]
        public async Task<IActionResult> FilterContent(Guid gameId, [FromQuery] ContentFilterRequest filterRequest) {
            try
            {
                var content = await _contentService.GetPartialContentForGame(gameId, filterRequest);
                return Ok(new ContentViewModel(content));
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "invalid filter arguments" });
            }
        }
    }
}