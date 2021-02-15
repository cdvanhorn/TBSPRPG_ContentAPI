using System.Collections.Generic;

using TbspRpgLib.Aggregates;

namespace ContentApi.ViewModels {
    public class ContentViewModel {
        public ContentViewModel(ContentAggregate agg) {
            Id = agg.Id;
            Texts = new List<string>(agg.Text.Split('\n'));
        }

        public string Id { get; set; }

        public List<string> Texts { get; set; }
    }
}