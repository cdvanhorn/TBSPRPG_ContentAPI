using System;
using System.Collections.Generic;
using System.Linq;
using ContentApi.Entities;
using TbspRpgLib.Aggregates;

namespace ContentApi.ViewModels {
    public class ContentViewModel {
        public ContentViewModel(IEnumerable<Content> contents) {
            Texts = new List<string>();
            Index = 0;
            foreach (var content in contents)
            {
                Id = content.GameId;
                if(content.Position > Index)
                    Index = content.Position;
                Texts.Add(content.Text);
            }
        }

        public Guid Id { get; set; }

        public List<string> Texts { get; set; }

        public ulong Index { get; set; }
    }
}