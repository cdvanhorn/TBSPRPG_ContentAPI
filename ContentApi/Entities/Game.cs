using System;
using System.Collections.Generic;

namespace ContentApi.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public ICollection<Content> Contents { get; set; }
    }
}