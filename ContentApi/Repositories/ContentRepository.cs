using TbspRpgLib.Repositories;

namespace ContentApi.Repositories {
    public interface IContentRepository : IServiceTrackingRepository {

    }

    public class ContentRepository : ServiceTrackingRepository, IContentRepository {
        private readonly ContentContext _context;

        public ContentRepository(ContentContext context) : base(context) {
            _context = context;
        }
    }
}
