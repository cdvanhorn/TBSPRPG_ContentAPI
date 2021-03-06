using TbspRpgLib.Services;
using TbspRpgLib.Aggregates;

using ContentApi.Repositories;
using ContentApi.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContentApi.Entities;

namespace ContentApi.Services {
    public interface IContentService : IServiceTrackingService {
        Task<List<Content>> GetAllContentForGame(Guid gameId);
        Task<Content> GetLatestForGame(Guid gameId);
        Task<List<Content>> GetContentForGameAfterPosition(Guid gameId, ulong position);
        Task<List<Content>> GetPartialContentForGame(Guid gameId, ContentFilterRequest filterRequest);
        Task AddContent(Content content);
    }

    public class ContentService : ServiceTrackingService, IContentService {
        private readonly IContentRepository _repository;

        public ContentService(IContentRepository repository) : base(repository) {
            _repository = repository;
        }

        public async Task<List<Content>> GetAllContentForGame(Guid gameId)
        {
            var contents = await _repository.GetContentForGame(gameId);
            return contents;
        }

        public async Task<Content> GetLatestForGame(Guid gameId)
        {
            var contents = await _repository.GetContentForGameReverse(gameId, null, 1);
            return contents.FirstOrDefault();
        }
        
        public async Task<List<Content>> GetContentForGameAfterPosition(Guid gameId, ulong position)
        {
            return await _repository.GetContentForGameAfterPosition(gameId, position);
        }

        public async Task<List<Content>> GetPartialContentForGame(Guid gameId, ContentFilterRequest filterRequest)
        {
            List<Content> contents = null;
            if (string.IsNullOrEmpty(filterRequest.Direction) || filterRequest.IsForward())
            {
                contents = await _repository.GetContentForGame(
                    gameId,
                    (int?) filterRequest.Start,
                    (int?) filterRequest.Count);
            } 
            else if (filterRequest.IsBackward())
            {
                contents = await _repository.GetContentForGameReverse(
                    gameId,
                    (int?) filterRequest.Start,
                    (int?) filterRequest.Count);
            }
            else
            {
                //we can't parse the direction
                throw new ArgumentException($"invalid direction argument {filterRequest.Direction}");
            }

            return contents;
        }

        public async Task AddContent(Content content)
        {
            //check if we already have this content determined by game id and position
            var dbContent = await _repository.GetContentForGameWithPosition(content.GameId, content.Position);
            if (dbContent == null)
            {
                _repository.AddContent(content);
            }
        }
    }
}
