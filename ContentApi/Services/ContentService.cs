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
        Task<ContentViewModel> GetPartialContentForGame(string gameId, ContentFilterRequest filterRequest);
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

        public async Task<ContentViewModel> GetPartialContentForGame(string gameId, ContentFilterRequest filterRequest) {
            Aggregate agg = null;

            if(filterRequest.Direction == null || (filterRequest.Direction != null && filterRequest.IsForward())) {
                //direction specified and it's forward, or un specified so forward by default
                if(filterRequest.Start == null && filterRequest.Count == null) {
                    //no start specified so start at beginning and read everything
                    agg = await _aggregateService.BuildAggregate(
                        $"content_{gameId}",
                        "ContentAggregate"
                    );
                } else if(filterRequest.Start != null && filterRequest.Count == null) {
                    //there is a start and we're reading to the end
                    agg = await _aggregateService.BuildPartialAggregate(
                        $"content_{gameId}",
                        "ContentAggregate",
                        (ulong)filterRequest.Start.GetValueOrDefault()
                    );
                } else if(filterRequest.Start == null && filterRequest.Count != null) {
                    //we're starting at the beginning and reading given count
                    agg = await _aggregateService.BuildPartialAggregate(
                        $"content_{gameId}",
                        "ContentAggregate",
                        0,
                        filterRequest.Count.GetValueOrDefault()
                    );
                } else {
                    // they're both not null
                    //we have a start point and a count
                    agg = await _aggregateService.BuildPartialAggregate(
                        $"content_{gameId}",
                        "ContentAggregate",
                        (ulong)filterRequest.Start.GetValueOrDefault(),
                        filterRequest.Count.GetValueOrDefault()
                    );
                }
            } else if(filterRequest.Direction != null && filterRequest.IsBackward()) {
                //direction specified and it's backward
                if(filterRequest.Start == null && filterRequest.Count == null) {
                    //no start specified so start at end and read everything
                    agg = await _aggregateService.BuildAggregateReverse(
                        $"content_{gameId}",
                        "ContentAggregate"
                    );
                } else if(filterRequest.Start != null && filterRequest.Count == null) {
                    //there is a start and we're reading to the beginning
                    agg = await _aggregateService.BuildPartialAggregateReverse(
                        $"content_{gameId}",
                        "ContentAggregate",
                        filterRequest.Start.GetValueOrDefault()
                    );
                } else if(filterRequest.Start == null && filterRequest.Count != null) {
                    //we're starting at the end and reading given count, how do I get the last position
                    agg = await _aggregateService.BuildPartialAggregateReverse(
                        $"content_{gameId}",
                        "ContentAggregate",
                        -1,
                        filterRequest.Count.GetValueOrDefault()
                    );
                } else {
                    // they're both not null
                    //we have a start point and a count
                    agg = await _aggregateService.BuildPartialAggregateReverse(
                        $"content_{gameId}",
                        "ContentAggregate",
                        filterRequest.Start.GetValueOrDefault(),
                        filterRequest.Count.GetValueOrDefault()
                    );
                }
            } else if(filterRequest.Direction != null) {
                //direction specified but not parsable, log error, return bad request
            } else {
                //we shouldn't be here, log error, return a bad request
            }

            if(agg == null) {
                //log error, return a bad request, this shouldn't happen, even if there are no content events
            }

            return new ContentViewModel((ContentAggregate)agg);
        }
    }
}
