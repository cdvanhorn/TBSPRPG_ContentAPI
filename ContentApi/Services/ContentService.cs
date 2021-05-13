using TbspRpgLib.Services;
using TbspRpgLib.Aggregates;

using ContentApi.Repositories;
using ContentApi.ViewModels;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContentApi.Services {
    public interface IContentService : IServiceTrackingService {
        Task<ContentViewModel> GetAllContentForGame(Guid gameId);
        Task<ContentViewModel> GetLatestForGame(Guid gameId);
        Task<ContentViewModel> GetPartialContentForGame(Guid gameId, ContentFilterRequest filterRequest);
    }

    public class ContentService : ServiceTrackingService, IContentService {
        private readonly IContentRepository _repository;

        public ContentService(IContentRepository repository) : base(repository) {
            _repository = repository;
        }

        public Task<ContentViewModel> GetAllContentForGame(Guid gameId)
        {
            return GetAllContentForGame(gameId.ToString());
        }

        private async Task<ContentViewModel> GetAllContentForGame(string gameId)
        {
            throw new NotImplementedException();
        }

        public Task<ContentViewModel> GetLatestForGame(Guid gameId)
        {
            return GetLatestForGame(gameId.ToString());
        }

        private async Task<ContentViewModel> GetLatestForGame(string gameId) {
            throw new NotImplementedException();
        }

        public Task<ContentViewModel> GetPartialContentForGame(Guid gameId, ContentFilterRequest filterRequest)
        {
            return GetPartialContentForGame(gameId.ToString(), filterRequest);
        }

        private async Task<ContentViewModel> GetPartialContentForGame(string gameId, ContentFilterRequest filterRequest) {
            throw new NotImplementedException();
        }
    }
}
