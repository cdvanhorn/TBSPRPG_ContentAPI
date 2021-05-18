using System;

namespace ContentApi.Entities
{
    public class Source
    {
        public Guid Id { get; set; }
        
        public Guid ContentKey { get; set; }
        
        public string Text { get; set; }
    }
}