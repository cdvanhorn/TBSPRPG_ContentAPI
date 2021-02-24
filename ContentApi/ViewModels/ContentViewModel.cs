using System.Collections.Generic;

using TbspRpgLib.Aggregates;

namespace ContentApi.ViewModels {
    public class ContentViewModel {
        public ContentViewModel(ContentAggregate agg) {
            Id = agg.Id;
            Texts = agg.Text;
            Index = agg.StreamPosition;
        }

        public string Id { get; set; }

        public List<string> Texts { get; set; }

        public ulong Index { get; set; }
    }
}