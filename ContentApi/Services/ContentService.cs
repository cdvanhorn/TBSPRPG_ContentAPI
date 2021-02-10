using TbspRpgLib.Services;
using ContentApi.Repositories;

namespace ContentApi.Services {
    public interface IContentService : IServiceTrackingService {

    }

    public class ContentService : ServiceTrackingService, IContentService {
        private IContentRepository _repository;

        public ContentService(IContentRepository repository) : base(repository) {
            _repository = repository;
        }
    }
}
