using TbspRpgLib.Services;
using TbspRpgLib.Aggregates;

using ContentApi.Repositories;
using ContentApi.ViewModels;

using System;
using System.Threading.Tasks;

namespace ContentApi.Services {
    public interface IContentService : IServiceTrackingService {
        Task<ContentViewModel> GetAllContentForGame(string gameId);
        Task<ContentViewModel> GetLatestForGame(string gameId);
    }

    public class ContentService : ServiceTrackingService, IContentService {
        private IContentRepository _repository;
        private IAggregateService _aggregateService;

        public ContentService(IContentRepository repository, IAggregateService aggregateService) : base(repository) {
            _repository = repository;
            _aggregateService = aggregateService;
        }

        public async Task<ContentViewModel> GetAllContentForGame(string gameId) {
            //need to fix this hard coding
            var agg = await _aggregateService.BuildAggregate($"content_{gameId}","ContentAggregate");
            return new ContentViewModel((ContentAggregate)agg);
        }

        public async Task<ContentViewModel> GetLatestForGame(string gameId) {
            var agg = await _aggregateService.BuildPartialAggregateLatest($"content_{gameId}", "ContentAggregate");
            return new ContentViewModel((ContentAggregate)agg);
        }
    }
}
