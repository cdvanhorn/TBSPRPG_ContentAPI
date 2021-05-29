using System;

namespace ContentApi.Entities
{
    public class Content
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public ulong Position { get; set; }
        public string Text { get; set; }
    }
}