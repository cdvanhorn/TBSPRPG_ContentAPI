using System;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

using ContentApi.Services;
using ContentApi.ViewModels;

namespace ContentApi.Controllers {
    [ApiController, Route("/api/[controller]/source")]
    public class ContentController : ControllerBase {
        private readonly ISourceService _sourceService;

        public ContentController(ISourceService sourceService) {
            _sourceService = sourceService;
        }

        [Authorize, HttpGet("{language}/{sourceKey:guid}")]
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
    }
}