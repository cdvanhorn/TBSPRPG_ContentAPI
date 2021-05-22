using System;

namespace ContentApi.Entities
{
    public class ConditionalSource
    {
        public Guid Id { get; set; }
        public Guid ContentKey { get; set; }
        public string JavaScript { get; set; }
    }
}